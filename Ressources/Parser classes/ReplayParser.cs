using Godot;
using System;

using OsuParsers.Replays;
using OsuParsers.Decoders;
using OsuParsers.Replays.Objects;
using OsuParsers.Enums.Replays;

using System.Collections.Generic;

using osu_data_analyzer.Parser.BeatmapParser;
using osu_data_analyzer.GraphsClass;
using osu_data_analyzer.GraphsClass.HitClass;


// Author : Avelia Bard

namespace osu_data_analyzer.Parser.ReplayParser
{

	public partial class AnalizerReplayFrame
	{
		public int time { get; private set; }
		public Vector2 position { get; private set; }
		public StandardKeys keyInFrame { get; private set; }
		public StandardKeys keyPressedFrame { get; private set; }

		public HitObject hitObjectTime { get; private set; } 
        public HitObject hitObjectDistance { get; private set; } 

		public int index {get; private set;}

		public AnalizerReplay replay;


        public AnalizerReplayFrame(int pTime, Vector2 pPosition, StandardKeys pkey, StandardKeys pPressedkey , AnalizerReplay pReplay)
		{
			time = pTime;
			position = pPosition;
			keyInFrame = pkey;
			keyPressedFrame = pPressedkey;
			replay = pReplay;

			hitObjectTime = null;
			hitObjectDistance = null;

		}

    }

	public partial class AnalizerReplay // this is the class responsible to make the interface between the user and the library parsing the replays
	{
		public Replay replay { get; private set; }		
		public AnalizerReplayFrame[] frames { get; private set; }
		public AnalizerReplayFrame[] framesKey { get; private set; }
		public int replayLength { get; private set; }

		private const StandardKeys smoke = StandardKeys.Smoke;

		// replay info
		public string playerName { get; private set; }
		public string mapName { get; private set; }

		// synced data 
		public AnalizerBeatmap map { get; private set; }



		public AnalizerReplay(string pReplay, bool Autosync = false)
		{
			replay = ReplayDecoder.Decode(pReplay);

			playerName = replay.PlayerName;

			List<AnalizerReplayFrame> lListKeyPressed = new List<AnalizerReplayFrame>();

			replayLength = replay.ReplayFrames.Count;

			frames = new AnalizerReplayFrame[replayLength];

			int lI = 0;
			StandardKeys lkeyPressed;

			foreach (ReplayFrame frame in replay.ReplayFrames)
			{
				if (lI == 0)
				{
					lkeyPressed = frame.StandardKeys;
				}
				else
				{
					lkeyPressed = (frame.StandardKeys ^ frames[lI - 1].keyInFrame) & (~smoke) & ~frames[lI-1].keyInFrame;
				}

				frames[lI] = new AnalizerReplayFrame(
						frame.Time,
						new Vector2(frame.X, frame.Y),
						frame.StandardKeys,
						lkeyPressed,
						this
						);

				if (lkeyPressed != StandardKeys.None)
				{
					lListKeyPressed.Add(frames[lI]);					
				}

				lI++;
			}

			framesKey = lListKeyPressed.ToArray();
			
		}
		public List<Hit> Sync_with_map(AnalizerBeatmap pMap,Graphs pGraph, int pTolerance ) // pTolerance being the tolerance in n_circle of what is submitted for the sync
		{
			map = pMap;
			int lObjectIndex = 0; 

			List<Hit> lListHit = new();

			
			foreach(AnalizerReplayFrame frame in framesKey)
			{
				while (lObjectIndex < map.BeatmapLength && frame.time > map.map[lObjectIndex].time) // we find the nearest object
				{
					lObjectIndex++;
				}

				lObjectIndex = Mathf.Min(lObjectIndex, map.BeatmapLength -1);

				lListHit.Add
				(
					new Hit
					(
						map.map[lObjectIndex],
						CreateArray(map,lObjectIndex,pTolerance),
						frame,
						Array.IndexOf(framesKey,frame)-1 >= 0 ? framesKey[Array.IndexOf(framesKey,frame)-1] : null,
						Array.IndexOf(frames,frame)-1 >= 0 ? frames[Array.IndexOf(frames,frame)-1] : null
					)
				);
			}

			pGraph.AddData(lListHit);
			
			return lListHit;
		}


		private HitObject[] CreateArray(AnalizerBeatmap pMap, int pIndex, int pTolerance)
		{
			HitObject[] lArray = new HitObject[pTolerance*2 + 1];

			for (int lI = 0; lI > pTolerance*2 +1; lI++ )
			{
				lArray[lI] = pIndex-pTolerance+lI > 0 & pIndex-pTolerance+lI < pMap.BeatmapLength ? pMap.map[pIndex - pTolerance + lI ] : null;
			}

			return lArray;
		}

	}


}