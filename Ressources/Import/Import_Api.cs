using Godot;
using System;
using osu_data_analyzer.GraphsClass.Import;

namespace osu_data_analyzer.GraphsClass.Import.API
{
    class ImportAPI : IImport
    {
		public ImportAPI(string pApiSecret , string pApiKey)
		{

		}

        public string[] GetReplay(string pPath)
        {
            return new string[0];
        }

    }
}
