using UnityEngine;

public class MainBack : MonoBehaviour
{
	private const int maxline = 10;

	private static GameObject goBack = null;

	private static Texture2D[] tBack = new Texture2D[4];

	private static Sprite[] sBack = new Sprite[4];

	private static Material[,] mat = new Material[10, 4];

	private static GameObject[,] goW = new GameObject[10, 4];

	private static GameObject[,] goTV = new GameObject[10, 4];

	private static Texture2D tTV = null;

	private static Sprite sTV = null;

	private static Material mTV = null;

	private static bool clampspeed = false;

	public static void Create()
	{
		if (goBack != null)
		{
			return;
		}
		goBack = new GameObject();
		goBack.name = "MenuBack";
		goBack.transform.position = Vector3.zero;
		tTV = ContentLoader_.LoadTexture("fx_tv") as Texture2D;
		for (int i = 0; i < 4; i++)
		{
			tBack[i] = ContentLoader_.LoadTexture("back" + i) as Texture2D;
			sBack[i] = Sprite.Create(tBack[i], new Rect(0f, 0f, tBack[i].width, tBack[i].height), new Vector2(0f, 0f), 32f, 0u, SpriteMeshType.FullRect, Vector4.zero);
		}
		sTV = Sprite.Create(tTV, new Rect(0f, 0f, tTV.width, tTV.height), new Vector2(0f, 0f), 32f, 0u, SpriteMeshType.FullRect, Vector4.zero);
		mTV = new Material(Shader.Find("Sprites/Diffuse"));
		int num = 0;
		for (int j = 0; j < 10; j++)
		{
			CreateSprite(num, 0, j);
			CreateSprite(num, 1, j);
			CreateSprite(num, 2, j);
			CreateSprite(num, 3, j);
			num++;
			if (num >= 4)
			{
				num = 0;
			}
		}
		SetTransparent(0.75f, new Color(1f, 1f, 1f, 0.95f));
		if (Main.winter)
		{
			GameObject obj = Object.Instantiate(Resources.Load("prefabs/guisnow") as GameObject);
			obj.transform.SetParent(goBack.transform);
			obj.transform.position = new Vector3(0f, -4.55f, 0f);
		}
		else if (Main.halloween)
		{
			GameObject obj2 = Object.Instantiate(Resources.Load("prefabs/guibat") as GameObject);
			obj2.transform.SetParent(goBack.transform);
			obj2.transform.position = new Vector3(0f, -14.55f, 0f);
		}
	}

	public static void SetTransparent(float val, Color c)
	{
		mTV.SetColor("_Color", new Color(1f, 1f, 1f, val));
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				mat[i, j].SetColor("_Color", c);
			}
		}
	}

	public static void ClampSpeed(bool val)
	{
		clampspeed = val;
	}

	public static void Clear()
	{
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				Object.Destroy(goW[i, j]);
				Object.Destroy(goTV[i, j]);
			}
		}
		Object.Destroy(goBack);
	}

	private static void CreateSprite(int idx, int posx, int posy)
	{
		Texture2D texture2D = tBack[idx];
		GameObject gameObject = new GameObject();
		gameObject.name = "spr_" + idx + " " + posx + " " + posy;
		SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = sBack[idx];
		Vector3 vector = Main.cam.ScreenToWorldPoint(new Vector3(-512f, 0f, 0f));
		gameObject.transform.position = new Vector3(vector.x + (float)(posx * texture2D.width / 32), vector.y + (float)(posy * texture2D.height / 32), 10f);
		spriteRenderer.material = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
		mat[posy, posx] = spriteRenderer.material;
		goW[posy, posx] = gameObject;
		goW[posy, posx].layer = 8;
		goW[posy, posx].transform.SetParent(goBack.transform);
		gameObject = new GameObject();
		gameObject.name = "tv_" + idx + " " + posx + " " + posy;
		spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = sTV;
		spriteRenderer.sortingOrder = 1;
		spriteRenderer.sharedMaterial = mTV;
		gameObject.transform.position = new Vector3(vector.x + (float)(posx * texture2D.width / 32), vector.y + (float)(posy * texture2D.height / 32), 10f);
		goTV[posy, posx] = gameObject;
		goTV[posy, posx].layer = 8;
		goTV[posy, posx].transform.SetParent(goBack.transform);
	}

	private void Update()
	{
		if (!(goW[0, 0] == null))
		{
			OffsetMaterial(0, 0.075f);
			OffsetMaterial(2, -0.2f);
			OffsetMaterial(8, -0.12f);
			OffsetMaterial(5, 0.05f);
			OffsetMaterial(9, -0.05f);
			OffsetMaterial(10, 0.05f);
			OffsetMaterial(13, -0.25f);
		}
	}

	private void OffsetMaterial(int line, float speed)
	{
		if (line >= 10)
		{
			return;
		}
		if (clampspeed)
		{
			if (speed < -0.05f)
			{
				speed = -0.05f;
			}
			else if (speed > 0.05f)
			{
				speed = 0.05f;
			}
		}
		for (int i = 0; i < 4 && !(mat[line, i] == null); i++)
		{
			Vector2 textureOffset = mat[line, i].GetTextureOffset("_MainTex");
			mat[line, i].SetTextureOffset("_MainTex", new Vector2(textureOffset.x + speed * Time.deltaTime, 0f));
		}
	}
}
