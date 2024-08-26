using UnityEngine;

public class OutlineSystem : MonoBehaviour
{
	[Header("Outline Settings")]
	[Tooltip("Should the outline be solid or fade out")]
	public bool solidOutline;

	[Tooltip("Strength override multiplier")]
	[Range(0f, 10f)]
	public float outlineStrength = 1f;

	[Tooltip("Which layers should this outline system display on")]
	public LayerMask outlineLayer;

	[Tooltip("What color should the outline be")]
	public Color outlineColor;

	[Tooltip("How many times should the render be downsampled")]
	[Range(0f, 4f)]
	public int downsampleAmount = 2;

	[Tooltip("How big should the outline be")]
	[Range(0f, 10f)]
	public float outlineSize = 1.5f;

	[Tooltip("How many times should the blur be performed")]
	[Range(1f, 10f)]
	public int outlineIterations = 2;

	[Tooltip("Upscaling of outline texture")]
	[Range(0.1f, 5f)]
	public float outlineUpscale = 1f;

	public Camera mainCamera;

	[Space(10f)]
	[Header("Component References - Do not change")]
	private RenderTexture renTexInput;

	private RenderTexture renTexRecolor;

	private RenderTexture renTexDownsample;

	private RenderTexture renTexBlur;

	private RenderTexture renTexOut;

	public Material blurMaterial;

	public Material outlineMaterial;

	private Vector2 prevSize;

	private void Awake()
	{
		if (mainCamera == null)
		{
			mainCamera = Camera.main;
		}
		UpdateRenderTextureSizes();
	}

	private void UpdateRenderTextureSizes()
	{
		Vector2 vector = ScreenDimension();
		int width = Mathf.FloorToInt((float)Mathf.FloorToInt(vector.x) * outlineUpscale);
		int height = Mathf.FloorToInt((float)Mathf.FloorToInt(vector.y) * outlineUpscale);
		renTexInput = new RenderTexture(width, height, 1);
		renTexDownsample = new RenderTexture(width, height, 1);
		renTexRecolor = new RenderTexture(width, height, 1);
		renTexOut = new RenderTexture(width, height, 1);
		renTexBlur = new RenderTexture(width, height, 1);
	}

	public Vector2 ScreenDimension()
	{
		Vector2 one = Vector2.one;
		return new Vector2(Screen.width, Screen.height);
	}

	private void RunCalcs()
	{
		outlineMaterial.SetColor("_OutlineCol", outlineColor);
		outlineMaterial.SetFloat("_GradientStrengthModifier", outlineStrength);
		RenderTexture targetTexture = mainCamera.targetTexture;
		int cullingMask = mainCamera.cullingMask;
		CameraClearFlags clearFlags = mainCamera.clearFlags;
		Color backgroundColor = mainCamera.backgroundColor;
		mainCamera.cullingMask = outlineLayer.value;
		mainCamera.targetTexture = renTexInput;
		mainCamera.clearFlags = CameraClearFlags.Color;
		mainCamera.backgroundColor = new Color(1f, 0f, 1f, 1f);
		mainCamera.Render();
		mainCamera.backgroundColor = backgroundColor;
		mainCamera.clearFlags = clearFlags;
		mainCamera.targetTexture = targetTexture;
		mainCamera.cullingMask = cullingMask;
		float num = 1f / (1f * (float)(1 << downsampleAmount));
		blurMaterial.SetVector("_Parameter", new Vector4(outlineSize * num, (0f - outlineSize) * num, 0f, 0f));
		Graphics.Blit(renTexInput, renTexRecolor, outlineMaterial, 0);
		Graphics.Blit(renTexRecolor, renTexDownsample, blurMaterial, 0);
		for (int i = 0; i < outlineIterations; i++)
		{
			float num2 = (float)i * 1f;
			blurMaterial.SetVector("_Parameter", new Vector4(outlineSize * num + num2, (0f - outlineSize) * num - num2, 0f, 0f));
			Graphics.Blit(renTexDownsample, renTexBlur, blurMaterial, 1);
			Graphics.Blit(renTexBlur, renTexDownsample, blurMaterial, 2);
		}
		outlineMaterial.SetFloat("_Solid", solidOutline ? 1f : 0f);
		outlineMaterial.SetTexture("_BlurTex", renTexDownsample);
		Graphics.Blit(renTexRecolor, renTexOut, outlineMaterial, 1);
	}

	private void LateUpdate()
	{
		Vector2 vector = new Vector2(Screen.width, Screen.height);
		if (prevSize != vector)
		{
			UpdateRenderTextureSizes();
		}
		prevSize = vector;
		RunCalcs();
	}

	private void OnGUI()
	{
		GL.PushMatrix();
		GL.LoadPixelMatrix(0f, Screen.width, Screen.height, 0f);
		Graphics.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), renTexOut);
		GL.PopMatrix();
	}
}
