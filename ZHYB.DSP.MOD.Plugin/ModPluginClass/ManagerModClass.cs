using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Patch;

namespace ModClass
{
	public static class ManagerModClass

	{
		private static KeyboardShortcut shortKey_Toggle_forceAccMode = new(KeyCode.L,KeyCode.LeftControl,KeyCode.LeftShift,KeyCode.LeftAlt);
		private static KeyboardShortcut shortKey_VeinControl = new(KeyCode.V,KeyCode.LeftControl,KeyCode.LeftShift,KeyCode.LeftAlt);
		private static KeyboardShortcut shortcut_TestUIDysonEditor = new KeyboardShortcut(KeyCode.N,KeyCode.LeftControl);
		private static KeyboardShortcut shortcut_AutoBuild = new KeyboardShortcut(KeyCode.B,KeyCode.LeftControl);

		public static void KeyDown()
		{
			if(shortKey_Toggle_forceAccMode.IsDown())
			{
				ToggleforceAccMode.Toggle_forceAccMode();
			}
			else if(shortKey_VeinControl.IsDown())
			{
				VeinControl.CheatMode=ModConfig.CheatMode.Value;
				VeinControl.ControlVein();
			}
			else if(shortcut_TestUIDysonEditor.IsDown())
			{
				DysonSphereGen.DysonGenNodesFrameShell();
			}
			else if(shortcut_AutoBuild.IsDown())
			{
				Patch_PlayerController.EnableAutoBuild=!Patch_PlayerController.EnableAutoBuild;
				UIRealtimeTip.Popup(Patch_PlayerController.EnableAutoBuild ? "开启自动建造" : "关闭自动建造");
			}
		}
	}
}