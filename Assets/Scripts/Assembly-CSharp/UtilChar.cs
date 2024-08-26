using Player;
using UnityEngine;

public class UtilChar : MonoBehaviour
{
	public static CharColor[] colorskin = new CharColor[3];

	public static CharColor[] coloreye = new CharColor[3];

	public static CharColor[] colorhair = new CharColor[4];

	public static CharColor ccskin = null;

	public static CharColor cceye = null;

	public static CharColor cchair = null;

	public static void InitColors()
	{
		colorskin[0] = new CharColor(0, new Color(47f / 51f, 63f / 85f, 52f / 85f), new Color(43f / 51f, 0.6745098f, 0.5568628f));
		colorskin[1] = new CharColor(1, new Color(0.5137255f, 0.2901961f, 8f / 51f), new Color(37f / 85f, 0.2509804f, 0.14509805f));
		colorskin[2] = new CharColor(2, new Color(0.99607843f, 69f / 85f, 0.5568628f), new Color(0.8509804f, 2f / 3f, 0.41568628f));
		coloreye[0] = new CharColor(0, new Color(0.3019608f, 54f / 85f, 13f / 15f), new Color(0.1254902f, 21f / 85f, 1f / 3f));
		coloreye[1] = new CharColor(1, new Color(0.39607844f, 0.72156864f, 8f / 51f), new Color(8f / 51f, 0.2784314f, 6f / 85f));
		coloreye[2] = new CharColor(2, new Color(1f, 16f / 85f, 16f / 85f), new Color(33f / 85f, 0.07450981f, 1f / 15f));
		colorhair[0] = new CharColor(0, new Color(16f / 85f, 0.1254902f, 0f), new Color(8f / 85f, 0.0627451f, 0f));
		colorhair[1] = new CharColor(1, new Color(12f / 85f, 12f / 85f, 12f / 85f), new Color(0.0627451f, 0.0627451f, 0.0627451f));
		colorhair[2] = new CharColor(2, new Color(82f / 85f, 0.9254902f, 0.64705884f), new Color(78f / 85f, 0.827451f, 26f / 51f));
		colorhair[3] = new CharColor(3, new Color(0.9411765f, 0.9411765f, 0.9411765f), new Color(0.8784314f, 0.8784314f, 0.8784314f));
		ccskin = colorskin[0];
		cceye = coloreye[0];
		cchair = colorhair[0];
	}
}
