using Godot;

namespace osu_data_analyzer.GraphsClass.Export
{

	public partial class Export_Json : IExport
	{
		Graphs graph;

		string graphJson;
		public Export_Json(Graphs pgraph)
		{
			graph = pgraph;
		}

		public void Convert()
		{
			graphJson = Json.Stringify(graph.data);

		}

		public void  Save(string pPath)
		{
			using var lfile = FileAccess.Open(pPath,FileAccess.ModeFlags.Write);
			lfile.StoreString(graphJson);
			lfile.Close();

		}
	}
}