﻿namespace Patch
{
	[HarmonyPatch(typeof(BuildingParameters))]
	internal class Patch_BuildingParameters
	{
		[HarmonyPostfix]
		[HarmonyPatch("ApplyPrebuildParametersToEntity")]
		public static void ApplyPrebuildParametersToEntity(int entityId,int recipeId,int filterId,int[] parameters,PlanetFactory factory)
		{
			int stationId = factory.entityPool[entityId].stationId;
			if(stationId==0)
				return;
			StationComponent component = factory.transport.stationPool[stationId];
			if(component==null)
				return;
			PrefabDesc prefabDesc = LDB.items.Select( factory.entityPool[component.entityId].protoId).prefabDesc;
			component.setToggle();
			component.SetCharge(prefabDesc);
			component.AddWarperRequestToLastSlot();
		}
	}
}