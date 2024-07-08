using System;
using System.Threading.Tasks;

namespace Assets.Scripts.AD.Applovin
{
	public class RequestRetryDelayer
	{
		public int MaxRetryAttempt;

		private int _retryAttempt;

		public RequestRetryDelayer(int maxRetryAttempt = 6) 
		{
			MaxRetryAttempt = maxRetryAttempt;
		}

		public async Task Wait()
		{
			_retryAttempt++;
			double retryDelay = Math.Pow(2, Math.Min(MaxRetryAttempt, _retryAttempt));
			await Task.Delay((int)(retryDelay * 1000));
		}

		public void Reset()
		{
			_retryAttempt = 0;
		}
	}
}
