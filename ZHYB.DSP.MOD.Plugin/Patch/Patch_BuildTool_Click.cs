using HarmonyLib;

using UnityEngine;

namespace Patch
{
	[HarmonyPatch(typeof(BuildTool_Click))]
	public static class Patch_BuildTool_Click
	{
		[HarmonyPostfix]
		[HarmonyPatch("_OnInit")]
		public static void Patch_Oninit(BuildTool_Click __instance)
		{
			__instance.dotsSnapped=new Vector3[500];
		}

		[HarmonyPostfix]
		[HarmonyPatch("CreatePrebuilds")]
		public static void SetGasStation(BuildTool_Click __instance)
		{
			if(__instance.planet.type!=EPlanetType.Gas||__instance.buildPreviews.Count==0)
				return;
			BuildPreview buildPreview = __instance.buildPreviews[0];
			if(!buildPreview.desc.isCollectStation)
				return;
			PlanetFactory factory = __instance.factory;
			StationComponent[] stationPool = factory.transport.stationPool;
			EntityData[] entityPool = factory.entityPool;
			int stationCursor = factory.transport.stationCursor;
			Vector3 v1 = buildPreview.lpos;
			Vector3 lpos2 = buildPreview.lpos2;
			int id = buildPreview.item.ID;
			int num1 =40;
			if(num1==0)
				return;
			List<int> intList = new List<int>(){-buildPreview.objId};
			Player player = __instance.player;
			Vector3 vector3_1 = v1;
			int num2 = PlanetGrid.DetermineLongitudeSegmentCount(0, factory.planet.aux.mainGrid.segment) * 5;
			double halfRad = Math.PI / (double) num2;
			int num3 = 0;
			for(int index = 1;index<=num2;++index)
			{
				v1=( Vector3 )Maths.RotateLF(0.0,1.0,0.0,halfRad,( VectorLF3 )v1);
				if(( double )( vector3_1-v1 ).sqrMagnitude>=14297.0)
				{
					num3=index;
					break;
				}
			}
			if(num3==0)
				return;
			Vector3 v2 = vector3_1;
			Vector3 vector3_2 = (Vector3) Maths.RotateLF(0.0, 1.0, 0.0, halfRad * (double) num3, (VectorLF3) v2);
			Vector3 vector3_3 = (Vector3) Maths.RotateLF(0.0, 1.0, 0.0, halfRad * (double) num3, (VectorLF3) lpos2);
			int num4 = num3;
			while(num4<num2&&num1!=0)
			{
				bool flag = false;
				for(int index = 1;index<stationCursor;++index)
				{
					if(stationPool[index]!=null&&stationPool[index].id==index&&( double )( entityPool[stationPool[index].entityId].pos-vector3_2 ).sqrMagnitude<14297.0)
					{
						flag=true;
						break;
					}
				}
				if(flag)
				{
					vector3_2=( Vector3 )Maths.RotateLF(0.0,1.0,0.0,halfRad,( VectorLF3 )vector3_2);
					vector3_3=( Vector3 )Maths.RotateLF(0.0,1.0,0.0,halfRad,( VectorLF3 )vector3_3);
					++num4;
				}
				else
				{
					int num5;
					if(player.inhandItemId==id&&player.inhandItemCount>0)
					{
						player.UseHandItems(1,out num5);
					}
					else
					{
						int count = 1;
						player.package.TakeTailItems(ref id,ref count,out num5);
						if(count==0)
							break;
					}
					Quaternion quaternion1 = Maths.SphericalRotation(vector3_2, 0.0f);
					Quaternion quaternion2 = Maths.SphericalRotation(vector3_3, 0.0f);
					PrebuildData prebuild = new PrebuildData();
					prebuild.protoId=( short )buildPreview.item.ID;
					prebuild.modelIndex=( short )buildPreview.desc.modelIndex;
					prebuild.pos=vector3_2;
					prebuild.pos2=vector3_3;
					prebuild.rot=quaternion1;
					prebuild.rot2=quaternion2;
					prebuild.pickOffset=( short )buildPreview.inputOffset;
					prebuild.insertOffset=( short )buildPreview.outputOffset;
					prebuild.recipeId=buildPreview.recipeId;
					prebuild.filterId=buildPreview.filterId;
					prebuild.InitParametersArray(buildPreview.paramCount);
					for(int index = 0;index<buildPreview.paramCount;++index)
						prebuild.parameters[index]=buildPreview.parameters[index];
					intList.Add(factory.AddPrebuildDataWithComponents(prebuild));
					--num1;
					vector3_2=( Vector3 )Maths.RotateLF(0.0,1.0,0.0,halfRad*( double )num3,( VectorLF3 )vector3_2);
					vector3_3=( Vector3 )Maths.RotateLF(0.0,1.0,0.0,halfRad*( double )num3,( VectorLF3 )vector3_3);
					num4+=num3;
				}
			}

			foreach(int prebuildId in intList)
				factory.BuildFinally(player,prebuildId);
		}
	}

