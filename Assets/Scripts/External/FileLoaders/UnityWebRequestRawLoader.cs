using System.Threading.Tasks;
using System;
using UnityEngine.Networking;
using System.IO;
using Assets.Scripts.FileLoaders;
using System.Threading;
using UnityEngine;

namespace Scripts.FileLoaders.FromStreamingAssets
{
	public class UnityWebRequestRawLoader
	{
		private UnityWebRequest _webRequest;
		private float _preventProgress = -1;

		public event Action<float> OnProgressUpdate;

		public bool IsLoading => _webRequest != null;

		/// <exception cref="Exception">При неудачной записи будет брошен эксепшен</exception>
		public async Task<LoadResult> Load(string urlToDownloadFrom, string filePathToWrite, bool overwriteFile = false, 
			CancellationToken token = default)
		{
			if(overwriteFile && File.Exists(filePathToWrite))
			{
				File.Delete(filePathToWrite);
			}

			if(File.Exists(filePathToWrite))
			{
				return new LoadResult(200, null, urlToDownloadFrom);
			}

			string saveDirectoryPath = Path.GetDirectoryName(filePathToWrite);
			string tempFileName = $"{Path.GetRandomFileName()}.temp";
			string tempFilePathToWrite = Path.Combine(saveDirectoryPath, tempFileName);

			Directory.CreateDirectory(saveDirectoryPath);
			try
			{
				using(UnityWebRequest webRequest = await LoadInternal(urlToDownloadFrom, tempFilePathToWrite, token))
				{
					// File.Exists() на случай если другой лоадер уже скачал этот файл
					if(string.IsNullOrEmpty(webRequest.error) && !File.Exists(filePathToWrite))
					{
						File.Move(tempFilePathToWrite, filePathToWrite);
					}

					return new LoadResult(webRequest.responseCode, webRequest.error, urlToDownloadFrom);
				}
			}
			finally
			{
				if(File.Exists(tempFilePathToWrite))
				{
					File.Delete(tempFilePathToWrite);
				}
			}
		}

		/// <exception cref="Exception">При неудачной записи будет брошен эксепшен</exception>
		private async Task<UnityWebRequest> LoadInternal(string urlToDownloadFrom, string filePathToWrite, 
			CancellationToken token)
		{
			UnityWebRequest webRequest = UnityWebRequest.Get(urlToDownloadFrom);
			_webRequest = webRequest;
			try
			{
				DownloadHandlerFile downloadHandler = new DownloadHandlerFile(filePathToWrite);
				downloadHandler.removeFileOnAbort = true;
				webRequest.downloadHandler = downloadHandler;
				webRequest.SendWebRequest();

				while(webRequest.result == UnityWebRequest.Result.InProgress)
				{
					InvokeOnProgressUpdateIfProgressChanged(webRequest.downloadProgress);
					await Task.Yield();
					token.ThrowIfCancellationRequested();
				}
			}
			catch(OperationCanceledException)
			{
				webRequest.Abort();
				throw;
			}
			finally
			{
				_webRequest = null;
				_preventProgress = -1;
			}

			return webRequest;
		}

		private void InvokeOnProgressUpdateIfProgressChanged(float progress)
		{
			if(!Mathf.Approximately(_preventProgress, progress))
			{
				OnProgressUpdate?.Invoke(progress);
				_preventProgress = progress;
			}
		}
	}
}
