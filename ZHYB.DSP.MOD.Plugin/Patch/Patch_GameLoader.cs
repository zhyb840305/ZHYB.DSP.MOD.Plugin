using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patch
{
	[HarmonyPatch(typeof(GameLoader))]
	internal class Patch_GameLoader
	{
		[HarmonyPrefix, HarmonyPatch("FixedUpdate")]
		public static bool Prefix_FixedUpdate()
		{
			DSPGame.SkipPrologue=true;
			return true;
		}
	}
}