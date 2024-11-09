using Godot;

using osu_data_analyzer.GraphsClass;
using osu_data_analyzer.Parser.ReplayParser;
using osu_data_analyzer.GraphsClass.Export;

using Godot.Collections;
using System.Linq;
using osu_data_analyzer.GraphsClass.HitClass;
using System;
using osu_data_analyzer.GraphsClass.MapHitClass;
using osu_data_analyzer.Parser.BeatmapParser;

using osu_data_analyzer.GraphsClass.Import.file;

public partial class firstgraph : Node2D
{
	string[] REPLAYNAME = //array of replay add your shit here
	{
		"/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/replay/replay-osu_3825223_4305279447.osr",
		"/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/replay/replay-osu_3825223_4308719895.osr",
		"/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/replay/replay-osu_3825223_4312088515.osr",
		"/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/replay/replay-osu_3825223_4330554965.osr",
		"/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/replay/replay-osu_3825223_4335213571.osr",
		"/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/replay/replay-osu_3825223_4434298260.osr",
		"/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/replay/replay-osu_3825223_4543557494.osr",
		"/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/replay/replay-osu_3825223_4604223577.osr",
		"/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/replay/replay-osu_3825223_4638973489.osr",
		"/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/replay/replay-osu_3825223_4652559940.osr"
	};


	string[] MAPNAME = //array of map add your shit here number 2
	{
		"/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/map/PSYQUI feat. mikanzil - endroll (Log Off Now) [Girl with an Acoustic Guitar and Birds and Bubbles].osu",
		"/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/map/Demetori - A Dream Transcending Space-time ~ Dream War (nooblet) [scrub].osu"
	};

	TestGraphs testGraphs;

	public override void _Ready()
	{
		testGraphs = new TestGraphs(REPLAYNAME.Length,true); //initialize 
		AddChild(testGraphs); // make it visible

		ImportFile importer = new();


		testGraphs.keyX = "Time in seconds"; //define the axis
		testGraphs.keyY = "average absolute aim error";

		AnalizerBeatmap lBeatmap = new(MAPNAME[1]);

		foreach(string replay in importer.GetReplay("/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/replay/fuki replay"))
		{
			testGraphs.AddReplay(replay,lBeatmap);
		};

		testGraphs.ProcessData(); 

		testGraphs.CreateDataPoint();

		//Export_Json export = new(testGraphs); //if needed other export should be easy to do in the export folder
		//export.Convert();
		//export.Save("/home/avelia/Desktop/osu data analyzer (1)/osu_data_analyzer/test.json");

	}
}



public partial class TestGraphs : Graphs
{

        public TestGraphs(int pN_replay = 1 , bool pMultiplay = false) : base(pN_replay,pMultiplay)
		{}

	public override void ProcessData()
	{
		Array<Dictionary<string,float>> lData = new() ;

		Dictionary<string,float> lPoint;

		foreach(MapHit hit in listMapHit )
		{
			lPoint = new();

			lPoint[keyX] = ((float)hit.hitObject.time)/1000;
			lPoint[keyY] = hit.TimeError(a => a);


			lData.Add(lPoint);
		}

		data = lData;
	}
}

