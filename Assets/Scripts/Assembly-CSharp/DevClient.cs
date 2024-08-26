using UnityEngine;

public class DevClient : MonoBehaviour
{
	private BaseClient cl;

	private float sendtimer;

	private void Awake()
	{
		cl = new BaseClient();
		cl.IP = "127.0.0.1";
		cl.PORT = 50000;
		cl.onRecvPacket += OnRecvPacket;
		cl.onConnect += OnConnect;
		cl.onDisconnect += OnDisconnect;
		cl.onError += OnError;
	}

	public void Connect()
	{
		cl.Connect();
		if (!cl.Active)
		{
			GUIPlay.DevCenterLog("DevCenter not connected");
		}
		else
		{
			GUIPlay.DevCenterLog("DevCenter connected OK");
		}
	}

	public void Disconnect()
	{
	}

	private void Update()
	{
		if (cl != null)
		{
			cl.Update();
			UpdatePing();
		}
	}

	private void UpdatePing()
	{
		float time = Time.time;
		if (sendtimer == 0f || time > sendtimer)
		{
			sendtimer = time + 5f;
			send_ping();
		}
	}

	private void OnDisable()
	{
	}

	private void OnApplicationQuit()
	{
		cl.CloseClient();
	}

	public void send_ping()
	{
		NET.BEGIN_WRITE(245, 101);
		NET.END_WRITE();
		cl.cl_send();
	}

	private void OnRecvPacket()
	{
		byte b = cl.buffer[1];
		if (b != 0 && b == 101)
		{
			recv_ping();
		}
	}

	private void OnError()
	{
	}

	private void OnConnect()
	{
	}

	private void OnDisconnect()
	{
	}

	private void recv_ping()
	{
		NET.BEGIN_READ(cl.buffer, cl.len, 4);
		NET.READ_BYTE();
	}
}
