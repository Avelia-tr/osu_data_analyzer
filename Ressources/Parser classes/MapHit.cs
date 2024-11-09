using Godot;
using System.Linq;
using System.Collections;
using osu_data_analyzer.GraphsClass.HitClass;
using osu_data_analyzer.Parser.BeatmapParser;
using System;

namespace osu_data_analyzer.GraphsClass.MapHitClass
{
	public class MapHit : IEnumerator,IEnumerable//this class is as a replacement to hit to use when analyzing multiple play on 1 beatmap
	{

		public AnalizerBeatmap map {get; private set;}

		public HitObject hitObject {get; private set;}

		public Hit[] hits {get; private set;}


		private int position = 0;

		public MapHit(HitObject pObject) 
		{
			map = pObject.map;
			hitObject = pObject;
			hits = new Hit[0];
		}
        public void AddHit(Hit pHit)
		{
			hits = hits.Append(pHit).ToArray(); 
		}
		public int DeltaTempObject()
		{
			return hitObject.time - hitObject.prevHitObject.time;
		}

		public float DistancePrevObject() 
		{
			return hitObject.placement.DistanceTo(hitObject.prevHitObject.placement);
		}

		public float TimeError(Func<float, float> Fcustom = null)
		{
			Fcustom ??= (float a) => { return a > 0 ? a : -a;};
			
			return hits.Length > 0 ? hits.Average(pHit => Fcustom(pHit.TimeError())) : 0 ;
		}

        public Vector2 AimError(Func<float, float> Fcustom = null)
		{
			Fcustom ??= (float a) => Mathf.Abs(a);

			float lX = 0;
			float lY = 0;

			foreach(Hit lHit in hits)
			{
				Vector2 lVector = lHit.AimError();
				lX += Fcustom(lVector.X);
				lY += Fcustom(lVector.Y);
			}

			return new Vector2(lX,lY) / hits.Length;
		}

		public Vector2 DirectionFromPreviousObject()
		{
			return hitObject.prevHitObject.placement - hitObject.placement;
		}

		public Vector2 NormalizedAimError(Func<float, float> Fcustom = null)
		{
			return AimError(Fcustom).Rotated(DirectionFromPreviousObject().Angle());
		}


#region iterator
        object IEnumerator.Current 
		{
			get { return hits[position]; }
		}
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
        bool IEnumerator.MoveNext()
        {
			position++;
			return position < hits.Length;
        }
        void IEnumerator.Reset()
        {
            position = -1;
			
        }
#endregion

    }




}


