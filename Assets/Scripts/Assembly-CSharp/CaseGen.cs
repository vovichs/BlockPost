using UnityEngine;
using UnityEngine.Rendering;

public class CaseGen : MonoBehaviour
{
	private static Texture2D tTex;

	private static int width;

	private static int height;

	private static MeshFilter mf;

	private static MeshRenderer mr;

	private void Start()
	{
		Build("case_battle");
	}

	public static GameObject Build(string _name)
	{
		tTex = Resources.Load("cases/" + _name) as Texture2D;
		if (tTex == null)
		{
			tTex = ContentLoader_.LoadTexture(_name) as Texture2D;
		}
		width = tTex.width;
		height = tTex.height;
		GameObject gameObject = new GameObject();
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
		gameObject.name = "case_" + Time.time;
		GameObject gameObject2 = CreateGameObject("box");
		mr.material.SetTexture("_MainTex", tTex);
		MeshBuilder.Create();
		BlockBuilder.SetTexCoord(0, width, height);
		BlockBuilder.SetPivot(new Vector3(64f, 0f, 32f));
		BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 128f, 52f, 64f, 128f, 0f, 128f, 64f);
		BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 128f, 52f, 64f, 0f, 192f, 128f, 64f);
		BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 128f, 52f, 64f, 0f, 76f, 128f, 52f);
		BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 128f, 52f, 64f, 0f, 76f, 128f, 52f);
		BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 128f, 52f, 64f, 128f, 76f, 64f, 52f);
		BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 128f, 52f, 64f, 128f, 76f, 64f, 52f);
		mf.sharedMesh = MeshBuilder.ToMesh();
		GameObject obj = CreateGameObject("cover");
		obj.transform.position = new Vector3(0f, 52f, -32f);
		mr.material.SetTexture("_MainTex", tTex);
		MeshBuilder.Create();
		BlockBuilder.SetTexCoord(0, width, height);
		BlockBuilder.SetPivot(new Vector3(64f, 0f, 0f));
		BlockBuilder.BuildFaceBlockTex(4, new Vector3(0f, 0f, 0f), Color.white, 128f, 12f, 64f, 0f, 0f, 128f, 64f);
		BlockBuilder.BuildFaceBlockTex(5, new Vector3(0f, 0f, 0f), Color.white, 128f, 12f, 64f, 128f, 0f, 128f, 64f);
		BlockBuilder.BuildFaceBlockTex(0, new Vector3(0f, 0f, 0f), Color.white, 128f, 12f, 64f, 0f, 64f, 128f, 12f);
		BlockBuilder.BuildFaceBlockTex(1, new Vector3(0f, 0f, 0f), Color.white, 128f, 12f, 64f, 0f, 64f, 128f, 12f);
		BlockBuilder.BuildFaceBlockTex(2, new Vector3(0f, 0f, 0f), Color.white, 128f, 12f, 64f, 128f, 64f, 64f, 12f);
		BlockBuilder.BuildFaceBlockTex(3, new Vector3(0f, 0f, 0f), Color.white, 128f, 12f, 64f, 128f, 64f, 64f, 12f);
		mf.sharedMesh = MeshBuilder.ToMesh();
		gameObject2.transform.parent = gameObject.transform;
		obj.transform.parent = gameObject.transform;
		gameObject.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f);
		gameObject2.layer = 9;
		obj.layer = 9;
		gameObject.layer = 9;
		return gameObject;
	}

	public static GameObject CreateGameObject(string newname)
	{
		GameObject obj = new GameObject();
		obj.name = newname;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localEulerAngles = Vector3.zero;
		mf = obj.AddComponent<MeshFilter>();
		mr = obj.AddComponent<MeshRenderer>();
		mr.material = new Material(ContentLoader_.LoadMaterial("vertex_color"));
		mr.shadowCastingMode = ShadowCastingMode.Off;
		mr.material.SetTexture("_MainTex", null);
		MeshBuilder.Create();
		BlockBuilder.SetPivot(new Vector3(0f, 0f, 0f));
		BlockBuilder.SetTexCoord(0, 1, 1);
		return obj;
	}
}
