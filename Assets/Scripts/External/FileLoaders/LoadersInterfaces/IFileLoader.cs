namespace Scripts.FileLoaders.LoadersInterfaces
{
	public interface IFileLoader : ILoader
	{
		public bool IsOverwriteFile
		{
			get;
			set;
		}

		public string PathToFile
		{
			get;
		}
	}
}
