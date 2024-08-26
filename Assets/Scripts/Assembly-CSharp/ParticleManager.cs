using UnityEngine;
using UnityEngine.UI;

public class ParticleManager : MonoBehaviour
{
	public int pLength;

	public int pCurrent;

	public Text pText;

	public int pTest;

	public GameObject[] particles;

	public bool disableObject;

	public GameObject goToDisable;

	private void Start()
	{
		pLength = base.transform.childCount;
		particles = new GameObject[pLength];
		pTest = 0;
		foreach (Transform item in base.gameObject.transform)
		{
			particles[pTest] = item.gameObject;
			pTest++;
		}
		pLength = particles.Length;
		pCurrent = 0;
		particles[pCurrent].SetActive(true);
		pText.text = particles[pCurrent].name;
		if (disableObject)
		{
			goToDisable.SetActive(false);
		}
	}

	public void GoForward()
	{
		if (pCurrent + 1 < pLength)
		{
			particles[pCurrent].SetActive(false);
			particles[pCurrent + 1].SetActive(true);
			pCurrent++;
		}
		else
		{
			particles[pCurrent].SetActive(false);
			pCurrent = 0;
			particles[pCurrent].SetActive(true);
		}
		pText.text = particles[pCurrent].name;
	}

	public void GoBackward()
	{
		if (pCurrent > 0)
		{
			particles[pCurrent].SetActive(false);
			particles[pCurrent - 1].SetActive(true);
			pCurrent--;
		}
		else
		{
			particles[pCurrent].SetActive(false);
			pCurrent = pLength - 1;
			particles[pCurrent].SetActive(true);
		}
		pText.text = particles[pCurrent].name;
	}
}
