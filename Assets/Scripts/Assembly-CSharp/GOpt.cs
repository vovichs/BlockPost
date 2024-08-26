using UnityEngine;
using UnityEngine.Rendering;

public class GOpt : MonoBehaviour
{
	public static bool custom_render = false;

	public static Color custom_fog;

	public static int custom_fogstart;

	public static int custom_fogend;

	public static float custom_intensity = 1f;

	public static void SetShaders(int val)
	{
		for (int i = 0; i < ContentLoader_.materialList.Count; i++)
		{
			Material material = ContentLoader_.materialList[i] as Material;
			if (material.name == "legacy_transparent")
			{
				if (val == 0)
				{
					material.shader = Shader.Find("Sprites/Default");
				}
				else
				{
					material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
				}
			}
			else if (material.name == "matlas")
			{
				if (val == 0)
				{
					material.shader = Shader.Find("VertexColorLit");
				}
				else
				{
					material.shader = Shader.Find("Standard (Vertex Color)");
				}
			}
			else
			{
				if (material.name == "muzzle")
				{
					continue;
				}
				if (material.name == "standard")
				{
					if (val == 0)
					{
						material.shader = Shader.Find("Unlit/Color");
					}
					else
					{
						material.shader = Shader.Find("Standard (Vertex Color)");
					}
				}
				else if (!(material.name == "standard_fade") && !(material.name == "unlit_transparent") && material.name == "vertex_color")
				{
					if (val == 0)
					{
						material.shader = Shader.Find("VertexColorLit");
					}
					else
					{
						material.shader = Shader.Find("Standard (Vertex Color)");
					}
				}
			}
		}
		if (val == 0)
		{
			RenderSettings.ambientMode = AmbientMode.Trilight;
			RenderSettings.ambientSkyColor = new Color(0.49803922f, 0.49803922f, 0.49803922f);
			RenderSettings.ambientEquatorColor = new Color(20f / 51f, 23f / 51f, 0.49803922f);
			RenderSettings.ambientGroundColor = new Color(23f / 51f, 0.49803922f, 0.49803922f);
		}
		else
		{
			RenderSettings.ambientMode = AmbientMode.Skybox;
			RenderSettings.ambientIntensity = 1f;
		}
		if (val == 0)
		{
			Shader.globalMaximumLOD = 100;
		}
		else
		{
			Shader.globalMaximumLOD = 200;
		}
		if (GUIOptions.globalpresetload && GUIOptions.globalpreset == 0)
		{
			DisableEffects();
		}
		SetDistance();
	}

	public static void DisableEffects()
	{
		GUIOptions.ssao = 0;
		PlayerPrefs.SetInt("ssao", GUIOptions.ssao);
		if (Controll.aoe != null)
		{
			Controll.aoe.enabled = false;
		}
	}

	public static void SetDistance()
	{
		if (Controll.csCam == null)
		{
			return;
		}
		Light component = GameObject.Find("Dlight").GetComponent<Light>();
		if (custom_render)
		{
			component.intensity = custom_intensity * 0.75f;
		}
		else
		{
			component.intensity = 0.75f;
		}
		float num = 256f;
		if (GUIOptions.distanceview == 0)
		{
			num = 64f;
		}
		else if (GUIOptions.distanceview == 1)
		{
			num = 96f;
		}
		else if (GUIOptions.distanceview == 2)
		{
			num = 128f;
		}
		else if (GUIOptions.distanceview == 3)
		{
			num = 160f;
		}
		else if (GUIOptions.distanceview == 4)
		{
			num = 192f;
		}
		else if (GUIOptions.distanceview == 5)
		{
			num = 224f;
		}
		else if (GUIOptions.distanceview == 6)
		{
			num = 256f;
		}
		float[] array = new float[32];
		for (int i = 0; i < 32; i++)
		{
			array[i] = num;
		}
		Controll.csCam.layerCullDistances = array;
		Color color = new Color(40f / 51f, 46f / 51f, 1f, 1f);
		if (custom_render)
		{
			color = custom_fog;
		}
		if (num < 256f && num < (float)(Map.MAP_SIZE_X * Map.CHUNK_SIZE_X))
		{
			float num2 = num - 32f;
			float num3 = num;
			if (custom_render)
			{
				if ((float)custom_fogstart < num2)
				{
					num2 = custom_fogstart;
				}
				if ((float)custom_fogend < num3)
				{
					num3 = custom_fogend;
				}
			}
			Controll.csCam.clearFlags = CameraClearFlags.Color;
			Controll.csCam.backgroundColor = color;
			RenderSettings.fogColor = color;
			RenderSettings.fogStartDistance = num2;
			RenderSettings.fogEndDistance = num3;
			QualitySettings.shadowDistance = num;
			return;
		}
		float num4 = 128f;
		float num5 = 256f;
		if (custom_render)
		{
			if ((float)custom_fogstart < num4)
			{
				num4 = custom_fogstart;
			}
			if ((float)custom_fogend < num5)
			{
				num5 = custom_fogend;
			}
		}
		Controll.csCam.clearFlags = CameraClearFlags.Skybox;
		RenderSettings.fogColor = color;
		RenderSettings.fogStartDistance = num4;
		RenderSettings.fogEndDistance = num5;
		QualitySettings.shadowDistance = 128f;
	}
}
