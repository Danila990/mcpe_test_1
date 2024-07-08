using Scripts.FileLoaders.LoadersInterfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.FileLoaders
{
	public class FileLoaderRestartOnError : IFileLoader
	{
		private readonly IFileLoader _fileLoader;
		private readonly int _maxCooldown;

		private bool _isWaitingForCooldown;
		private int _cooldownBeforeNextRequest;

		public FileLoaderRestartOnError(IFileLoader fileLoader) : this(fileLoader, 32)
		{
		}

		public FileLoaderRestartOnError(IFileLoader fileLoader, int maxCooldown)
		{
			_fileLoader = fileLoader;
			_fileLoader.OnProgressUpdate+= (value) => OnProgressUpdate?.Invoke(value);
			_maxCooldown = maxCooldown;
			CooldownToBaseValue();
		}

		public event Action<float> OnProgressUpdate;

		public string PathToFile => _fileLoader.PathToFile;

		public bool IsLoading => _fileLoader.IsLoading || _isWaitingForCooldown == true;

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

			return await LoadWhileError(token);
		}

		private async Task<LoadStatus> LoadWhileError(CancellationToken token)
		{
			LoadStatus result = LoadStatus.InProgress;
			do
			{
				await WaitForCooldown(_cooldownBeforeNextRequest, token);
				UpCooldown();
				result = await _fileLoader.Load(token);
#if UNITY_EDITOR
				if(!Application.isPlaying)
				{
					result = LoadStatus.Cancel;
					break;
				}
#endif
			} while(result != LoadStatus.Success && result != LoadStatus.Cancel);

			return result;
		}

		private async Task WaitForCooldown(int waitSeconds, CancellationToken token)
		{
			try
			{
				_isWaitingForCooldown = true;
				await Task.Delay(TimeSpan.FromSeconds(waitSeconds), token);
			}
			finally 
			{
				_isWaitingForCooldown = false;
			}
		}

		private void UpCooldown()
		{
			_cooldownBeforeNextRequest *= 2;
			_cooldownBeforeNextRequest = _cooldownBeforeNextRequest > _maxCooldown ? _maxCooldown : _cooldownBeforeNextRequest;
		}

		private void CooldownToBaseValue()
		{
			_cooldownBeforeNextRequest = 1;
		}
	}
}
