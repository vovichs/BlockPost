using UnityEngine;

public class LineDebug : MonoBehaviour
{
	public static void Draw(Vector3 start, Vector3 end, Color color, float duration = 10f)
	{
		GameObject obj = new GameObject();
		obj.transform.position = start;
		obj.AddComponent<LineRenderer>();
		LineRenderer component = obj.GetComponent<LineRenderer>();
		component.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		component.SetColors(color, color);
		component.SetWidth(0.1f, 0.1f);
		component.SetPosition(0, start);
		component.SetPosition(1, end);
		Object.Destroy(obj, duration);
	}
}
