using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BepInEx;

using HarmonyLib;

using UnityEngine;

namespace BigUpgradeSize
{
	[BepInPlugin(Plugin_GUID,Plugin_NAME,Plugin_VERSION)]
	[BepInProcess(Plugin_Process)]
	public class BigUpgradeSize:BaseUnityPlugin
	{
		public const string Plugin_GUID = "ZHYB.DSP.MOD.BigUpgradeSize";
		public const string Plugin_NAME = "ZHYB.DSP.MOD.BigUpgradeSize";
		public const string Plugin_Process = "DSPGAME.exe";
		public const string Plugin_VERSION = "20230516.15.38";
		public const int cursorSize = 50;
		private Harmony harmony = new Harmony(Plugin_GUID);

		private void Start()
		{
			harmony=new Harmony(Plugin_GUID);
			harmony.PatchAll(typeof(PatchBuildTool_Upgrade));
		}

		private void OnDestroy()
		{
			harmony.UnpatchSelf();
		}
	}

	[HarmonyPatch(typeof(BuildTool_Upgrade))]
	public static class PatchBuildTool_Upgrade
	{
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
				else if(__instance.cursorSize>BigUpgradeSize.cursorSize)
				{
					__instance.cursorSize=BigUpgradeSize.cursorSize;
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
}