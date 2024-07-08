using System;
using System.Threading;
using System.Threading.Tasks;

namespace Scripts.FileLoaders.LoadersInterfaces
{
	public interface ILoader
	{
		public event Action<float> OnProgressUpdate;

		public bool IsLoading
		{
			get;
		}

		public bool IsLoaded
		{
			get;
		}

		public Task<LoadStatus> Load(CancellationToken token = default);
	}
}
