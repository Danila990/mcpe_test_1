using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class NativeShare
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		private readonly AndroidJavaObject _activity = null;
		private AndroidJavaObject _sharingIntent = null;
#endif

		public NativeShare()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			_activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
						.GetStatic<AndroidJavaObject>("currentActivity");
			_sharingIntent = GetBaseSharingIntent();
#endif
		}

		public NativeShare SetSubject(string subject)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			_sharingIntent = _sharingIntent.Call<AndroidJavaObject>("putExtra", "android.intent.extra.SUBJECT", subject);
#endif
			return this;
		}

		public NativeShare SetText(string text)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			_sharingIntent = _sharingIntent.Call<AndroidJavaObject>("putExtra", "android.intent.extra.TEXT", text);
#endif
			return this;
		}

		public void Share()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent")
							  .CallStatic<AndroidJavaObject>("createChooser", _sharingIntent, null);
			_activity.Call("startActivity", intent);
#else
			Debug.Log("Share");
#endif
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		private AndroidJavaObject GetBaseSharingIntent()
		{
			var result = new AndroidJavaObject("android.content.Intent", "android.intent.action.SEND");
			result = result.Call<AndroidJavaObject>("setType", "text/plain");
			return result;
	}
#endif
}
}
