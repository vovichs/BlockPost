using System;
using System.Collections.Generic;
using System.Net.Sockets;

public class BaseClient
{
	public class RecvData
	{
		public byte[] Buffer;

		public int Len;

		public RecvData(byte[] _buffer, int _len)
		{
			Buffer = new byte[_len];
			for (int i = 0; i < _len; i++)
			{
				Buffer[i] = _buffer[i];
			}
			Len = _len;
		}

		~RecvData()
		{
			Buffer = null;
		}
	}

	public delegate void MethodContainer();

	public bool Lock;

	public int Rate = 30000;

	public string IP = "127.0.0.1";

	public int PORT = 8778;

	public int POLICYPORT = 843;

	public bool Active;

	private TcpClient client;

	private byte[] sendbuffer;

	private bool forcedisconnect;

	private List<RecvData> Tlist = new List<RecvData>();

	public byte[] buffer;

	public int len;

	private byte[] readBuffer = new byte[102400];

	private int SplitRead;

	private int BytesRead;

	public event MethodContainer onRecvPacket;

	public event MethodContainer onConnect;

	public event MethodContainer onDisconnect;

	public event MethodContainer onError;

	public BaseClient()
	{
		sendbuffer = new byte[2050];
		Active = false;
		forcedisconnect = false;
		lock (Tlist)
		{
			Tlist.Clear();
		}
	}

	public void Connect()
	{
		if (client != null && client.Connected)
		{
			return;
		}
		try
		{
			client = new TcpClient();
			client.NoDelay = true;
			IAsyncResult asyncResult = client.BeginConnect(IP, PORT, null, null);
			asyncResult.AsyncWaitHandle.WaitOne(2000);
			if (!client.Connected)
			{
				Active = false;
				Log.Add("BaseClient connection error (server is not active)");
			}
			else
			{
				client.EndConnect(asyncResult);
				client.GetStream().BeginRead(readBuffer, 0, Rate, DoRead, null);
				Active = true;
				Log.Add("BaseClient connected to server " + IP + " " + PORT);
			}
		}
		catch
		{
			Log.Add("BaseClient connection error (server is not active)");
			Active = false;
		}
		Lock = false;
		if (Active)
		{
			this.onConnect();
		}
	}

	public void Update()
	{
		lock (Tlist)
		{
			for (int i = 0; i < Tlist.Count; i++)
			{
				buffer = Tlist[i].Buffer;
				len = Tlist[i].Len;
				ProcessData();
			}
			Tlist.Clear();
		}
		if (forcedisconnect)
		{
			forcedisconnect = false;
		}
	}

	public void CloseClient()
	{
		if (client != null)
		{
			client.Close();
			client = null;
		}
	}

	private void DoRead(IAsyncResult ar)
	{
		try
		{
			BytesRead = client.GetStream().EndRead(ar);
			if (BytesRead < 1)
			{
				forcedisconnect = true;
				return;
			}
			SplitRead += BytesRead;
			while (SplitRead >= 4)
			{
				int num = NET.DecodeShort(readBuffer, 2);
				if (SplitRead < num)
				{
					break;
				}
				lock (Tlist)
				{
					Tlist.Add(new RecvData(readBuffer, num));
				}
				int num2 = 0;
				for (int i = num; i < SplitRead; i++)
				{
					readBuffer[num2] = readBuffer[i];
					num2++;
				}
				SplitRead -= num;
			}
			client.GetStream().BeginRead(readBuffer, SplitRead, Rate, DoRead, null);
		}
		catch (Exception)
		{
		}
	}

	public void AddMsg(byte[] buffer, int len)
	{
		lock (Tlist)
		{
			Tlist.Add(new RecvData(buffer, len));
		}
	}

	private void ProcessData()
	{
		if (len >= 2 && buffer[0] == 245)
		{
			this.onRecvPacket();
		}
	}

	public void cl_send()
	{
		if (!Active)
		{
			return;
		}
		try
		{
			client.GetStream().Write(NET.WRITE_DATA(), 0, NET.WRITE_LEN());
		}
		catch (Exception)
		{
			drop();
		}
	}

	public void drop()
	{
		Active = false;
		this.onDisconnect();
	}

	public void send_ping()
	{
		NET.BEGIN_WRITE(245, 101);
		NET.END_WRITE();
		cl_send();
	}
}
