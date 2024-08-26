using UnityEngine;

public class DevDraw : MonoBehaviour
{
	private MaterialPropertyBlock block;

	private Material mat;

	private void Start()
	{
		mat = new Material(Shader.Find("VertexLit"));
		mat.SetTexture("_MainTex", Atlas.mat.GetTexture(0));
		block = new MaterialPropertyBlock();
	}

	private void Update()
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
					Map.CChunk cChunk = Map.chunk[i, j, k];
					if (cChunk != null && !(cChunk.go == null) && !(cChunk.mf == null) && !(cChunk.mf.sharedMesh == null))
					{
						Graphics.DrawMesh(cChunk.mf.sharedMesh, cChunk.go.transform.position, Quaternion.identity, Atlas.mat, 0, Controll.csCam, 0, block);
					}
				}
			}
		}
	}
}
