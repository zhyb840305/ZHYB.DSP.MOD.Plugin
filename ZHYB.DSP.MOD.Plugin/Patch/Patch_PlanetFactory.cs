namespace Patch
{
	[HarmonyPatch(typeof(PlanetFactory))]
	internal class Patch_PlanetFactory
	{
		private const int pilerLvl = 4;

		[HarmonyPrefix]
		[HarmonyPatch("PickFrom")]
		public static bool PickFromPatch(ref PlanetFactory __instance,int entityId,int offset,int filter,int[] needs,ref byte stack,ref byte inc,ref int __result,out byte __state)
		{
			PlanetFactory planetFactory = __instance;
			int beltId = planetFactory.entityPool[entityId].beltId;
			__state=( byte )0;
			if(beltId>0)
				return true;
			int assemblerId = planetFactory.entityPool[entityId].assemblerId;
			if(assemblerId>0)
			{
				lock(planetFactory.entityMutexs[entityId])
				{
					int[] products = planetFactory.factorySystem.assemblerPool[assemblerId].products;
					int[] produced = planetFactory.factorySystem.assemblerPool[assemblerId].produced;
					if(products==null)
					{
						__result=0;
						return false;
					}

					for(int index = 0;index<products.Length;++index)
					{
						if(( filter==products[index]||filter==0 )&&produced[index]>=pilerLvl&&products[index]>0&&( needs==null||needs[0]==products[index]||needs[1]==products[index]||needs[2]==products[index]||needs[3]==products[index]||needs[4]==products[index]||needs[5]==products[index] ))
						{
							produced[index]-=pilerLvl;
							__state=( byte )pilerLvl;
							__result=products[index];
							return false;
						}
					}
				}
				__result=0;
				return false;
			}
			int labId = __instance.entityPool[entityId].labId;
			if(labId<=0)
				return true;
			lock(__instance.entityMutexs[entityId])
			{
				int[] products = __instance.factorySystem.labPool[labId].products;
				int[] produced = __instance.factorySystem.labPool[labId].produced;
				if(products==null||produced==null)
				{
					__result=0;
					return false;
				}
				for(int index = 0;index<products.Length;++index)
				{
					if(produced[index]>=pilerLvl&&products[index]>0&&( filter==0||filter==products[index] )&&( needs==null||needs[0]==products[index]||needs[1]==products[index]||needs[2]==products[index]||needs[3]==products[index]||needs[4]==products[index]||needs[5]==products[index] ))
					{
						produced[index]-=pilerLvl;
						__state=( byte )pilerLvl;
						__result=products[index];
						return false;
					}
				}
			}
			__result=0;
			return false;
		}

		[HarmonyPostfix]
		[HarmonyPatch(typeof(PlanetFactory),"PickFrom")]
		public static void PickFromPatch2(ref byte stack,byte __state)
		{
			if(__state<=( byte )0)
				return;
			stack=__state;
		}
	}
}