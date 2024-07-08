using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class EventExecutorOnApplicationFocus : MonoBehaviour
	{
		public static event Action<bool> OnApplicationFocusEvent;

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		private void OnApplicationFocus(bool focus)
		{
			OnApplicationFocusEvent?.Invoke(focus);
		}
	}
}
