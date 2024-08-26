using UnityEngine;

public class Movement : MonoBehaviour
{
	private static float ground_accelerate = 8f;

	private static float max_velocity_ground = 8f;

	private static float air_accelerate = 1f;

	private static float max_velocity_air = 1f;

	private static float friction = 8f;

	public static Vector3 Accelerate(Vector3 accelDir, Vector3 prevVelocity, float accelerate, float max_velocity)
	{
		float num = Vector3.Dot(prevVelocity, accelDir);
		float num2 = accelerate * Controll.fdeltatime;
		if (num + num2 > max_velocity)
		{
			num2 = max_velocity - num;
		}
		return prevVelocity + accelDir * num2;
	}

	public static Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity)
	{
		float magnitude = prevVelocity.magnitude;
		if (magnitude != 0f)
		{
			float num = magnitude * friction * Controll.fdeltatime;
			prevVelocity *= Mathf.Max(magnitude - num, 0f) / magnitude;
		}
		return Accelerate(accelDir, prevVelocity, ground_accelerate, max_velocity_ground);
	}

	public static Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity)
	{
		return Accelerate(accelDir, prevVelocity, air_accelerate, max_velocity_air);
	}
}
