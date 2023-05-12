global using System;
global using System.Collections.Generic;
global using System.Linq;

global using BepInEx;
global using BepInEx.Configuration;
global using BepInEx.Logging;

global using HarmonyLib;

global using ModClass;

global using ModCommon;

global using UnityEngine;
global using UnityEngine.UI;

namespace ModCommon
{
	public static class ModCommon
	{
		public static ManualLogSource logger = null;
		public static PlanetFactory factory = null;

		public static void Log(string str)
		{
			if(logger==null)
			{
				logger=new ManualLogSource("ZHYB.DSP.MOD.ModCommon");
			}
			logger.LogMessage(str);
		}
	}
}

namespace ModClass
{
}

namespace Patch
{
}