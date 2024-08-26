using System.Collections;
using UnityEngine;

public class MapLightBaker : MonoBehaviour
{
	public int samples = 32;

	public float maxRange = 1.5f;

	public float minRange = 1E-10f;

	public float intensity = 2f;

	public bool resetExistingAlpha = true;

	public bool averageColors;

	public bool averageNormals;

	public void Start()
	{
		StartCoroutine("StartBakeLightDirect");
	}

	private IEnumerator StartBakeLightAO()
	{
		yield return new WaitForSeconds(1f);
		BakeLightAO();
	}

	private IEnumerator StartBakeLightDirect()
	{
		yield return new WaitForSeconds(1f);
		BakeLightDirect();
	}

	public void BakeLightAO()
	{
		if (Map.chunk == null)
		{
			return;
		}
		for (int i = 0; i < Map.MAP_SIZE_X; i++)
		{
			for (int j = 0; j < Map.MAP_SIZE_Y; j++)
			{
				for (int k = 0; k < Map.MAP_SIZE_Z; k++)
				{
					if (Map.chunk[i, j, k] != null && !(Map.chunk[i, j, k].go == null))
					{
						BakeLightAOGameObject(Map.chunk[i, j, k].go);
					}
				}
			}
		}
	}

	public void BakeLightDirect()
	{
		if (Map.chunk == null)
		{
			return;
		}
		for (int i = 0; i < Map.MAP_SIZE_X; i++)
		{
			for (int j = 0; j < Map.MAP_SIZE_Y; j++)
			{
				for (int k = 0; k < Map.MAP_SIZE_Z; k++)
				{
					if (Map.chunk[i, j, k] != null && !(Map.chunk[i, j, k].go == null))
					{
						BakeLightDirectGameObject(Map.chunk[i, j, k].go);
					}
				}
			}
		}
	}

