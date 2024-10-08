using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace AmplifyMotion
{
	internal class ParticleState : MotionState
	{
		public class Particle
		{
			public int refCount;

			public Matrix4x4 prevLocalToWorld;

			public Matrix4x4 currLocalToWorld;
		}

		public ParticleSystem m_particleSystem;

		public ParticleSystemRenderer m_renderer;

		private Mesh m_mesh;

		private ParticleSystem.RotationOverLifetimeModule rotationOverLifetime;

		private ParticleSystem.RotationBySpeedModule rotationBySpeed;

		private ParticleSystem.Particle[] m_particles;

		private Dictionary<uint, Particle> m_particleDict;

		private List<uint> m_listToRemove;

		private Stack<Particle> m_particleStack;

		private int m_capacity;

		private MaterialDesc[] m_sharedMaterials;

		public bool m_moved;

		private bool m_wasVisible;

		private static HashSet<AmplifyMotionObjectBase> m_uniqueWarnings = new HashSet<AmplifyMotionObjectBase>();

		public ParticleState(AmplifyMotionCamera owner, AmplifyMotionObjectBase obj)
			: base(owner, obj)
		{
			m_particleSystem = m_obj.GetComponent<ParticleSystem>();
			m_renderer = m_particleSystem.GetComponent<ParticleSystemRenderer>();
			rotationOverLifetime = m_particleSystem.rotationOverLifetime;
			rotationBySpeed = m_particleSystem.rotationBySpeed;
		}

		private Mesh CreateBillboardMesh()
		{
			int[] triangles = new int[6] { 0, 1, 2, 2, 3, 0 };
			Vector3[] vertices = new Vector3[4]
			{
				new Vector3(-0.5f, -0.5f, 0f),
				new Vector3(0.5f, -0.5f, 0f),
				new Vector3(0.5f, 0.5f, 0f),
				new Vector3(-0.5f, 0.5f, 0f)
			};
			Vector2[] uv = new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(1f, 0f),
				new Vector2(1f, 1f),
				new Vector2(0f, 1f)
			};
			return new Mesh
			{
				vertices = vertices,
				uv = uv,
				triangles = triangles
			};
		}

		private Mesh CreateStretchedBillboardMesh()
		{
			int[] triangles = new int[6] { 0, 1, 2, 2, 3, 0 };
			Vector3[] vertices = new Vector3[4]
			{
				new Vector3(0f, -0.5f, -1f),
				new Vector3(0f, -0.5f, 0f),
				new Vector3(0f, 0.5f, 0f),
				new Vector3(0f, 0.5f, -1f)
			};
			Vector2[] uv = new Vector2[4]
			{
				new Vector2(1f, 1f),
				new Vector2(0f, 1f),
				new Vector2(0f, 0f),
				new Vector2(1f, 0f)
			};
			return new Mesh
			{
				vertices = vertices,
				uv = uv,
				triangles = triangles
			};
		}

		internal override void Initialize()
		{
			if (m_renderer == null)
			{
				if (!m_uniqueWarnings.Contains(m_obj))
				{
					Debug.LogWarning("[AmplifyMotion] Missing/Invalid Particle Renderer in object " + m_obj.name + ". Skipping.");
					m_uniqueWarnings.Add(m_obj);
				}
				m_error = true;
				return;
			}
			base.Initialize();
			if (m_renderer.renderMode == ParticleSystemRenderMode.Mesh)
			{
				m_mesh = m_renderer.mesh;
			}
			else if (m_renderer.renderMode == ParticleSystemRenderMode.Stretch)
			{
				m_mesh = CreateStretchedBillboardMesh();
			}
			else
			{
				m_mesh = CreateBillboardMesh();
			}
			m_sharedMaterials = ProcessSharedMaterials(m_renderer.sharedMaterials);
			m_capacity = m_particleSystem.maxParticles;
			m_particleDict = new Dictionary<uint, Particle>(m_capacity);
			m_particles = new ParticleSystem.Particle[m_capacity];
			m_listToRemove = new List<uint>(m_capacity);
			m_particleStack = new Stack<Particle>(m_capacity);
			for (int i = 0; i < m_capacity; i++)
			{
				m_particleStack.Push(new Particle());
			}
			m_wasVisible = false;
		}

		private void RemoveDeadParticles()
		{
			m_listToRemove.Clear();
			Dictionary<uint, Particle>.Enumerator enumerator = m_particleDict.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, Particle> current = enumerator.Current;
				if (current.Value.refCount <= 0)
				{
					m_particleStack.Push(current.Value);
					if (!m_listToRemove.Contains(current.Key))
					{
						m_listToRemove.Add(current.Key);
					}
				}
				else
				{
					current.Value.refCount = 0;
				}
			}
			for (int i = 0; i < m_listToRemove.Count; i++)
			{
				m_particleDict.Remove(m_listToRemove[i]);
			}
		}

		internal override void UpdateTransform(CommandBuffer updateCB, bool starting)
		{
			if (!m_initialized || m_capacity != m_particleSystem.maxParticles)
			{
				Initialize();
				return;
			}
			if (!starting && m_wasVisible)
			{
				Dictionary<uint, Particle>.Enumerator enumerator = m_particleDict.GetEnumerator();
				while (enumerator.MoveNext())
				{
					Particle value = enumerator.Current.Value;
					value.prevLocalToWorld = value.currLocalToWorld;
				}
			}
			m_moved = true;
			int particles = m_particleSystem.GetParticles(m_particles);
			Matrix4x4 matrix4x = Matrix4x4.TRS(m_transform.position, m_transform.rotation, Vector3.one);
			bool flag = (rotationOverLifetime.enabled && rotationOverLifetime.separateAxes) || (rotationBySpeed.enabled && rotationBySpeed.separateAxes);
			for (int i = 0; i < particles; i++)
			{
				uint randomSeed = m_particles[i].randomSeed;
				bool flag2 = false;
				Particle value2;
				if (!m_particleDict.TryGetValue(randomSeed, out value2) && m_particleStack.Count > 0)
				{
					value2 = (m_particleDict[randomSeed] = m_particleStack.Pop());
					flag2 = true;
				}
				if (value2 == null)
				{
					continue;
				}
				float currentSize = m_particles[i].GetCurrentSize(m_particleSystem);
				Vector3 s = new Vector3(currentSize, currentSize, currentSize);
				Matrix4x4 currLocalToWorld;
				if (m_renderer.renderMode == ParticleSystemRenderMode.Mesh)
				{
					Matrix4x4 matrix4x2 = Matrix4x4.TRS(q: (!flag) ? Quaternion.AngleAxis(m_particles[i].rotation, m_particles[i].axisOfRotation) : Quaternion.Euler(m_particles[i].rotation3D), pos: m_particles[i].position, s: s);
					currLocalToWorld = ((m_particleSystem.simulationSpace != ParticleSystemSimulationSpace.World) ? (matrix4x * matrix4x2) : matrix4x2);
				}
				else if (m_renderer.renderMode == ParticleSystemRenderMode.Billboard)
				{
					if (m_particleSystem.simulationSpace == ParticleSystemSimulationSpace.Local)
					{
						m_particles[i].position = matrix4x.MultiplyPoint(m_particles[i].position);
					}
					Quaternion quaternion = ((!flag) ? Quaternion.AngleAxis(m_particles[i].rotation, Vector3.back) : Quaternion.Euler(0f - m_particles[i].rotation3D.x, 0f - m_particles[i].rotation3D.y, m_particles[i].rotation3D.z));
					currLocalToWorld = Matrix4x4.TRS(m_particles[i].position, m_owner.Transform.rotation * quaternion, s);
				}
				else
				{
					currLocalToWorld = Matrix4x4.identity;
				}
				value2.refCount = 1;
				value2.currLocalToWorld = currLocalToWorld;
				if (flag2)
				{
					value2.prevLocalToWorld = value2.currLocalToWorld;
				}
			}
			if (starting || !m_wasVisible)
			{
				Dictionary<uint, Particle>.Enumerator enumerator2 = m_particleDict.GetEnumerator();
				while (enumerator2.MoveNext())
				{
					Particle value3 = enumerator2.Current.Value;
					value3.prevLocalToWorld = value3.currLocalToWorld;
				}
			}
			RemoveDeadParticles();
			m_wasVisible = m_renderer.isVisible;
		}

		internal override void RenderVectors(Camera camera, CommandBuffer renderCB, float scale, Quality quality)
		{
			if (!m_initialized || m_error || !m_renderer.isVisible)
			{
				return;
			}
			bool flag = ((int)m_owner.Instance.CullingMask & (1 << m_obj.gameObject.layer)) != 0;
			if (flag && (!flag || !m_moved))
			{
				return;
			}
			int num = (flag ? m_owner.Instance.GenerateObjectId(m_obj.gameObject) : 255);
			renderCB.SetGlobalFloat("_AM_OBJECT_ID", (float)num * 0.003921569f);
			renderCB.SetGlobalFloat("_AM_MOTION_SCALE", flag ? scale : 0f);
			int num2 = ((quality != 0) ? 2 : 0);
			for (int i = 0; i < m_sharedMaterials.Length; i++)
			{
				MaterialDesc materialDesc = m_sharedMaterials[i];
				int shaderPass = num2 + (materialDesc.coverage ? 1 : 0);
				if (materialDesc.coverage)
				{
					Texture mainTexture = materialDesc.material.mainTexture;
					if (mainTexture != null)
					{
						materialDesc.propertyBlock.SetTexture("_MainTex", mainTexture);
					}
					if (materialDesc.cutoff)
					{
						materialDesc.propertyBlock.SetFloat("_Cutoff", materialDesc.material.GetFloat("_Cutoff"));
					}
				}
				Dictionary<uint, Particle>.Enumerator enumerator = m_particleDict.GetEnumerator();
				while (enumerator.MoveNext())
				{
					KeyValuePair<uint, Particle> current = enumerator.Current;
					Matrix4x4 value = m_owner.PrevViewProjMatrixRT * current.Value.prevLocalToWorld;
					renderCB.SetGlobalMatrix("_AM_MATRIX_PREV_MVP", value);
					renderCB.DrawMesh(m_mesh, current.Value.currLocalToWorld, m_owner.Instance.SolidVectorsMaterial, i, shaderPass, materialDesc.propertyBlock);
				}
			}
		}
	}
}
