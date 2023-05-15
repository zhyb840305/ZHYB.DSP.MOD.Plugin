using PowerNetworkStructures;

namespace ModClass
{
	public static class TestUIDysonEditor
	{
		private const int perAngle = 10;
		private static int[,] nodsIds;

		public static void TestdysonEditor()
		{
			UIDysonEditor dysonEditor=UIRoot.instance.uiGame.dysonEditor;

			DysonSphereLayer layer =UIRoot.instance.uiGame.dysonEditor?.selection?.singleSelectedLayer;
			if(layer==null)
				return;
			Array.Clear(nodsIds,0,0);
			nodsIds=new int[180/perAngle+10,360/perAngle+10];

			var nodeProtoId=dysonEditor.nodeProtoId;
			var frameProtoId=dysonEditor.frameProtoId;
			var shellProtoId=dysonEditor.shellProtoId;

			int idlog;
			int idlat;
			for(idlat=nodsIds.GetLowerBound(0);idlat<nodsIds.GetUpperBound(0);idlat++)
				for(idlog=nodsIds.GetLowerBound(1);idlog<nodsIds.GetUpperBound(1);idlog++)
				{
					int lat =-90+perAngle*idlat;
					int log=0+perAngle*idlog;
					var pos=Maths.GetPosByLatitudeAndLongitude(lat,log,layer.orbitRadius);
					nodsIds[idlat,idlog]=layer.NewDysonNode(nodeProtoId,pos);
				}

			for(idlat=nodsIds.GetLowerBound(0);idlat<nodsIds.GetUpperBound(0);idlat++)
				for(idlog=nodsIds.GetLowerBound(1);idlog<nodsIds.GetUpperBound(1);idlog++)
				{
					var nodeA=nodsIds[idlat,idlog];
					var nodeB=nodsIds[idlat+1,idlog];
					var nodeC=nodsIds[idlat+1,idlog+1];
					var nodeD=nodsIds[idlat,idlog+1];

					if(nodeA!=0&&nodeB!=0)
						layer.NewDysonFrame(frameProtoId,nodeA,nodeB,false);
					if(nodeB!=0&&nodeC!=0)
						layer.NewDysonFrame(frameProtoId,nodeB,nodeC,false);
					if(nodeC!=0&&nodeD!=0)
						layer.NewDysonFrame(frameProtoId,nodeC,nodeD,false);
					if(nodeD!=0&&nodeA!=0)
						layer.NewDysonFrame(frameProtoId,nodeD,nodeA,false);

					if(nodeA!=0&&nodeC!=0&&nodeC!=0&&nodeD!=0)
					{
						layer.NewDysonShell(shellProtoId,new List<int> { nodeA,nodeB,nodeC,nodeD });
					}
				}
		}
	}
}