using Godot;
using System;

using OsuParsers.Beatmaps;
using OsuParsers.Decoders;

using osu_data_analyzer.Parser.ReplayParser;


// Author : Avelia


namespace osu_data_analyzer.Parser.BeatmapParser
{
    public class HitObject
    {
        public int time {get; private set;}
        public int endTime {get; private set;}
        public Godot.Vector2 placement {get; private set;}
        public bool isSlider {get; private set;}
        public int index {get; private set;} 
        public HitObject prevHitObject {get; private set;}
        public AnalizerBeatmap map;

        public AnalizerReplayFrame[] frame_Distance;
        public AnalizerReplayFrame[] frame_Time;
        
        public bool HasMapHitBeenGenerated ;


        public HitObject(int pTime,int pEndTime, System.Numerics.Vector2 pPlacement, int pIndex, AnalizerBeatmap pMap) 
        {
            time = pTime;
            endTime = pEndTime;

            placement = new Godot.Vector2(pPlacement.X,pPlacement.Y );

            index = pIndex;
            map = pMap;

            isSlider = time != endTime;

            if (index>0)     prevHitObject = map.map[index-1];
            else              prevHitObject = this;
        }

    }

    public class AnalizerBeatmap
    {
        public HitObject[] map {get; private set; }
        public int BeatmapLength {get; private set;}
        private Beatmap beatmap;



        public AnalizerBeatmap(string pBeatmap) 
        {
            beatmap = BeatmapDecoder.Decode(pBeatmap);

            BeatmapLength = beatmap.HitObjects.Count;

            map = new HitObject[BeatmapLength];

            int lI = 0;

            foreach (OsuParsers.Beatmaps.Objects.HitObject hitObject in beatmap.HitObjects)
            {
                map[lI] = new HitObject(hitObject.StartTime, 
                                        hitObject.EndTime , 
                                        hitObject.Position , 
                                        lI , 
                                        this );
                lI++;
            }
        }

    }


}
