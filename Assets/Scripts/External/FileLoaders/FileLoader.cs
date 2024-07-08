using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.Http;
using Scripts.FileLoaders.LoadersInterfaces;
using Scripts.FileLoaders.FromStreamingAssets;
using System.Threading;

namespace Scripts.FileLoaders
{
	/// <summary>
	/// Будет сохранено в FolderToSave.FolderPath
	/// </summary>
	public class FileLoader : IFileLoader
	{
		private readonly string _fileKey;
		private readonly UnityWebRequestRawLoader _loader;

		public FileLoader(string fileKey)
		{
			_fileKey = fileKey;
			_loader = new UnityWebRequestRawLoader();

			_loader.OnProgressUpdate += progress => OnProgressUpdate?.Invoke(progress);
		}

		public event Action<float> OnProgressUpdate;

		public string PathToFile => Path.Combine(FolderToSave.FolderPath, _fileKey);

        public bool IsLoading => _loader.IsLoading;

		public bool IsLoaded => File.Exists(PathToFile);

		public bool IsOverwriteFile
		{
			get;
			set;
		}
		= true;

		public async Task<LoadStatus> Load(CancellationToken token)
		{
			if(IsLoading)
			{
				return LoadStatus.InProgress;
			}

			if(IsLoaded && !IsOverwriteFile)
			{
				return LoadStatus.Success;
			}

			return await LoadWithTryCatch(token);
		}

		private async Task<LoadStatus> LoadWithTryCatch(CancellationToken token)
		{
			if(IsLoading)
			{
				return LoadStatus.InProgress;
			}

			LoadStatus result;
			try
			{
                string urlToFile = Path.Combine(Application.streamingAssetsPath, _fileKey);
                var loadStatus = await _loader.Load(urlToFile, PathToFile, IsOverwriteFile, token);
				result = loadStatus.GetLoadStatus();
			}
			catch(OperationCanceledException)
			{
				result = LoadStatus.Cancel;
			}
			catch(HttpRequestException ex)
			{
				Debug.LogException(ex);
				result = LoadStatus.ConnectionError;
			}
			catch(WebException ex)
			{
				Debug.LogException(ex);
				result = LoadStatus.ConnectionError;
			}
			catch(FileNotFoundException ex) // Случай когда во время установки пользователь удалил файлы
			{
				Debug.LogException(ex);
				result = LoadStatus.InternalError;
			}
			catch(ArgumentException ex)
			{
				Debug.LogException(ex);
				result = LoadStatus.InternalError;
			}
			catch(UnauthorizedAccessException ex)
			{
				Debug.LogException(ex);
				result = LoadStatus.NoAccess;
			}
			catch(Exception ex)
			{
				if(ex.Message.Contains("Disk full."))
				{
					result = LoadStatus.DiskIsFull;
				}
				else
				{
					Debug.LogException(ex);
					//AppMetrica.Instance.ReportUnhandledException(ex);
					result = LoadStatus.UnknownError;
				}
			}

			return result;
		}
	}
}
