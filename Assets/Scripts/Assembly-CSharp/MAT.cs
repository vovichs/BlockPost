using UnityEngine;

public class MAT : MonoBehaviour
{
	private static Material currmat;

	public static void Init()
	{
	}

	public static void Test()
	{
	}

	public static Material Get(string _name)
	{
		Material material = ContentLoader_.LoadMaterial(_name);
		if (material != null)
		{
			return material;
		}
		material = Resources.Load("Materials/" + _name) as Material;
		if (material != null)
		{
			return material;
		}
		return Resources.Load("Materials/matError") as Material;
	}
}
