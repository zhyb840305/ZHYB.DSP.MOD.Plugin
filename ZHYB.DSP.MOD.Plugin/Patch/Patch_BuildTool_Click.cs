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