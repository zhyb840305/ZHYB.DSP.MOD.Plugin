namespace Patch
{
	[HarmonyPatch(typeof(PlayerController))]
	internal class Patch_PlayerController
	{
		[HarmonyPostfix, HarmonyPatch("GetInput")]
		public static void Postfix_GetInput(PlayerController __instance)
		{
			var prebuildPool=__instance.actionBuild?.player?.factory?.prebuildPool;
			var player=GameMain.mainPlayer;
			if(prebuildPool==null||UIGame.viewMode!=EViewMode.Build||player==null)
				return;
			bool flag = false;
			int preCount=0;
			Vector3 vector3 = new Vector3(0.0f, 0.0f, 0.0f);
			foreach(PrebuildData prebuildData in prebuildPool)
			{
				if(prebuildData.id!=0&&( prebuildData.itemRequired==0||prebuildData.itemRequired<=player.package.GetItemCount(prebuildData.protoId) ))
				{
					preCount++;
					var a= prebuildData.pos;
					var b= player.position;
					if(Maths.VectorSqrDistance(ref a,ref b)>GameMain.mainPlayer.mecha.buildArea/10)
					{
						flag=true;
						vector3=prebuildData.pos;
					}
				}
			}

			if(flag&&preCount>1000&&UIGame.viewMode==EViewMode.Build)
			{
				player.Order(new OrderNode()
				{
					target=vector3,
					type=EOrderType.Move
				},false);
			}
		}
	}
}