using UnityEngine;

public class ReShaders : MonoBehaviour
{
	public Renderer[] renderers;

	public Material[] materials;

	public string[] shaders;

	private void Awake()
	{
		renderers = GetComponentsInChildren<Renderer>();
	}

	private void Start()
	{
		Renderer[] array = renderers;
		foreach (Renderer renderer in array)
		{
			materials = renderer.sharedMaterials;
			shaders = new string[materials.Length];
			for (int j = 0; j < materials.Length; j++)
			{
				shaders[j] = materials[j].shader.name;
			}
			for (int k = 0; k < materials.Length; k++)
			{
				materials[k].shader = Shader.Find(shaders[k]);
			}
		}
	}
}
