namespace osu_data_analyzer.GraphsClass.Export 
{

interface IExport 
{

	public abstract void Convert();

	public abstract void Save(string pPath);
}

}