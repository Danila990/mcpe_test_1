using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public static class NativeMessageBoxWrapper
    {
        private static AndroidJavaObject _androidMessageBoxObject;

        static NativeMessageBoxWrapper()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
		using AndroidJavaObject unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
		_androidMessageBoxObject = new AndroidJavaObject("com.kruzilininc.unity.MessageBox", currentActivity);
#endif
        }

        public static void Show(string message)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
		_androidMessageBoxObject.Call("Show", message);
#endif
            Debug.Log(message);
        }
    }
}