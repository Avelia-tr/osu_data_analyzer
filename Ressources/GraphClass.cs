using Godot;
using System;
using osu_data_analyzer.Parser.ReplayParser;
using osu_data_analyzer.Parser.BeatmapParser;
using System.Linq;
using System.Collections.Generic;
using Godot.Collections;
using osu_data_analyzer.GraphsClass.HitClass;
using osu_data_analyzer.GraphsClass.MapHitClass;


// Author : Avelia

namespace osu_data_analyzer.GraphsClass
{

    public partial class Graphs : Node2D // main class for data 
    {

        private AnalizerReplay[] replayData; 


    //those two store the data to analize in a simple manner
        public Hit[] listHit {get; private set;} 

        public MapHit[] listMapHit {get; private set;}

    // this array is where we store the data once it's processed
        public Array<Godot.Collections.Dictionary<string,float>> data {get; protected set; } 
        

        public List<Line2D> listPoints {get; protected set; } // store the point, this is where the scaling and all go

        public string keyX = "x"; 
        public string keyY = "y";

        private bool multiplay ;

        public Graphs(int pN_replay = 1 , bool pMultiplay = false)
        {
            replayData = new AnalizerReplay[pN_replay];

            Scale = new Vector2(1,-1); //just making it so up is positive

            multiplay = pMultiplay;
        }

        public void AddReplay(AnalizerReplay pReplay)
        {
            for (int i = 0; i <= replayData.Count() - 1; i++)
            {
                if(replayData[i] == null)
                {
                    replayData[i] = pReplay;
                    return;
                }
            }
        }


// those are the method for non synced replay

        public void AddReplay(string pReplay,AnalizerBeatmap pBeatmap, int pTolerance = 1)
        {
            AnalizerReplay lReplay = new AnalizerReplay(pReplay);
            Sync_With_Hitmap(
                        lReplay,
                        lReplay.Sync_with_map(pBeatmap, this, pTolerance)
                            );

            AddReplay(lReplay);      
        }

        public void Sync_With_Hitmap(AnalizerReplay pReplay, List<Hit> pListHit)
        {
            if (!multiplay) return;
            
            List<MapHit> lHitMapList = new();
            MapHit lMapHit ;

            foreach(HitObject hitObject in pReplay.map.map)
            {
                lMapHit = CreateHit(hitObject);

                lHitMapList.Add(lMapHit);

                pListHit
                    .Where( hit => hit.hitObject == hitObject ).ToList()
                    .ForEach( hit => lMapHit.AddHit(hit));
                
            }
            

            if (listMapHit == null) 
            {
                listMapHit = lHitMapList.ToArray();
                return;
            }
            listMapHit.Concat(lHitMapList).ToArray();

        }

        private MapHit CreateHit(HitObject pHitObject)
        {
            if (!pHitObject.HasMapHitBeenGenerated)
            {
                pHitObject.HasMapHitBeenGenerated = true;
                return new MapHit(pHitObject);
            }
            return listMapHit.Where(pMapHit => pHitObject == pMapHit.hitObject ).First();

        }



        public virtual void ProcessData() {} // to fill with data processing

        public virtual void CreateDataPoint() // overrideable for custom graph if you do you'll need to change other shit 
        {
            listPoints =  new List<Line2D>();

            

            Line2D lPoint;
            foreach (Godot.Collections.Dictionary<string, float> point in data)
            {
                lPoint = new Line2D();

                lPoint.AddPoint(Vector2.Zero);
                lPoint.AddPoint(new Vector2(0,10));

                lPoint.Position = new Vector2(point[keyX]*50,point[keyY]);
                lPoint.Scale *= 0.5f;


                listPoints.Add(lPoint);                
                this.AddChild(lPoint);
            }
        }

        public void AddData(List<Hit> pData)
        {
            if (listHit == null) 
            {
                listHit = pData.ToArray();
                return;
            }

            listHit.Concat(pData).ToArray();
        }

        

    }
}
