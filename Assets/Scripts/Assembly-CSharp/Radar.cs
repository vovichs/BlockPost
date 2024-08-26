using UnityEngine;
using UnityEngine.Rendering;

public class Radar : MonoBehaviour
{
	public static bool show = false;

	private Texture2D tBlack;

	private static Texture2D tRadar = null;

	private static Color no = new Color(0f, 0f, 0f, 0f);

	private Color a = new Color(1f, 1f, 1f, 0.2f);

	private Rect rRadar;

	private static GameObject goRadar = null;

	private static GameObject goRadarBack = null;

	public static void SetActive(bool val)
	{
		show = val;
		if (!val)
		{
			if (goRadarBack != null)
			{
				goRadarBack.name = "dead_radarback";
				Object.Destroy(goRadarBack);
			}
			if (goRadar != null)
			{
				goRadar.name = "dead_radar";
				Object.Destroy(goRadar);
			}
		}
	}

	private void LoadEnd()
	{
		tBlack = TEX.GetTextureByName("black");
	}

	private void OnResize()
	{
		rRadar = new Rect(GUIM.YRES(16f), GUIM.YRES(16f), GUIM.YRES(200f), GUIM.YRES(160f));
		if (Controll.csRadarCam != null)
		{
			Controll.csRadarCam.rect = new Rect(GUIM.YRES(16f) / (float)Screen.width, ((float)Screen.height - GUIM.YRES(16f) - GUIM.YRES(160f)) / (float)Screen.height, GUIM.YRES(200f) / (float)Screen.width, GUIM.YRES(160f) / (float)Screen.height);
		}
	}

	private void OnGUI()
	{
		if (show)
		{
			GUIM.DrawBoxBorder(rRadar, tBlack, 0.2f);
		}
	}

	public static void GenerateRadar()
	{
		tRadar = new Texture2D(Map.BLOCK_SIZE_X, Map.BLOCK_SIZE_Z, TextureFormat.RGBA32, false);
		tRadar.filterMode = FilterMode.Point;
		for (int i = 0; i < Map.BLOCK_SIZE_X; i++)
		{
			for (int j = 0; j < Map.BLOCK_SIZE_Z; j++)
			{
				tRadar.SetPixel(i, j, GetHighBlock(i, j));
			}
		}
		tRadar.Apply(false);
		if (goRadarBack != null)
		{
			goRadarBack.name = "dead_radarback";
			Object.Destroy(goRadarBack);
		}
		goRadarBack = new GameObject();
		goRadarBack.name = "MapRadarBack";
		goRadarBack.transform.localPosition = new Vector3(0f, -1f, 0f);
		goRadarBack.transform.localEulerAngles = Vector3.zero;
		goRadarBack.layer = 8;
		MeshFilter meshFilter = goRadarBack.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = goRadarBack.AddComponent<MeshRenderer>();
		meshRenderer.material = new Material(ContentLoader_.LoadMaterial("legacy_transparent"));
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		meshRenderer.receiveShadows = false;
		meshRenderer.material.SetTexture("_MainTex", null);
		meshRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.25f));
		MeshBuilder.Create();
		BlockBuilder.SetPivot(new Vector3(Map.BLOCK_SIZE_X, 0f, Map.BLOCK_SIZE_Z));
		BlockBuilder.SetTexCoord(0, 1, 1);
		BlockBuilder.BuildFaceBlock(1, 4, Vector3.zero, Color.white, Map.BLOCK_SIZE_X * 3, 0f, Map.BLOCK_SIZE_Z * 3);
		meshFilter.mesh = MeshBuilder.ToMesh();
		if (goRadar != null)
		{
			goRadar.name = "dead_radar";
			Object.Destroy(goRadar);
		}
		goRadar = new GameObject();
		goRadar.name = "MapRadar";
		goRadar.transform.localPosition = Vector3.zero;
		goRadar.transform.localEulerAngles = Vector3.zero;
		goRadar.layer = 8;
		MeshFilter meshFilter2 = goRadar.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer2 = goRadar.AddComponent<MeshRenderer>();
		meshRenderer2.material = new Material(ContentLoader_.LoadMaterial("legacy_transparent"));
		meshRenderer2.shadowCastingMode = ShadowCastingMode.Off;
		meshRenderer2.receiveShadows = false;
		meshRenderer2.material.SetTexture("_MainTex", tRadar);
		MeshBuilder.Create();
		BlockBuilder.SetPivot(new Vector3(0f, 0f, 0f));
		BlockBuilder.SetTexCoord(0, 1, 1);
		BlockBuilder.BuildFaceBlock(1, 4, Vector3.zero, Color.white, Map.BLOCK_SIZE_X, 0f, Map.BLOCK_SIZE_Z);
		meshFilter2.mesh = MeshBuilder.ToMesh();
	}

	private static Color GetHighBlock(int x, int z)
	{
		for (int num = Map.BLOCK_SIZE_Y - 1; num >= 0; num--)
		{
			if (Map.GetBlock(x, num, z) > 0)
			{
				Color lastColorFixed = Map.GetLastColorFixed();
				return new Color(lastColorFixed.r, lastColorFixed.g, lastColorFixed.b, lastColorFixed.a - 0.25f);
			}
		}
		return no;
	}
}
