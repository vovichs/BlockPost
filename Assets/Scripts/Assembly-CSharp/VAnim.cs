using UnityEngine;

public class VAnim : MonoBehaviour
{
	public static void AddAnimator(GameObject go)
	{
		go.AddComponent<Animation>();
	}
}