	public void BakeLightAOGameObject(GameObject subject)
	{
		RaycastHit hitInfo = default(RaycastHit);
		GameObject gameObject = GameObject.Find("p_pointsample");
		bool flag = gameObject == null;
		MeshFilter[] componentsInChildren = subject.GetComponentsInChildren<MeshFilter>();
		int num = 0;
		MeshFilter[] array = componentsInChildren;
		foreach (MeshFilter meshFilter in array)
		{
			num += meshFilter.sharedMesh.vertices.Length;
		}
		int sample = samples;
		array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Mesh sharedMesh = array[i].sharedMesh;
			Vector3[] vertices = sharedMesh.vertices;
			Color[] array2 = sharedMesh.colors;
			if (array2.Length == 0)
			{
				Debug.Log("Mesh is missing color data.  Supplying...");
				array2 = new Color[vertices.Length];
			}
			if (resetExistingAlpha)
			{
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].a = 1f;
				}
			}
			Vector3[] array3 = new Vector3[sharedMesh.normals.Length];
			if (array3.Length == 0)
			{
				sharedMesh.RecalculateNormals();
			}
			if (averageNormals)
			{
				Mesh mesh = new Mesh();
				mesh.vertices = sharedMesh.vertices;
				mesh.normals = sharedMesh.normals;
				mesh.tangents = sharedMesh.tangents;
				mesh.triangles = sharedMesh.triangles;
				mesh.RecalculateBounds();
				mesh.RecalculateNormals();
				array3 = mesh.normals;
				Object.DestroyImmediate(mesh);
			}
			else
			{
				array3 = sharedMesh.normals;
			}
			int num2 = 0;
			num2 = vertices.Length;
			for (int k = 0; k < num2; k++)
			{
				Vector3 vector = array3[k];
				Vector3 vector2 = subject.transform.TransformPoint(vertices[k]);
				Vector3 vector3 = subject.transform.TransformPoint(vertices[k] + vector) - vector2;
				vector3.Normalize();
				float num3 = 0f;
				if (gameObject != null)
				{
					Vector3 position = gameObject.transform.position;
					float x = gameObject.GetComponent<MeshRenderer>().bounds.extents.x;
					Vector3 forward = gameObject.transform.forward;
					if (Mathf.Abs((position - vector2).magnitude) < x)
					{
						Vector3.Dot(vector3, forward);
						float num8 = 0.6f;
					}
				}
				for (int l = 0; l < samples; l++)
				{
					float num4 = 180f / 2f;
					float x2 = 180f * Random.value - num4;
					float y = 180f * Random.value - num4;
					float z = 180f * Random.value - num4;
					Vector3 vector4 = Quaternion.Euler(x2, y, z) * Vector3.up;
					Vector3 vector5 = Quaternion.FromToRotation(Vector3.up, vector3) * vector4;
					Vector3 vector6 = Vector3.Reflect(vector5, vector3);
					vector5 *= maxRange / vector5.magnitude;
					if (Physics.Linecast(vector2 - vector6 * 0.1f, vector2 + vector5, out hitInfo) && hitInfo.distance > minRange)
					{
						num3 += Mathf.Clamp01(1f - hitInfo.distance / maxRange);
					}
				}
				num3 = Mathf.Clamp01(1f - num3 * intensity / (float)samples);
				array2[k].a = array2[k].a * num3;
				array2[k].r = array2[k].a;
				array2[k].g = array2[k].a;
				array2[k].b = array2[k].a;
			}
			if (averageColors)
			{
				int[] triangles = sharedMesh.triangles;
				num2 = triangles.Length;
				for (int k = 0; k < num2; k += 3)
				{
					int num5 = triangles[k];
					int num6 = triangles[k + 1];
					int num7 = triangles[k + 2];
					Color color = array2[num5];
					Color color2 = array2[num6];
					Color color3 = array2[num7];
					Color color4 = default(Color);
					color4.a = (color.a + color2.a + color3.a) / 3f;
					color.a += (color4.a - color.a) / 2f;
					color2.a += (color4.a - color2.a) / 2f;
					color3.a += (color4.a - color3.a) / 2f;
					array2[num5] = color;
					array2[num6] = color2;
					array2[num7] = color3;
				}
			}
			sharedMesh.colors = array2;
		}
	}

	public void BakeLightDirectGameObject(GameObject subject)
	{
		Vector3 eulerAngles = GameObject.Find("Dlight").transform.eulerAngles;
		maxRange = 512f;
		RaycastHit hitInfo = default(RaycastHit);
		MeshFilter[] componentsInChildren = subject.GetComponentsInChildren<MeshFilter>();
		int num = 0;
		MeshFilter[] array = componentsInChildren;
		foreach (MeshFilter meshFilter in array)
		{
			num += meshFilter.sharedMesh.vertices.Length;
		}
		int sample = samples;
		array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Mesh sharedMesh = array[i].sharedMesh;
			Vector3[] vertices = sharedMesh.vertices;
			Color[] array2 = sharedMesh.colors;
			if (array2.Length == 0)
			{
				Debug.Log("Mesh is missing color data.  Supplying...");
				array2 = new Color[vertices.Length];
			}
			if (resetExistingAlpha)
			{
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].a = 1f;
				}
			}
			Vector3[] array3 = new Vector3[sharedMesh.normals.Length];
			if (array3.Length == 0)
			{
				sharedMesh.RecalculateNormals();
			}
			if (averageNormals)
			{
				Mesh mesh = new Mesh();
				mesh.vertices = sharedMesh.vertices;
				mesh.normals = sharedMesh.normals;
				mesh.tangents = sharedMesh.tangents;
				mesh.triangles = sharedMesh.triangles;
				mesh.RecalculateBounds();
				mesh.RecalculateNormals();
				array3 = mesh.normals;
				Object.DestroyImmediate(mesh);
			}
			else
			{
				array3 = sharedMesh.normals;
			}
			int num2 = 0;
			num2 = vertices.Length;
			for (int k = 0; k < num2; k++)
			{
				Vector3 vector = array3[k];
				Vector3 vector2 = subject.transform.TransformPoint(vertices[k]);
				Vector3 vector3 = subject.transform.TransformPoint(vertices[k] + vector) - vector2;
				vector3.Normalize();
				float num3 = 0f;
				for (int l = 0; l < samples; l++)
				{
					float num7 = 180f / 2f;
					float x = eulerAngles.x;
					float y = eulerAngles.y;
					float z = eulerAngles.z;
					Vector3 vector4 = Quaternion.Euler(x, y, z) * Vector3.up;
					Vector3 vector5 = Quaternion.FromToRotation(Vector3.up, vector3) * vector4;
					Vector3 vector6 = Vector3.Reflect(vector5, vector3);
					vector5 *= maxRange / vector5.magnitude;
					if (Physics.Linecast(vector2 - vector6 * 0.1f, vector2 + vector5, out hitInfo) && hitInfo.distance > minRange)
					{
						num3 += Mathf.Clamp01(1f - hitInfo.distance / maxRange);
					}
				}
				num3 = Mathf.Clamp01(1f - num3 * intensity / (float)samples);
				array2[k].a = array2[k].a * num3;
				array2[k].r = array2[k].a;
				array2[k].g = array2[k].a;
				array2[k].b = array2[k].a;
			}
			if (averageColors)
			{
				int[] triangles = sharedMesh.triangles;
				num2 = triangles.Length;
				for (int k = 0; k < num2; k += 3)
				{
					int num4 = triangles[k];
					int num5 = triangles[k + 1];
					int num6 = triangles[k + 2];
					Color color = array2[num4];
					Color color2 = array2[num5];
					Color color3 = array2[num6];
					Color color4 = default(Color);
					color4.a = (color.a + color2.a + color3.a) / 3f;
					color.a += (color4.a - color.a) / 2f;
					color2.a += (color4.a - color2.a) / 2f;
					color3.a += (color4.a - color3.a) / 2f;
					array2[num4] = color;
					array2[num5] = color2;
					array2[num6] = color3;
				}
			}
			sharedMesh.colors = array2;
		}
	}
}
