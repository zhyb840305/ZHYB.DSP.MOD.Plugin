using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Patch
{
	public static class AutoBuild
	{
		public static bool AutoBuildEnabled = false;
		public static Text modeText;
	}

	[HarmonyPatch(typeof(PlayerController))]
	internal class Patch_PlayerController
	{
		[HarmonyPostfix, HarmonyPatch("GetInput")]
		public static void Postfix_GetInput(PlayerController __instance)
		{
			if(!AutoBuild.AutoBuildEnabled)
				return;
			var prebuildPool=__instance.actionBuild?.player?.factory?.prebuildPool;

			var player=GameMain.mainPlayer;
			bool flag = false;
			int preCount=0;

			Vector3 vector3 = new Vector3(0.0f, 0.0f, 0.0f);
			float num1 = 999f;
			foreach(PrebuildData prebuildData in prebuildPool)
			{
				if(prebuildData.id!=0&&( prebuildData.itemRequired==0||prebuildData.itemRequired<=player.package.GetItemCount(prebuildData.protoId) ))
				{
					preCount++;
					if(( double )num1>( double )( prebuildData.pos-player.position ).magnitude)
					{
						flag=true;
						vector3=prebuildData.pos;
						num1=( vector3-player.position ).magnitude;
					}
				}
			}

			if(flag)
			{
				player.Order(new OrderNode()
				{
					target=vector3,
					type=EOrderType.Move
				},false);
			}
			else if(preCount==0)
			{
				AutoBuild.AutoBuildEnabled=false;
			}
		}
	}
}