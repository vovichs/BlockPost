using UnityEngine;

public class VUtil : MonoBehaviour
{
	public static bool headcontact = false;

	public static bool groundcontact = false;

	public static bool bodycontact = false;

	public static Vector3[] p = new Vector3[16];

	public static Vector3 Cast(Vector3 pos, Vector3 dir)
	{
		Vector3 pos2 = pos + dir;
		int num = 0;
		while (true)
		{
			num++;
			if (num > 64)
			{
				MonoBehaviour.print("breaked at " + pos2.x + " " + pos2.y + " " + pos2.z + " | " + Map.GetBlock(pos2));
				break;
			}
			int block = Map.GetBlock(pos2);
			if (block < 0)
			{
				return pos;
			}
			if (block != 0)
			{
				break;
			}
			pos2 += dir;
		}
		return pos2 -= dir;
	}

	public static bool isValidBBox(Vector3 pos, float rad, float height)
	{
		headcontact = false;
		groundcontact = false;
		bodycontact = false;
		p[0] = pos + Vector3.right * rad + Vector3.forward * rad;
		p[1] = pos + Vector3.right * (0f - rad) + Vector3.forward * rad;
		p[2] = pos + Vector3.right * (0f - rad) + Vector3.forward * (0f - rad);
		p[3] = pos + Vector3.right * rad + Vector3.forward * (0f - rad);
		p[4] = pos + Vector3.right * rad + Vector3.forward * rad + new Vector3(0f, height, 0f);
		p[5] = pos + Vector3.right * (0f - rad) + Vector3.forward * rad + new Vector3(0f, height, 0f);
		p[6] = pos + Vector3.right * (0f - rad) + Vector3.forward * (0f - rad) + new Vector3(0f, height, 0f);
		p[7] = pos + Vector3.right * rad + Vector3.forward * (0f - rad) + new Vector3(0f, height, 0f);
		p[8] = pos + Vector3.right * rad + Vector3.forward * rad + new Vector3(0f, 1f, 0f);
		p[9] = pos + Vector3.right * (0f - rad) + Vector3.forward * rad + new Vector3(0f, 1f, 0f);
		p[10] = pos + Vector3.right * (0f - rad) + Vector3.forward * (0f - rad) + new Vector3(0f, 1f, 0f);
		p[11] = pos + Vector3.right * rad + Vector3.forward * (0f - rad) + new Vector3(0f, 1f, 0f);
		if (height > 2f)
		{
			p[12] = pos + Vector3.right * rad + Vector3.forward * rad + new Vector3(0f, 2f, 0f);
			p[13] = pos + Vector3.right * (0f - rad) + Vector3.forward * rad + new Vector3(0f, 2f, 0f);
			p[14] = pos + Vector3.right * (0f - rad) + Vector3.forward * (0f - rad) + new Vector3(0f, 2f, 0f);
			p[15] = pos + Vector3.right * rad + Vector3.forward * (0f - rad) + new Vector3(0f, 2f, 0f);
		}
		else
		{
			p[12] = Vector3.zero;
		}
		for (int i = 0; i < 16; i++)
		{
			if (p[i] == Vector3.zero)
			{
				return true;
			}
			if (Map.GetBlock(p[i]) != 0)
			{
				if (i <= 3)
				{
					groundcontact = true;
				}
				else if (i <= 7)
				{
					headcontact = true;
				}
				else
				{
					bodycontact = true;
				}
				return false;
			}
		}
		return true;
	}

	public static bool isValidBlockPlace(Vector3 pos, float rad, float height, Vector3 blockpos)
	{
		p[0] = pos + Vector3.right * rad + Vector3.forward * rad;
		p[1] = pos + Vector3.right * (0f - rad) + Vector3.forward * rad;
		p[2] = pos + Vector3.right * (0f - rad) + Vector3.forward * (0f - rad);
		p[3] = pos + Vector3.right * rad + Vector3.forward * (0f - rad);
		p[4] = pos + Vector3.right * rad + Vector3.forward * rad + new Vector3(0f, height, 0f);
		p[5] = pos + Vector3.right * (0f - rad) + Vector3.forward * rad + new Vector3(0f, height, 0f);
		p[6] = pos + Vector3.right * (0f - rad) + Vector3.forward * (0f - rad) + new Vector3(0f, height, 0f);
		p[7] = pos + Vector3.right * rad + Vector3.forward * (0f - rad) + new Vector3(0f, height, 0f);
		p[8] = pos + Vector3.right * rad + Vector3.forward * rad + new Vector3(0f, 1f, 0f);
		p[9] = pos + Vector3.right * (0f - rad) + Vector3.forward * rad + new Vector3(0f, 1f, 0f);
		p[10] = pos + Vector3.right * (0f - rad) + Vector3.forward * (0f - rad) + new Vector3(0f, 1f, 0f);
		p[11] = pos + Vector3.right * rad + Vector3.forward * (0f - rad) + new Vector3(0f, 1f, 0f);
		if (height > 2f)
		{
			p[12] = pos + Vector3.right * rad + Vector3.forward * rad + new Vector3(0f, 2f, 0f);
			p[13] = pos + Vector3.right * (0f - rad) + Vector3.forward * rad + new Vector3(0f, 2f, 0f);
			p[14] = pos + Vector3.right * (0f - rad) + Vector3.forward * (0f - rad) + new Vector3(0f, 2f, 0f);
			p[15] = pos + Vector3.right * rad + Vector3.forward * (0f - rad) + new Vector3(0f, 2f, 0f);
		}
		else
		{
			p[12] = Vector3.zero;
		}
		for (int i = 0; i < 16; i++)
		{
			if (p[i] == Vector3.zero)
			{
				return true;
			}
			if ((int)p[i].x == (int)blockpos.x && (int)p[i].y == (int)blockpos.y && (int)p[i].z == (int)blockpos.z)
			{
				return false;
			}
		}
		return true;
	}

	public static bool isHeadContact()
	{
		return headcontact;
	}

	public static bool isGroundContact()
	{
		return groundcontact;
	}

	public static Vector3 CastBBox(Vector3 pos, Vector3 dir, float rad, float height)
	{
		return Vector3.zero;
	}
}
