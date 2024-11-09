using Godot;
using osu_data_analyzer.Parser.BeatmapParser;
using osu_data_analyzer.Parser.ReplayParser;
using osu_data_analyzer.GraphsClass;
using System;



namespace osu_data_analyzer.GraphsClass.HitClass
{

	public partial class Hit 
	{
		private AnalizerBeatmap map;
		private AnalizerReplay replay;

		public HitObject hitObject {get; private set;}
		public HitObject prevHitObject {get; private set;}

		public HitObject[] ObjectHistory {get; private set;}

		public AnalizerReplayFrame replayFrame {get; private set;}
		public AnalizerReplayFrame prevReplayFrame {get; private set;}
		public AnalizerReplayFrame prevKeyReplayFrame {get; private set;}

		public Hit(
					HitObject pHitobject,
					HitObject[] pObjectHistory,
					AnalizerReplayFrame pReplayFrame,
					AnalizerReplayFrame pPrevReplayFrame,
					AnalizerReplayFrame pPrevKeyReplayFrame
				  )
		{
			map = pHitobject.map;
			replay = pReplayFrame.replay;

			hitObject = pHitobject;
			prevHitObject = pHitobject.prevHitObject;
			ObjectHistory = pObjectHistory;

			replayFrame = pReplayFrame;
			prevReplayFrame = pPrevReplayFrame;
			prevKeyReplayFrame = pPrevKeyReplayFrame;

		}

		public float DeltaTempObject()
		{
			return hitObject.time - prevHitObject.time;
		}

		public float DistancePrevObject() 
		{
			return hitObject.placement.DistanceTo(prevHitObject.placement);
		}

		public float TimeError()
		{
			return hitObject.time - replayFrame.time;
		}

		public Vector2 AimError()
		{
			return hitObject.placement - replayFrame.position;
		}

		public Vector2 DirectionFromPreviousObject()
		{
			return prevHitObject.placement - hitObject.placement;
		}

		public Vector2 NormalizedAimError()
		{
			return AimError().Rotated(DirectionFromPreviousObject().Angle());
		}

	}

}
