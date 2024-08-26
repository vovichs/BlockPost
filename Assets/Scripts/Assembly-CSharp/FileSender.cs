using UnityEngine;

public class FileSender : MonoBehaviour
{
	public static FileSender cs;

	public bool inSend;

	private int filetype;

	private byte[] filedata;

	private int filesize;

	private string filename;

	private int pos;

	private byte[] buffer;

	private void Awake()
	{
		cs = this;
	}

	public void Send(int _filetype, string _filename, byte[] _filedata, int _filesize)
	{
		filetype = _filetype;
		filedata = _filedata;
		filesize = _filesize;
		filename = _filename;
		inSend = true;
		Client.cs.send_file_start(filetype, filename, filesize);
		pos = 0;
		buffer = new byte[512];
	}

	private void Update()
	{
		if (inSend)
		{
			if (pos < filesize)
			{
				SendFragment();
			}
			else
			{
				SendEnd();
			}
		}
	}

	private void SendFragment()
	{
		int num = filesize - pos;
		if (num > 500)
		{
			num = 500;
		}
		for (int i = 0; i < num; i++)
		{
			buffer[i] = filedata[pos];
			pos++;
		}
		Client.cs.send_file_continue(buffer, num);
		Debug.Log("pos " + pos + "/" + filesize);
	}

	private void SendEnd()
	{
		Client.cs.send_file_end();
		inSend = false;
		buffer = null;
		filedata = null;
	}
}
