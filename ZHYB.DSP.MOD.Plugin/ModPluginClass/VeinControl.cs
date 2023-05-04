namespace ModClass
{
    internal class VeinControl
    {
        private static readonly Dictionary<EVeinType,int> veinAmount = new();
        public static PlanetFactory factory = null;
        private const int VEINPERCOUNT = 10*10000;
        public static bool CheatMode = false;

        private static void ClearAllVein()
        {
            veinAmount.Clear();

            foreach(EVeinType eVeinType in Enum.GetValues(typeof(EVeinType)))
            {
                if(LDB.veins.Select(( int )eVeinType)==null)
                    continue;
                veinAmount.Add(eVeinType,0);
            }

            foreach(var vein in factory.veinPool)
            {
                if(vein.id==0)
                    continue;
                if(veinAmount.TryGetValue(vein.type,out int vd))
                {
                    vd+=vein.amount;
                    veinAmount[vein.type]=vd;
                }
                else
                {
                    veinAmount.Add(vein.type,vein.amount);
                }

                if(factory.veinGroups[vein.groupIndex].count==0)
                {
                    factory.veinGroups[vein.groupIndex].type=0;
                    factory.veinGroups[vein.groupIndex].amount=0;
                    factory.veinGroups[vein.groupIndex].pos=Vector3.zero;
                }
                factory.RemoveVeinWithComponents(vein.id);
            }

            factory.RecalculateAllVeinGroups();
            factory.ArrangeVeinGroups();
        }

        private static void RefreshNewVein()
        {
            var idx =0;
            float lat=58.0f;
            foreach(var Pair in veinAmount)
            {
                if(( !CheatMode )&&( Pair.Value==0 ))
                    continue;
                int veintype =(int)Pair.Key;
                if(LDB.veins.Select(veintype)==null)
                    continue;
                float log=idx++*25;

                Vector3 pos =PostionCompute(lat,log,0,Pair.Key==EVeinType.Oil);

                int groupIndex=factory.AddVeinGroup(Pair.Key,pos);
                int CurrentCount;
                if(CheatMode)
                    CurrentCount=EVeinType.Oil==Pair.Key ? ( VEINPERCOUNT*1000/60 ) : VEINPERCOUNT*1000;
                else
                    CurrentCount=EVeinType.Oil==Pair.Key ? ( Pair.Value/60 ) : Pair.Value;
                if(CurrentCount==0)
                    continue;
                int veinPerCount=Pair.Key==EVeinType.Oil?( CurrentCount/4+1) :VEINPERCOUNT ;

                int index=0;
                while(CurrentCount>0)
                {
                    Vector3 tmpPos=PostionCompute(lat,log,index++,Pair.Key==EVeinType.Oil);
                    VeinData tmpV;
                    if(CurrentCount>veinPerCount)
                        tmpV=CreateNewVein(Pair.Key,groupIndex,veinPerCount,tmpPos);
                    else
                        tmpV=CreateNewVein(Pair.Key,groupIndex,CurrentCount,tmpPos);

                    CurrentCount-=tmpV.amount;
                }
            }
            factory.RecalculateAllVeinGroups();
            factory.ArrangeVeinGroups();
        }

        private static VeinData CreateNewVein(EVeinType type,int groupIndex,int count,Vector3 Pos)
        {
            int veintype=(int ) type;
            VeinData tmpV = new()
            {
                type=type,
                groupIndex=( short )groupIndex,
                amount=count,

                pos=Pos,
                modelIndex=( short )LDB.veins.Select(veintype).ModelIndex,
                productId=LDB.veins.Select(veintype).MiningItem,
                minerCount=0
            };

            tmpV.id=factory.AddVeinData(tmpV);
            tmpV.colliderId=factory.planet.physics.AddColliderData(LDB.veins.Select(veintype).prefabDesc.colliders[0].BindToObject(tmpV.id,0,EObjectType.Vein,tmpV.pos,Quaternion.FromToRotation(Vector3.up,tmpV.pos.normalized)));
            tmpV.modelId=factory.planet.factoryModel.gpuiManager.AddModel(tmpV.modelIndex,tmpV.id,tmpV.pos,Maths.SphericalRotation(tmpV.pos,UnityEngine.Random.value*360f));
            factory.planet.factoryModel.gpuiManager.AlterModel(factory.veinPool[tmpV.id].modelIndex,factory.veinPool[tmpV.id].modelId,tmpV.id,factory.veinPool[tmpV.id].pos,Maths.SphericalRotation(factory.veinPool[tmpV.id].pos,90f));
            factory.AssignGroupIndexForNewVein(ref tmpV);
            factory.veinPool[tmpV.id]=tmpV;
            factory.RefreshVeinMiningDisplay(tmpV.id,0,0);
            factory.RecalculateVeinGroup(factory.veinPool[tmpV.id].groupIndex);
            factory.ArrangeVeinGroups();

            return tmpV;
        }

        public static Vector3 PostionCompute(float lat,float log,int index,bool oil = false)
        {
            float areaRadius = oil ? 5f : 0.175f;
            int    lineCount = oil ?  1 : 10;

            return Maths.GetPosByLatitudeAndLongitude(
                lat+( index%lineCount )*areaRadius,
                log+( index/lineCount )*areaRadius,
                GameMain.localPlanet.realRadius);
        }

        public static void ControlVein()
        {
            if(GameMain.localPlanet.type==EPlanetType.Gas)
                return;

            ClearAllVein();
            RefreshNewVein();
        }
    }
}