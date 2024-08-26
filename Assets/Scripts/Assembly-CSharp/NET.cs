using System;
using System.Text;
using UnityEngine;

public class NET : MonoBehaviour
{
	private static float COORDMAX = 1024f;

	private static float COORDRES = 64f;

	private static float COORDCOEF = 1f / COORDRES;

	private static byte[] sendbuffer = new byte[8193];

	private static int writepos = 0;

	private static byte[] b = null;

	private static byte[] readbuffer;

	private static int readlen;

	private static int readpos;

	private static bool readerror;

	public static int i_coord = 0;

	private static float f_coord = 0f;

	public static int i_angle = 0;

	private static float f_angle = 0f;

	public static void BEGIN_WRITE(byte proto, byte packetid)
	{
		sendbuffer[0] = proto;
		sendbuffer[1] = packetid;
		writepos = 4;
	}

	public static void BEGIN_WRITE()
	{
		writepos = 0;
	}

	public static void WRITE_BYTE(byte bvalue)
	{
		sendbuffer[writepos] = bvalue;
		writepos++;
	}

	public static void WRITE_SHORT(short svalue)
	{
		byte[] array = EncodeShort(svalue);
		sendbuffer[writepos] = array[0];
		sendbuffer[writepos + 1] = array[1];
		writepos += 2;
	}

	public static void WRITE_FLOAT(float fvalue)
	{
		byte[] array = EncodeFloat(fvalue);
		sendbuffer[writepos] = array[0];
		sendbuffer[writepos + 1] = array[1];
		sendbuffer[writepos + 2] = array[2];
		sendbuffer[writepos + 3] = array[3];
		writepos += 4;
	}

	public static void WRITE_LONG(int ivalue)
	{
		byte[] array = EncodeInteger(ivalue);
		sendbuffer[writepos] = array[0];
		sendbuffer[writepos + 1] = array[1];
		sendbuffer[writepos + 2] = array[2];
		sendbuffer[writepos + 3] = array[3];
		writepos += 4;
	}

	public static void WRITE_ULONG64(ulong ivalue)
	{
		byte[] array = EncodeInteger64(ivalue);
		sendbuffer[writepos] = array[0];
		sendbuffer[writepos + 1] = array[1];
		sendbuffer[writepos + 2] = array[2];
		sendbuffer[writepos + 3] = array[3];
		sendbuffer[writepos + 4] = array[4];
		sendbuffer[writepos + 5] = array[5];
		sendbuffer[writepos + 6] = array[6];
		sendbuffer[writepos + 7] = array[7];
		writepos += 8;
	}

	public static void WRITE_STRING_ANSI(string svalue)
	{
		byte[] bytes = Encoding.GetEncoding(1251).GetBytes(svalue);
		for (int i = 0; i < bytes.Length; i++)
		{
			WRITE_BYTE(bytes[i]);
		}
		WRITE_BYTE(0);
	}

