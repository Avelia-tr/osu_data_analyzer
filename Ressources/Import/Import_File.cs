using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu_data_analyzer.GraphsClass.Import.file
{
    class ImportFile : IImport
    {
		string[] ls =  {"ls"};
		Godot.Collections.Array output = new();

		List<string> firstlinqpass ;
		public ImportFile()
		{
		}

        public string[] GetReplay(string pPath)
        {
			GD.Print(OS.Execute("ls", new string[] {pPath},output, true));

			List<string> lResult = new() ;

			GD.Print(output);

			firstlinqpass = ((string)output[0]).Split("\n").ToList();
			firstlinqpass.RemoveAll(a => a == "");
			

            return firstlinqpass.Select( a =>pPath + "/" + a ).ToArray();
        }

    }
}

