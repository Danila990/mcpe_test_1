using System.Threading.Tasks;

namespace Assets.Scripts.AD
{
	public class AdsTimer
	{
		private readonly float _timeToDelay;

		private Task _delayTask = Task.CompletedTask;

		/// <param name="timeToDelay">In seconds</param>
		public AdsTimer(float timeToDelay)
		{
			_timeToDelay = timeToDelay;
		}

		public bool IsTimerEnd => _delayTask.IsCompleted;

		public void StartTimer() 
		{
			_delayTask = Task.Delay((int)(_timeToDelay * 1000));
		}
	}
}
