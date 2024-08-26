using UnityEngine;

public class FadeLight : MonoBehaviour
{
	public Light lightToDim;

	public float maxTime = 30f;

	private float mEndTime;

	private float mStartTime;

	private void Awake()
	{
		mStartTime = Time.time;
		mEndTime = mStartTime + maxTime;
	}

	private void Update()
	{
		if ((bool)lightToDim)
		{
			lightToDim.intensity = Mathf.InverseLerp(mEndTime, mStartTime, Time.time) * 4f;
		}
	}
}