	[HarmonyPatch(typeof(BuildTool_Upgrade))]
	public static class PatchBuildTool_Upgrade
	{
		public const int BigUpgradeSize = 50;

		[HarmonyPrefix]
		[HarmonyPatch("DeterminePreviews")]
		public static bool Patch_DeterminePreviews(BuildTool_Upgrade __instance)
		{
			int _overlappedCount=( int )Traverse.Create(__instance).Field("_overlappedCount").GetValue() ;
			int[] _overlappedIds=( int[] )Traverse.Create(__instance).Field("_overlappedIds").GetValue();
			if(!VFInput.onGUI)
			{
				if(__instance.upgradeLevel>=1)
				{
					UICursor.SetCursor(ECursor.Upgrade);
				}
				else if(__instance.upgradeLevel<=-1)
				{
					UICursor.SetCursor(ECursor.Downgrade);
				}
			}
			var castObjectId=__instance.castObjectId;

			__instance.buildPreviews.Clear();
			if(__instance.cursorType==0)
			{
				if(castObjectId!=0)
				{
					ItemProto itemProto = __instance.GetItemProto(castObjectId);
					Pose objectPose =__instance. GetObjectPose(castObjectId);
					bool flag = false;
					if(itemProto!=null&&itemProto.Grade>0&&itemProto.Upgrades.Length!=0)
					{
						flag=true;
					}

					if(flag)
					{
						PrefabDesc prefabDesc =__instance. GetPrefabDesc(castObjectId);
						if(( prefabDesc.isInserter&&__instance.filterInserter )||( prefabDesc.isBelt&&__instance.filterBelt )||( !prefabDesc.isInserter&&!prefabDesc.isBelt&&__instance.filterFacility ))
						{
							BuildPreview buildPreview = new BuildPreview();
							buildPreview.item=itemProto;
							buildPreview.desc=prefabDesc;
							buildPreview.lpos=objectPose.position;
							buildPreview.lrot=objectPose.rotation;
							buildPreview.lpos2=objectPose.position;
							buildPreview.lrot2=objectPose.rotation;
							buildPreview.objId=castObjectId;
							if(buildPreview.desc.lodCount>0&&buildPreview.desc.lodMeshes[0]!=null)
							{
								buildPreview.needModel=true;
							}
							else
							{
								buildPreview.needModel=false;
							}

							buildPreview.isConnNode=true;
							if(buildPreview.desc.isInserter)
							{
								Pose objectPose2 =__instance. GetObjectPose2(buildPreview.objId);
								buildPreview.lpos2=objectPose2.position;
								buildPreview.lrot2=objectPose2.rotation;
							}

							if(( buildPreview.lpos-__instance.player.position ).sqrMagnitude>__instance.player.mecha.buildArea*__instance.player.mecha.buildArea)
							{
								buildPreview.condition=EBuildCondition.OutOfReach;
								__instance.actionBuild.model.cursorText="目标超出范围".Translate();
								__instance.actionBuild.model.cursorState=-1;
							}
							else
							{
								buildPreview.condition=EBuildCondition.Ok;
								__instance.actionBuild.model.cursorText="升级".Translate()+buildPreview.item.name+"\r\n"+"连锁升级提示".Translate();
							}

							__instance.buildPreviews.Add(buildPreview);
						}
					}
				}
			}
			else if(__instance.cursorType==1)
			{
				Vector4 zero = Vector4.zero;
				if(VFInput._cursorPlusKey.onDown)
				{
					__instance.cursorSize++;
				}

				if(VFInput._cursorMinusKey.onDown)
				{
					__instance.cursorSize--;
				}

				if(__instance.cursorSize<1)
				{
					__instance.cursorSize=1;
				}
				else if(__instance.cursorSize>BigUpgradeSize)
				{
					__instance.cursorSize=BigUpgradeSize;
				}

				if(__instance.castGround)
				{
					zero=__instance.actionBuild.planetAux.activeGrid.GratboxByCenterSize(__instance.castGroundPos,__instance.cursorSize);
					bool flag2 = false;
					__instance.GetOverlappedObjectsNonAlloc(__instance.castGroundPos,1.5f*( float )__instance.cursorSize,1.5f*( float )__instance.cursorSize,ignoreAltitude: true);
					for(int i = 0;i<_overlappedCount;i++)
					{
						ItemProto itemProto2 = __instance.GetItemProto(_overlappedIds[i]);
						if(itemProto2==null||itemProto2.Grade<=0||itemProto2.Upgrades.Length==0)
						{
							continue;
						}

						PrefabDesc prefabDesc2 =__instance. GetPrefabDesc(_overlappedIds[i]);
						Pose objectPose3 =__instance. GetObjectPose(_overlappedIds[i]);
						Pose pose = (prefabDesc2.isInserter ?__instance. GetObjectPose2(_overlappedIds[i]) : objectPose3);
						if(( __instance.actionBuild.planetAux.activeGrid.IsPointInGratbox(objectPose3.position,zero)||( __instance.filterInserter&&prefabDesc2.isInserter&&__instance.actionBuild.planetAux.activeGrid.IsPointInGratbox(pose.position,zero) ) )&&( ( prefabDesc2.isInserter&&__instance.filterInserter )||( prefabDesc2.isBelt&&__instance.filterBelt )||( !prefabDesc2.isInserter&&!prefabDesc2.isBelt&&__instance.filterFacility ) ))
						{
							BuildPreview buildPreview2 = new BuildPreview();
							buildPreview2.item=itemProto2;
							buildPreview2.desc=prefabDesc2;
							buildPreview2.lpos=objectPose3.position;
							buildPreview2.lrot=objectPose3.rotation;
							buildPreview2.lpos2=objectPose3.position;
							buildPreview2.lrot2=objectPose3.rotation;
							buildPreview2.objId=_overlappedIds[i];
							if(buildPreview2.desc.lodCount>0&&buildPreview2.desc.lodMeshes[0]!=null)
							{
								buildPreview2.needModel=true;
							}
							else
							{
								buildPreview2.needModel=false;
							}

							buildPreview2.isConnNode=true;
							if(prefabDesc2.isInserter)
							{
								buildPreview2.lpos2=pose.position;
								buildPreview2.lrot2=pose.rotation;
							}

							if(( objectPose3.position-__instance.player.position ).sqrMagnitude>__instance.player.mecha.buildArea*__instance.player.mecha.buildArea)
							{
								buildPreview2.condition=EBuildCondition.OutOfReach;
								flag2=true;
							}
							else
							{
								buildPreview2.condition=EBuildCondition.Ok;
								__instance.buildPreviews.Add(buildPreview2);
							}
						}
					}

					if(!flag2)
					{
						__instance.actionBuild.model.cursorText="升级".Translate()+" ("+__instance.buildPreviews.Count+")\r\n"+"连锁升级提示".Translate();
						__instance.actionBuild.model.cursorState=0;
					}
					else
					{
						__instance.actionBuild.model.cursorText="目标超出范围".Translate();
						__instance.actionBuild.model.cursorState=-1;
					}
				}
			}

			if(__instance.chainReaction)
			{
				__instance.DetermineMoreChainTargets();
			}
			return false;
		}
	}

	[HarmonyPatch(typeof(BuildTool_BlueprintPaste))]
	public static class Patch_BuildTool_BlueprintPaste
	{
		[HarmonyPostfix]
		[HarmonyPatch("_OnInit")]
		public static void Patch_Oninit(BuildTool_BlueprintPaste __instance)
		{
			__instance.dotsSnapped=new Vector3[500];
		}
	}
}