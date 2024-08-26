using UnityEngine;

public class ColorPicker : MonoBehaviour
{
	public bool useDefinedPosition;

	public int positionLeft;

	public int positionTop;

	public Color currColor;

	private int textureWidth = 16;

	private int textureHeight = 256;

	private int detailres = 64;

	private float SliderPos;

	private Texture2D saturationTexture;

	private Texture2D styleTexture;

	public bool showPicker = true;

	private Rect rBox;

	private Rect rBoxPicker;

	private Rect rBoxDrag;

	private Rect rBoxColor;

	private Rect rSliderPos;

	private Rect rBoxDetail;

	private Rect rNextColor;

	private Rect rPrevColor;

	private Texture2D tBlack;

	private Texture2D tGray2;

	private Texture2D tWhite;

	private Texture2D tColorSlider;

	private Texture2D tColorDetail;

	private bool dragging;

	private Vector2 startPos;

	private Vector2 currentPos;

	private float offsetx;

	private float offsety;

	private int detailx = 64;

	private int detaily;

	private int size = 6;

	private int size2 = 3;

	private void OnResize()
	{
		rBox.Set((float)(positionLeft - 4) + offsetx, (float)(positionTop - 4) + offsety, 328f, 264f);
		rBoxPicker.Set(rBox.x + 8f + 256f, rBox.y + 4f, textureWidth, textureHeight);
		rBoxDrag.Set(rBox.x + rBox.width - 44f, rBox.y + rBox.height - 44f, 40f, 40f);
		rBoxColor.Set(rBox.x + rBox.width - 44f, rBox.y + 4f, 40f, 40f);
		rSliderPos.Set(0f, 0f, 0f, 0f);
		rBoxDetail.Set(rBox.x + 4f, rBox.y + 4f, 256f, 256f);
		rPrevColor.Set(rBoxColor.x, rBoxColor.y + 44f, 40f, 40f);
		rNextColor.Set(rBoxColor.x, rBoxColor.y + 88f, 40f, 40f);
	}

	private void Awake()
	{
		tBlack = TEX.GetTextureByName("black");
		tGray2 = TEX.GetTextureByName("gray2");
		tWhite = TEX.GetTextureByName("white");
		positionLeft = Screen.width / 2 - textureWidth / 2;
		positionTop = Screen.height / 2 - textureHeight / 2;
		OnResize();
		tColorSlider = Resources.Load("colorbar") as Texture2D;
	}

	private void OnGUI()
	{
		if (showPicker)
		{
			GUI.depth = -1;
			ElementBox();
			ElementSlider();
			ElementDrag();
			GUI.color = currColor;
			GUI.DrawTexture(rBoxColor, tWhite);
			GUI.color = Color.white;
			int num = (int)Input.mousePosition.x;
			int num2 = Screen.height - (int)Input.mousePosition.y;
			if (currColor.a != 1f)
			{
				GUI.color = Color.white;
			}
			else
			{
				GUI.color = currColor;
			}
			if (GUIM.Contains(rBox))
			{
				GUI.DrawTexture(new Rect(num - 4, num2 - 4 - size, size + 2, size + 2), tBlack);
			}
			GUI.DrawTexture(new Rect(num - 3, num2 - 3 - size, size, size), tWhite);
		}
	}

	private void ElementDrag()
	{
		if (Event.current.type == EventType.MouseDrag)
		{
			currentPos = Event.current.mousePosition;
			if (!dragging && GUIM.Contains(rBoxDrag))
			{
				dragging = true;
				startPos = currentPos;
			}
		}
		if (Event.current.type == EventType.MouseUp && dragging)
		{
			dragging = false;
			positionLeft += (int)offsetx;
			positionTop += (int)offsety;
			offsetx = 0f;
			offsety = 0f;
			OnResize();
		}
		if (dragging)
		{
			offsetx = currentPos.x - startPos.x;
			offsety = currentPos.y - startPos.y;
			OnResize();
		}
	}

	private void ElementSlider()
	{
		float num = SliderPos;
		if (GUIM.HideButton(rBoxPicker))
		{
			float num2 = (float)Screen.height - Input.mousePosition.y - rBoxPicker.y;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			if (num2 > (float)textureHeight)
			{
				num2 = textureHeight;
			}
			num = (float)textureHeight - num2;
		}
		if (num != SliderPos)
		{
			SliderPos = num;
			currColor = tColorSlider.GetPixel(8, (int)SliderPos);
			rSliderPos.Set(rBoxPicker.x - 2f, rBoxPicker.y + rBoxPicker.height - SliderPos, 20f, 2f);
			if (tColorDetail == null)
			{
				tColorDetail = new Texture2D(detailres, detailres, TextureFormat.RGB24, false);
				tColorDetail.filterMode = FilterMode.Point;
			}
			Color white = Color.white;
			float num3 = 1f / (float)detailres;
			for (int i = 0; i < detailres; i++)
			{
				for (int j = 0; j < detailres; j++)
				{
					white.r = num3 * (float)i * (currColor.r + num3 * (float)(detailres - j) * (1f - currColor.r));
					white.g = num3 * (float)i * (currColor.g + num3 * (float)(detailres - j) * (1f - currColor.g));
					white.b = num3 * (float)i * (currColor.b + num3 * (float)(detailres - j) * (1f - currColor.b));
					tColorDetail.SetPixel(j, i, white);
				}
			}
			tColorDetail.Apply(false);
			currColor = tColorDetail.GetPixel(detailx, detaily);
		}
		GUI.DrawTexture(rBoxPicker, tColorSlider);
		GUI.DrawTexture(rSliderPos, tWhite);
		if (tColorDetail != null)
		{
			GUI.DrawTexture(rBoxDetail, tColorDetail);
		}
	}

	private void ElementBox()
	{
		GUIM.DrawBox(rBox, tBlack);
		GUI.DrawTexture(rBoxDrag, tGray2);
		if (GUIM.HideButton(rBoxDetail))
		{
			int num = (int)Input.mousePosition.x;
			int num2 = Screen.height - (int)Input.mousePosition.y;
			detailx = (num - (int)rBoxDetail.x) * detailres / 256;
			detaily = -(num2 - (int)rBoxDetail.y) * detailres / 256;
			if (detaily == 0)
			{
				detaily = -1;
			}
			if (tColorDetail != null)
			{
				currColor = tColorDetail.GetPixel(detailx, detaily);
			}
		}
	}

	public void SetCursorSize(int val)
	{
		size = val;
		size2 = size / 2;
	}
}
