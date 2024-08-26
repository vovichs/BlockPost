using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Console : MonoBehaviour
{
	public static Console cs;

	private static bool show = false;

	private Texture2D tblack;

	private Rect rblack;

	private Rect rButton;

	private string command = "";

	private static List<string> loglist = new List<string>();

	private static List<string> cmdlist = new List<string>();

	private static int cmdoffset = -1;

	public static bool showfps = false;

	public static string version = "BLOCKPOST CONSOLE";

	private static GameObject dl = null;

	private float updateInterval = 0.5f;

	private float accum;

	private int frames;

	private float timeleft;

	private string fps_text = "";

	private BaseColor fps_color = BaseColor.Green;

	private string screenfolder = "Screenshots";

	private Rect r0;

	private Rect r1;

	private Rect r2;

	private Rect r3;

	private float maxmag;

	public void LoadEnd()
	{
		cs = this;
		tblack = TEX.GetTextureByName("black");
		OnResize();
		version = "BLOCKPOST v" + Application.version;
		if (MainManager.steam)
		{
			version += " (STEAM)";
		}
		if (PlayerPrefs.GetInt("bp_console_showfps", 0) > 0)
		{
			showfps = true;
		}
		int @int = PlayerPrefs.GetInt("bp_console_fps_max", 0);
		if (@int >= 30)
		{
			Application.targetFrameRate = @int;
		}
	}

	public void SetActive(bool val)
	{
		show = val;
	}

	public void ToggleActive()
	{
		show = !show;
	}

	public static bool isActive()
	{
		return show;
	}

	public void OnResize()
	{
		rblack = new Rect(0f, 0f, Screen.width, (float)Screen.height / 2f);
		rButton = new Rect((float)Screen.width - GUIM.YRES(208f), rblack.height - GUIM.YRES(48f), GUIM.YRES(200f), GUIM.YRES(40f));
		r0.Set((float)Screen.width - GUIM.YRES(80f), GUIM.YRES(148f), GUIM.YRES(50f), GUIM.YRES(14f));
		r1.Set((float)Screen.width - GUIM.YRES(80f), GUIM.YRES(168f), GUIM.YRES(50f), GUIM.YRES(14f));
	}

	private void OnGUI()
	{
		if (showfps)
		{
			DrawFPS();
		}
		if (!show)
		{
			return;
		}
		GUI.depth = -5;
		GUI.color = new Color(1f, 1f, 1f, 0.75f);
		if ((bool)tblack)
		{
			GUI.DrawTexture(rblack, tblack);
		}
		GUI.color = Color.white;
		float num = (float)Screen.height / 2f - 46f;
		for (int num2 = loglist.Count - 1; num2 >= 0; num2--)
		{
			GUIM.DrawText(new Rect(4f, num, Screen.width - 8, 24f), loglist[num2], TextAnchor.MiddleLeft, BaseColor.White, 1, 16, false);
			num -= 22f;
		}
		GUIM.DrawText(new Rect(0f, 0f, Screen.width - 4, 24f), version, TextAnchor.MiddleRight, BaseColor.White, 0, 20, true);
		GUIM.DrawEdit(new Rect(4f, (float)Screen.height / 2f - 28f, Screen.width - 8, 24f), ref command, TextAnchor.MiddleLeft, BaseColor.White, 0, 16, true);
		char character = Event.current.character;
		if ((character < 'a' || character > 'z') && (character < 'A' || character > 'Z') && (character < '0' || character > '9') && character != ' ' && character != '_' && character != '.' && character != '-')
		{
			Event.current.character = '\0';
		}
		if (Event.current.isKey)
		{
			switch (Event.current.keyCode)
			{
			case KeyCode.F10:
				command = "";
				SetActive(false);
				Event.current.Use();
				break;
			case KeyCode.Return:
			case KeyCode.KeypadEnter:
				ParsingCommand(command);
				command = "";
				Event.current.Use();
				break;
			case KeyCode.UpArrow:
				if (cmdlist.Count > 0)
				{
					cmdoffset++;
					if (cmdoffset > cmdlist.Count - 1)
					{
						cmdoffset = cmdlist.Count - 1;
					}
					command = cmdlist[cmdlist.Count - cmdoffset - 1];
					TextEditor obj = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
					obj.selectIndex = command.Length + 1;
					obj.cursorIndex = command.Length + 1;
				}
				break;
			}
		}
		if (GUIM.Button(rButton, BaseColor.Black, ">", TextAnchor.MiddleCenter, BaseColor.Gray, 0, 32, false))
		{
			ParsingCommand(command);
			command = "";
		}
	}

	public void Command(string cmd)
	{
		ParsingCommand(cmd);
	}

	public static void Log(string msg)
	{
		loglist.Add(msg);
	}

	private void ParsingCommand(string cmd)
	{
		loglist.Add(cmd);
		if (loglist.Count > 100)
		{
			loglist.RemoveAt(0);
		}
		cmdlist.Add(cmd);
		if (cmdlist.Count > 30)
		{
			cmdlist.RemoveAt(0);
		}
		cmdoffset = -1;
		string[] array = cmd.Split(' ');
		if (array[0] == "showfps")
		{
			if (array.Length != 2)
			{
				return;
			}
			int result = 0;
			if (int.TryParse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				if (result == 1)
				{
					showfps = true;
				}
				else
				{
					showfps = false;
				}
				PlayerPrefs.SetInt("bp_console_showfps", showfps ? 1 : 0);
			}
		}
		else if (array[0] == "clearprefs")
		{
			PlayerPrefs.DeleteAll();
			Log("PlayerPrefs cleared");
		}
		else if (array[0] == "jscall")
		{
			if (array.Length == 2)
			{
				Application.ExternalCall(array[1]);
			}
		}
		else if (array[0] == "lefthand")
		{
			if (array.Length != 2)
			{
				return;
			}
			int result2 = 0;
			if (int.TryParse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result2))
			{
				if (result2 == 1)
				{
					Controll.trCamera.localScale = new Vector3(-1f, 1f, 1f);
				}
				else
				{
					Controll.trCamera.localScale = Vector3.one;
				}
			}
		}
		else if (array[0] == "sun")
		{
			if (array.Length != 3)
			{
				return;
			}
			float result3 = 0f;
			if (float.TryParse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result3))
			{
				float result4 = 0f;
				if (float.TryParse(array[2], NumberStyles.Any, CultureInfo.InvariantCulture, out result4))
				{
					GameObject.Find("Dlight").transform.eulerAngles = new Vector3(result3, result4, 0f);
					Map.sunpos = new Vector3(result3, result4, 0f);
				}
			}
		}
		else
		{
			if (array[0] == "skycolora" || array[0] == "skycolorb")
			{
				return;
			}
			if (array[0] == "dc")
			{
				Client.IP = "127.0.0.1";
				Client.PORT = 50000;
				Client.cs.Connect();
			}
			else
			{
				if (array[0] == "dc2")
				{
					return;
				}
				if (array[0] == "dc3")
				{
					Client.IP = "underdogs.ru";
					Client.PORT = 50500;
					Client.cs.Connect();
				}
				else if (array[0] == "clearent")
				{
					for (int i = 0; i < Map.mapent.Count; i++)
					{
						if (!(Map.mapent[i].go == null))
						{
							Object.Destroy(Map.mapent[i].go);
						}
					}
					Map.mapent.Clear();
					Map.entindex = 0;
					loglist.Add("Map Ents cleared!");
				}
				else if (array[0] == "fps_max")
				{
					if (array.Length != 2)
					{
						return;
					}
					int result5 = 0;
					if (int.TryParse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result5))
					{
						if (result5 < 30)
						{
							result5 = 30;
						}
						Application.targetFrameRate = result5;
					}
				}
				else if (array[0] == "sens")
				{
					if (array.Length == 2)
					{
						float result6 = 0f;
						if (float.TryParse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result6))
						{
							Controll.sens = result6;
							PlayerPrefs.SetFloat("sens", Controll.sens);
						}
					}
				}
				else if (array[0] == "zoomsens")
				{
					if (array.Length == 2)
					{
						float result7 = 0f;
						if (float.TryParse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result7))
						{
							Controll.zoomsens = result7;
							PlayerPrefs.SetFloat("zoomsens", Controll.zoomsens);
						}
					}
				}
				else if (array[0] == "invertmouse")
				{
					if (array.Length != 2)
					{
						return;
					}
					int result8 = 0;
					if (int.TryParse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result8))
					{
						if (result8 > 0)
						{
							Controll.invertmouse = true;
						}
						else
						{
							Controll.invertmouse = false;
						}
						PlayerPrefs.SetInt("invertmouse", result8);
					}
				}
				else if (array[0] == "quit")
				{
					Application.Quit();
				}
				else if (array[0] == "unlockeditor")
				{
					GUIPlay.editor = 1;
				}
				else if (array[0] == "unlockclan")
				{
					Main.unlockclan = true;
				}
				else if (array[0] == "hud")
				{
					if (array.Length != 2)
					{
						return;
					}
					int result9 = 0;
					if (!int.TryParse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result9))
					{
						return;
					}
					if (result9 == 0)
					{
						if (Builder.active)
						{
							Palette.SetActive(false);
							HUDBuild.SetActive(false);
						}
						else
						{
							HUD.SetActive(false);
						}
						if (Controll.csRadarCam != null)
						{
							Controll.csRadarCam.enabled = false;
						}
						Radar.SetActive(false);
						Crosshair.SetActive(false);
						return;
					}
					if (Builder.active)
					{
						Palette.SetActive(true);
						HUDBuild.SetActive(true);
					}
					else
					{
						HUD.SetActive(true);
					}
					if (Controll.csRadarCam != null)
					{
						Controll.csRadarCam.enabled = true;
					}
					Radar.SetActive(true);
					Radar.GenerateRadar();
					Crosshair.SetActive(true);
				}
				else if (array[0] == "hideweapon")
				{
					if (Controll.pl.currweapon != null)
					{
						Controll.pl.currweapon.go.SetActive(false);
					}
				}
				else if (array[0] == "order")
				{
					if (array.Length == 2)
					{
						int result10 = 0;
						if (int.TryParse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result10) && result10 >= 0 && result10 < 6)
						{
							Application.ExternalCall("order", result10);
							MainManager.inorder = true;
						}
					}
				}
				else if (array[0] == "disablesun")
				{
					GameObject.Find("Dlight").SetActive(false);
				}
				else if (array[0] == "rcon")
				{
					if (array.Length == 2)
					{
						MasterClient.cs.send_rcon(array[1]);
					}
					else if (array.Length == 3)
					{
						MasterClient.cs.send_rcon(array[1] + " " + array[2]);
					}
				}
				else if (array[0] == "mod")
				{
					if (array.Length == 2)
					{
						MasterClient.cs.send_moderator(array[1]);
					}
					else if (array.Length == 3)
					{
						MasterClient.cs.send_moderator(array[1] + " " + array[2]);
					}
				}
				else if (array[0] == "cmd")
				{
					if (array.Length == 2)
					{
						Client.cs.send_cmd(array[1]);
					}
				}
				else if (array[0] == "debugnet")
				{
					base.gameObject.AddComponent<dbgNet>();
				}
				else if (array[0] == "ztest")
				{
					if (Controll.csCam != null)
					{
						if (Controll.csCam.nearClipPlane <= 0.011f)
						{
							Controll.csCam.nearClipPlane = 0.02f;
							Log("ztest enabled");
						}
						else
						{
							Controll.csCam.nearClipPlane = 0.01f;
							Log("ztest disabled");
						}
					}
				}
				else if (array[0] == "shader")
				{
					Shader shader = Shader.Find("Standard (Vertex Color)");
					Log("hash: " + shader.GetHashCode());
					Log("renderqueue: " + shader.renderQueue);
				}
				else if (array[0] == "crosshair_type")
				{
					if (array.Length == 2)
					{
						int result11 = 0;
						if (int.TryParse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result11))
						{
							Crosshair.SetCrosshair(result11);
							PlayerPrefs.SetInt("crosshair_type", result11);
						}
					}
				}
				else if (array[0] == "crosshair_size" && array.Length == 2)
				{
					int result12 = 0;
					if (int.TryParse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture, out result12))
					{
						Crosshair.SetCrosshairSize(result12);
						PlayerPrefs.SetInt("crosshair_size", result12);
					}
				}
			}
		}
	}

	private void Update()
	{
		Input.GetKeyUp(KeyCode.F6);
		if (Input.GetKeyUp(KeyCode.F10))
		{
			ToggleActive();
		}
		else
		{
			if (!showfps)
			{
				return;
			}
			timeleft -= Time.deltaTime;
			accum += 1f / Time.deltaTime;
			frames++;
			if ((double)timeleft <= 0.0)
			{
				float num = accum / (float)frames;
				fps_text = string.Format("FPS {0:0}", num);
				if (num < 25f)
				{
					fps_color = BaseColor.Red;
				}
				else if (num < 50f)
				{
					fps_color = BaseColor.Yellow;
				}
				else
				{
					fps_color = BaseColor.Green;
				}
				timeleft = updateInterval;
				accum = 0f;
				frames = 0;
			}
		}
	}

	private void DrawFPS()
	{
		GUI.depth = -5;
		GUIM.DrawText(r0, fps_text, TextAnchor.MiddleLeft, fps_color, 0, 20, true);
		GUIM.DrawText(r1, "PING " + Controll.ping, TextAnchor.MiddleLeft, BaseColor.White, 0, 20, true);
	}

	private void OnEnable()
	{
		Application.logMessageReceived += HandleLog;
	}

	private void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}

	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		if (type == LogType.Error || type == LogType.Exception || type == LogType.Warning)
		{
			loglist.Add(logString + " > " + stackTrace);
		}
	}
}
