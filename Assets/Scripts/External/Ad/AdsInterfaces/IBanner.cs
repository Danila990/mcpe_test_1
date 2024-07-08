using System;

namespace Scripts.AD.AdsInterfaces
{
	public interface IBanner
	{
		public event Action<IBanner> OnLoadedEvent;

		public void Show();

		public void Hide();
	}
}