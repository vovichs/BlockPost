using UnityEngine;

public class Util : MonoBehaviour
{
	public static string GetBrowserPath()
	{
		return string.Empty;
	}

	public static void CalculateFrustumPlanes(Matrix4x4 mat)
	{
		Controll.camplanes[0].normal = new Vector3(mat.m30 + mat.m00, mat.m31 + mat.m01, mat.m32 + mat.m02);
		Controll.camplanes[0].distance = mat.m33 + mat.m03;
		Controll.camplanes[1].normal = new Vector3(mat.m30 - mat.m00, mat.m31 - mat.m01, mat.m32 - mat.m02);
		Controll.camplanes[1].distance = mat.m33 - mat.m03;
		Controll.camplanes[2].normal = new Vector3(mat.m30 + mat.m10, mat.m31 + mat.m11, mat.m32 + mat.m12);
		Controll.camplanes[2].distance = mat.m33 + mat.m13;
		Controll.camplanes[3].normal = new Vector3(mat.m30 - mat.m10, mat.m31 - mat.m11, mat.m32 - mat.m12);
		Controll.camplanes[3].distance = mat.m33 - mat.m13;
		Controll.camplanes[4].normal = new Vector3(mat.m30 + mat.m20, mat.m31 + mat.m21, mat.m32 + mat.m22);
		Controll.camplanes[4].distance = mat.m33 + mat.m23;
		Controll.camplanes[5].normal = new Vector3(mat.m30 - mat.m20, mat.m31 - mat.m21, mat.m32 - mat.m22);
		Controll.camplanes[5].distance = mat.m33 - mat.m23;
		for (uint num = 0u; num < 6; num++)
		{
			float magnitude = Controll.camplanes[num].normal.magnitude;
			Controll.camplanes[num].normal /= magnitude;
			Controll.camplanes[num].distance /= magnitude;
		}
	}

	public static void DrawPlane(Vector3 position, Vector3 normal)
	{
		Vector3 vector = ((!(normal.normalized != Vector3.forward)) ? (Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude) : (Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude));
		Vector3 vector2 = position + vector;
		Vector3 vector3 = position - vector;
		vector = Quaternion.AngleAxis(90f, normal) * vector;
		Vector3 vector4 = position + vector;
		Vector3 vector5 = position - vector;
		Debug.DrawLine(vector2, vector3, Color.green);
		Debug.DrawLine(vector4, vector5, Color.green);
		Debug.DrawLine(vector2, vector4, Color.green);
		Debug.DrawLine(vector4, vector3, Color.green);
		Debug.DrawLine(vector3, vector5, Color.green);
		Debug.DrawLine(vector5, vector2, Color.green);
		Debug.DrawRay(position, normal, Color.red);
	}
}
