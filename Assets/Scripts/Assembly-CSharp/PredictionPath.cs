using UnityEngine;

public class PredictionPath : MonoBehaviour
{
	public Transform Cube;

	private Transform[] Cubes;

	public Transform ShootingPoint;

	private GameObject TidyParent;

	public Rigidbody Bullet;

	private Vector3 Gravity;

	public float FrequencyMultiplier;

	public int Ammount;

	public Vector3 InitialVelocity;

	private void Start()
	{
		Gravity = Physics.gravity;
		Cubes = new Transform[Ammount];
		TidyParent = new GameObject("TidyParent");
		for (int i = 0; i < Ammount - 1; i++)
		{
			Cubes[i] = Object.Instantiate(Cube, Vector3.zero, Quaternion.identity);
			Cubes[i].parent = TidyParent.transform;
		}
	}

	private void Update()
	{
		UpdatePath();
		if (Input.GetKeyUp(KeyCode.Space))
		{
			Object.Instantiate(Bullet, ShootingPoint.position, Quaternion.identity).AddForce(InitialVelocity, ForceMode.VelocityChange);
		}
	}

	private void UpdatePath()
	{
		for (int i = 0; i < Ammount - 1; i++)
		{
			Vector3 position = PlotPath(ShootingPoint.position, InitialVelocity, (float)i * FrequencyMultiplier);
			Cubes[i].position = position;
		}
	}

	private Vector3 PlotPath(Vector3 InitialPosition, Vector3 InitialVelocity, float TimeStep)
	{
		return InitialPosition + (InitialVelocity * TimeStep + 0.5f * Gravity * (TimeStep * TimeStep));
	}
}
