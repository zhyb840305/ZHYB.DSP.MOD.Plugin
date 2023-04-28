using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patch
{
    [HarmonyPatch(typeof(PlayerAction_Build))]
    internal class PatchPlayerAction_Build
    {
        [HarmonyPrefix]
        [HarmonyPatch("DoDismantleObject")]
        public static bool DoDismantleObject(PlayerAction_Build __instance,int objId,bool __result)
        {
            var factory=  __instance.factory;
            __instance.SetFactoryReferences();
            if(objId==0)
            {
                __result=false;
            }

            if(objId>0&&factory.entityPool[objId].id==0)
            {
                __result=false;
            }

            if(objId<0&&factory.prebuildPool[-objId].id==0)
            {
                __result=false;
            }

            int protoId =  __instance.noneTool.GetItemProto(objId)?.ID ?? 0;
            int num = protoId;
            if(objId>0)
            {
                EntityData[] entityPool = factory.entityPool;
                PowerSystem powerSystem = factory.powerSystem;
                int powerAccId = entityPool[objId].powerAccId;
                powerSystem.accPool[powerAccId].curEnergy=powerSystem.accPool[powerAccId].maxEnergy;
                if(powerAccId>0&&powerSystem.accPool[powerAccId].curEnergy==powerSystem.accPool[powerAccId].maxEnergy)
                {
                    ItemProto itemProto = LDB.items.Select(entityPool[objId].protoId);
                    if(itemProto!=null&&itemProto.HeatValue==0L)
                    {
                        int modelIndex = itemProto.ModelIndex;
                        itemProto=LDB.items.Select(entityPool[objId].protoId+1);
                        itemProto.HeatValue=powerSystem.accPool[powerAccId].maxEnergy;
                        if(itemProto!=null&&itemProto.HeatValue==powerSystem.accPool[powerAccId].maxEnergy&&modelIndex==itemProto.ModelIndex)
                        {
                            num++;
                        }
                    }
                }
            }

            int num2 = __instance. ObjectAssetValue(objId);
            if(num2>0)
            {
                int upCount =  __instance.player.TryAddItemToPackage(num, num2, 0, throwTrash: true, objId);
                UIItemup.Up(num,upCount);
            }

            factory.DismantleFinally(__instance.player,objId,ref protoId);
            __result=true;
            return false;
        }
    }
}