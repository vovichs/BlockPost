using System.Linq;
using UnityEngine;
using VK.Unity.Responses;

namespace VK.Unity.Example
{
	internal abstract class MenuBase : ConsoleBase
	{
		protected abstract string MenuName { get; }

		protected abstract void GetGui();

		protected virtual bool ShowDialogModeSelector()
		{
			return false;
		}

		protected virtual bool ShowBackButton()
		{
			return true;
		}

		protected void HandleResult(AuthResponse result)
		{
			if (result == null)
			{
				base.LastResponse = "Null Response\n";
				return;
			}
			base.LastResponseTexture = null;
			if (result.error != null)
			{
				base.Status = "Error - Check log for details";
				base.LastResponse = "Error Response:\n" + result.error.errorMessage;
			}
			else
			{
				base.Status = "Success - Check log for details";
				base.LastResponse = "Success Response:\n" + JsonUtility.ToJson(result);
			}
		}

		protected void OnGUI()
		{
			if (IsHorizontalLayout())
			{
				GUILayout.BeginHorizontal();
				GUILayout.BeginVertical();
			}
			GUILayout.Label(MenuName, base.LabelStyle);
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				Vector2 vector = base.ScrollPosition;
				vector.y += Input.GetTouch(0).deltaPosition.y;
				base.ScrollPosition = vector;
			}
			base.ScrollPosition = GUILayout.BeginScrollView(base.ScrollPosition, GUILayout.MinWidth(ConsoleBase.MainWindowFullWidth));
			GUILayout.BeginHorizontal();
			if (ShowBackButton())
			{
				AddBackButton();
			}
			if (ShowBackButton())
			{
				GUILayout.Label(GUIContent.none, GUILayout.MinWidth(ConsoleBase.MarginFix));
			}
			GUILayout.EndHorizontal();
			ShowDialogModeSelector();
			GUILayout.BeginVertical();
			GetGui();
			GUILayout.Space(10f);
			AddStatus();
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
		}

		private void AddStatus()
		{
			GUILayout.Space(5f);
			GUILayout.Box("Status: " + base.Status, base.TextStyle, GUILayout.MinWidth(ConsoleBase.MainWindowWidth), GUILayout.MaxHeight(500f));
		}

		private void AddBackButton()
		{
			GUI.enabled = Enumerable.Any(ConsoleBase.MenuStack);
			if (Button("Back"))
			{
				GoBack();
			}
			GUI.enabled = true;
		}

		private void AddLogButton()
		{
			Button("Log");
		}
	}
}
