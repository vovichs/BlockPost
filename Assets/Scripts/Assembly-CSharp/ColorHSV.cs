using UnityEngine;

internal class ColorHSV
{
	private float h;

	private float s;

	private float v;

	private float a;

	public ColorHSV(float h, float s, float v)
	{
		this.h = h;
		this.s = s;
		this.v = v;
		a = 1f;
	}

	public ColorHSV(float h, float s, float v, float a)
	{
		this.h = h;
		this.s = s;
		this.v = v;
		this.a = a;
	}

	public ColorHSV(Color color)
	{
		float num = Mathf.Min(Mathf.Min(color.r, color.g), color.b);
		float num2 = Mathf.Max(Mathf.Max(color.r, color.g), color.b);
		float num3 = num2 - num;
		v = num2;
		if (!Mathf.Approximately(num2, 0f))
		{
			s = num3 / num2;
			if (Mathf.Approximately(num, num2))
			{
				v = num2;
				s = 0f;
				h = -1f;
				return;
			}
			if (color.r == num2)
			{
				h = (color.g - color.b) / num3;
			}
			else if (color.g == num2)
			{
				h = 2f + (color.b - color.r) / num3;
			}
			else
			{
				h = 4f + (color.r - color.g) / num3;
			}
			h *= 60f;
			if (h < 0f)
			{
				h += 360f;
			}
		}
		else
		{
			s = 0f;
			h = -1f;
		}
	}

	public Color ToColor()
	{
		if (s == 0f)
		{
			return new Color(v, v, v, a);
		}
		float num = h / 60f;
		int num2 = (int)Mathf.Floor(num);
		float num3 = num - (float)num2;
		float num4 = v;
		float num5 = num4 * (1f - s);
		float num6 = num4 * (1f - s * num3);
		float num7 = num4 * (1f - s * (1f - num3));
		Color result = new Color(0f, 0f, 0f, a);
		switch (num2)
		{
		case 0:
			result.r = num4;
			result.g = num7;
			result.b = num5;
			break;
		case 1:
			result.r = num6;
			result.g = num4;
			result.b = num5;
			break;
		case 2:
			result.r = num5;
			result.g = num4;
			result.b = num7;
			break;
		case 3:
			result.r = num5;
			result.g = num6;
			result.b = num4;
			break;
		case 4:
			result.r = num7;
			result.g = num5;
			result.b = num4;
			break;
		default:
			result.r = num4;
			result.g = num5;
			result.b = num6;
			break;
		}
		return result;
	}

	public static Color GetRandomColor(float h, float s, float v)
	{
		return new ColorHSV(h, s, v).ToColor();
	}
}