	public static void WRITE_STRING(string svalue)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		int byteCount = uTF8Encoding.GetByteCount(svalue);
		byte[] array = new byte[byteCount];
		Buffer.BlockCopy(uTF8Encoding.GetBytes(svalue), 0, array, 0, byteCount);
		for (int i = 0; i < byteCount; i++)
		{
			WRITE_BYTE(array[i]);
		}
		WRITE_BYTE(0);
	}

	public static int WRITE_LEN()
	{
		return writepos;
	}

	public static void END_WRITE()
	{
		short svalue = (short)writepos;
		writepos = 2;
		WRITE_SHORT(svalue);
		writepos = svalue;
	}

	public static byte[] WRITE_DATA()
	{
		return sendbuffer;
	}

	public static void _BEGIN_WRITE(byte[] buffer, int offset)
	{
		b = buffer;
		writepos = offset;
	}

	public static void _WRITE_BYTE(byte bvalue)
	{
		b[writepos] = bvalue;
		writepos++;
	}

	public static void _WRITE_SHORT(short svalue)
	{
		byte[] array = EncodeShort(svalue);
		b[writepos] = array[0];
		b[writepos + 1] = array[1];
		writepos += 2;
	}

	public static void _WRITE_FLOAT(float fvalue)
	{
		byte[] array = EncodeFloat(fvalue);
		b[writepos] = array[0];
		b[writepos + 1] = array[1];
		b[writepos + 2] = array[2];
		b[writepos + 3] = array[3];
		writepos += 4;
	}

	public static void _WRITE_LONG(int ivalue)
	{
		byte[] array = EncodeInteger(ivalue);
		b[writepos] = array[0];
		b[writepos + 1] = array[1];
		b[writepos + 2] = array[2];
		b[writepos + 3] = array[3];
		writepos += 4;
	}

	public static void _WRITE_ULONG64(ulong ivalue)
	{
		byte[] array = EncodeInteger64(ivalue);
		b[writepos] = array[0];
		b[writepos + 1] = array[1];
		b[writepos + 2] = array[2];
		b[writepos + 3] = array[3];
		b[writepos + 4] = array[4];
		b[writepos + 5] = array[5];
		b[writepos + 6] = array[6];
		b[writepos + 7] = array[7];
		writepos += 8;
	}

	public static void _END_WRITE(int ofs = 2)
	{
		short svalue = (short)writepos;
		writepos = ofs;
		_WRITE_SHORT(svalue);
		writepos = svalue;
	}

	public static int _WRITE_LEN()
	{
		return writepos;
	}

	public static byte[] EncodeShort(short inShort)
	{
		return BitConverter.GetBytes(inShort);
	}

	public static byte[] EncodeInteger(int inInt)
	{
		return BitConverter.GetBytes(inInt);
	}

	public static byte[] EncodeInteger64(ulong in64)
	{
		return BitConverter.GetBytes(in64);
	}

	public static byte[] EncodeFloat(float inFloat)
	{
		return BitConverter.GetBytes(inFloat);
	}

	public static void BEGIN_READ(byte[] inBytes, int len, int startpos)
	{
		readbuffer = inBytes;
		readlen = len;
		readpos = startpos;
		readerror = false;
	}

	public static int READ_BYTE()
	{
		if (readpos + 1 > readlen)
		{
			readerror = true;
			return 0;
		}
		byte result = readbuffer[readpos];
		readpos++;
		return result;
	}

	public static int READ_SHORT()
	{
		if (readpos + 2 > readlen)
		{
			readerror = true;
			return 0;
		}
		int result = DecodeShort(readbuffer, readpos);
		readpos += 2;
		return result;
	}

	public static int READ_LONG()
	{
		if (readpos + 4 > readlen)
		{
			readerror = true;
			return 0;
		}
		int result = DecodeInteger(readbuffer, readpos);
		readpos += 4;
		return result;
	}

	public static uint READ_ULONG()
	{
		if (readpos + 4 > readlen)
		{
			readerror = true;
			return 0u;
		}
		uint result = DecodeUInteger(readbuffer, readpos);
		readpos += 4;
		return result;
	}

	public static ulong READ_ULONG64()
	{
		if (readpos + 8 > readlen)
		{
			readerror = true;
			return 0uL;
		}
		ulong result = DecodeInteger64(readbuffer, readpos);
		readpos += 8;
		return result;
	}

	public static float READ_FLOAT()
	{
		if (readpos + 4 > readlen)
		{
			readerror = true;
			return 0f;
		}
		float result = DecodeSingle(readbuffer, readpos);
		readpos += 4;
		return result;
	}

	public static string READ_STRING()
	{
		int num = 0;
		int index = readpos;
		while (readpos < readlen && readbuffer[readpos] != 0)
		{
			num++;
			readpos++;
		}
		readpos++;
		if (num == 0)
		{
			return "";
		}
		return Encoding.UTF8.GetString(readbuffer, index, num);
	}

	public static float READ_COORD()
	{
		if (readpos + 2 > readlen)
		{
			readerror = true;
			return 0f;
		}
		i_coord = DecodeShort(readbuffer, readpos);
		f_coord = (float)i_coord * COORDCOEF;
		readpos += 2;
		return f_coord;
	}

	public static bool READ_ERROR()
	{
		return readerror;
	}

	public static int READ_POS()
	{
		return readpos;
	}

	public static float READ_ANGLE()
	{
		if (readpos + 1 > readlen)
		{
			readerror = true;
			return 0f;
		}
		i_angle = readbuffer[readpos];
		f_angle = (float)(int)readbuffer[readpos] * 360f / 256f;
		readpos++;
		return f_angle;
	}

	public static void READ_POS_SET(int val)
	{
		readpos = val;
	}

	public static int DecodeShort(byte[] inBytes, int pos)
	{
		return BitConverter.ToUInt16(inBytes, pos);
	}

	public static int DecodeInteger(byte[] inBytes, int pos)
	{
		return BitConverter.ToInt32(inBytes, pos);
	}

	public static uint DecodeUInteger(byte[] inBytes, int pos)
	{
		return BitConverter.ToUInt32(inBytes, pos);
	}

	public static ulong DecodeInteger64(byte[] inBytes, int pos)
	{
		return BitConverter.ToUInt64(inBytes, pos);
	}

	public static float DecodeSingle(byte[] inBytes, int pos)
	{
		return BitConverter.ToSingle(inBytes, pos);
	}
}
