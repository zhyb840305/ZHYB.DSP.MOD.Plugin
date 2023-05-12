using PowerNetworkStructures;

namespace ModClass
{
	public static class TestUIDysonEditor
	{
		private static int[,] nodsIds = new int[1000,1000];

		public static void TestdysonEditor()
		{
			ModCommon.ModCommon.Log("TestdysonEditor");

			UIDysonEditor dysonEditor=UIRoot.instance.uiGame.dysonEditor;
			DysonSphereLayer layer =UIRoot.instance.uiGame.dysonEditor?.selection?.singleSelectedLayer;
			if(layer==null)
				return;
			Array.Clear(nodsIds,0,0);
			var nodeProtoId=dysonEditor.nodeProtoId;
			var frameProtoId=dysonEditor.frameProtoId;
			var shellProtoId=dysonEditor.shellProtoId;
			int idlat=0;

			int idlog=0;

			for(int lat = -90;lat<=90;lat=lat+10)
			{
				idlog=0;
				for(int log = 0;log<=360;log=log+10)
				{
					var pos=Maths.GetPosByLatitudeAndLongitude(lat,log,layer.orbitRadius);
					nodsIds[idlat,idlog]=layer.NewDysonNode(nodeProtoId,pos);
					idlog++;
				}
				idlat++;
			}

			for(idlat=0;idlat<1000-1;idlat++)
				for(idlog=0;idlog<1000-1;idlog++)
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