using UnityEngine;

public class FaceGen : MonoBehaviour
{
	private static Texture2D BuildInternal(int p0, int p1, int p2, int p3, int p4, int p5, int p6, int p7, int p8, int p9)
	{
		VCGen.SetColor(p0, p1, p2);
		VCGen.SetData(p3, p4, p5, p6, p7, p8, p9, 1);
		VCGen.LoadAddonTex(1);
		Texture2D texture2D = VCGen.GenerateTextureHead(ContentLoader_.LoadTexture("skin_head") as Texture2D, VCGen.skin_a, VCGen.skin_a, VCGen.tHair, VCGen.hair_a, VCGen.hair_b, VCGen.tEye, VCGen.eye_a, VCGen.eye_b, VCGen.tBeard, VCGen.tHat);
		Texture2D texture2D2 = new Texture2D(16, 16, TextureFormat.RGBA32, false);
		Color color = new Color(0f, 0f, 0f, 0f);
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				if (i == 0 || j == 0 || i == 15 || j == 15)
				{
					texture2D2.SetPixel(i, j, color);
					continue;
				}
				Color pixel = texture2D.GetPixel(i + 13, j + 35);
				texture2D2.SetPixel(i, j, pixel);
			}
		}
		texture2D2.filterMode = FilterMode.Point;
		texture2D2.Apply(false);
		Object.Destroy(texture2D);
		return texture2D2;
	}

	public static Texture2D Build(int p0, int p1, int p2, int p3, int p4, int p5, int p6, int p7, int p8, int p9)
	{
		return BuildInternal(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
	}

	public static Texture2D Build()
	{
		int[] cp = Controll.pl.cp;
		return BuildInternal(cp[0], cp[1], cp[2], cp[3], cp[4], cp[5], cp[6], cp[7], cp[8], cp[9]);
	}

	public static Texture2D BuildZombie(int idx, int team, int p0, int p1, int p2, int p3, int p4, int p5, int p6, int p7, int p8, int p9)
	{
		VCGen.LoadAddonTex(team);
		Texture2D org = VCGen.LoadSkin("", "head", -1, team);
		VCGen.skin_a = VCGen.skin_zombie_a;
		VCGen.skin_b = VCGen.skin_zombie_b;
		VCGen.tHat = ContentLoader_.LoadTexture("skin_beard0") as Texture2D;
		VCGen.tBeard = ContentLoader_.LoadTexture("skin_beard0") as Texture2D;
		int num = idx - idx / 4 * 4 + 1;
		VCGen.tEye = ContentLoader_.LoadTexture("skin_zombie_face_" + num) as Texture2D;
		Texture2D texture2D = VCGen.GenerateTextureHead(org, VCGen.skin_a, VCGen.skin_b, VCGen.tHair, VCGen.hair_a, VCGen.hair_b, VCGen.tEye, VCGen.eye_a, VCGen.eye_b, VCGen.tBeard, VCGen.tHat);
		Texture2D texture2D2 = new Texture2D(16, 16, TextureFormat.RGBA32, false);
		Color color = new Color(0f, 0f, 0f, 0f);
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				if (i == 0 || j == 0 || i == 15 || j == 15)
				{
					texture2D2.SetPixel(i, j, color);
					continue;
				}
				Color pixel = texture2D.GetPixel(i + 13, j + 35);
				texture2D2.SetPixel(i, j, pixel);
			}
		}
		texture2D2.filterMode = FilterMode.Point;
		texture2D2.Apply(false);
		Object.Destroy(texture2D);
		return texture2D2;
	}
}
