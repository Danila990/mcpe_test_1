using Scripts.FileLoaders;
using Scripts.FileLoaders.LoadersInterfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts.FileLoaders
{
	public class FileLoaderWithEvents : IFileLoader
	{
		private readonly IFileLoader _fileLoader;

		public FileLoaderWithEvents(IFileLoader fileLoader)
		{
			_fileLoader = fileLoader;
			_fileLoader.OnProgressUpdate += (value) => OnProgressUpdate?.Invoke(value);
		}

		public event Action OnLoadingStart;
		public event Action<float> OnProgressUpdate;
		public event Action<LoadStatus> OnLoadEnd;
		public event Action OnSuccessLoadEnd;

		public string PathToFile => _fileLoader.PathToFile;

		public bool IsLoading => _fileLoader.IsLoading;

		public bool IsLoaded => _fileLoader.IsLoaded;

		public bool IsOverwriteFile
		{
			get => _fileLoader.IsOverwriteFile;
			set => _fileLoader.IsOverwriteFile = value;
		}

		public async Task<LoadStatus> Load(CancellationToken token)
		{
			if(IsLoading)
			{
				return LoadStatus.InProgress;
			}

			OnLoadingStart?.Invoke();
			LoadStatus result = await _fileLoader.Load(token);
			if(result == LoadStatus.Success)
			{
				OnSuccessLoadEnd?.Invoke();
			}

			OnLoadEnd?.Invoke(result);
			return result;
		}
	}
}
