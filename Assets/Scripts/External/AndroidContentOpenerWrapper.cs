using UnityEngine;

namespace UnityAndroidNativeFileOpener
{
    /// <summary>
    /// Java wrapper class. this class demonstrates how to natively open files on Android in Unity.
    /// Largely adapted from NativeShare https://github.com/yasirkula/UnityNativeShare/blob/master/Plugins/NativeShare/NativeShare.cs
    /// </summary>
    public class AndroidContentOpenerWrapper
    {
        private static AndroidJavaClass m_ajc = null;

        private static AndroidJavaClass AJC
        {
            get
            {
                if (m_ajc == null)
                {
                    //Accesses .JAR Java class ContentOpener
                    m_ajc = new AndroidJavaClass("com.cartoontexas.andyr.unityplugin.ContentOpener"); 
                }

                return m_ajc;
            }
        }

        private static AndroidJavaObject m_context = null;

        private static AndroidJavaObject Context
        {
            get
            {
                if (m_context == null)
                {
                    using AndroidJavaObject unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    m_context = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
                }

                return m_context;
            }
        }

		/// <summary>
		/// Access to Java class method OpenContent
		/// </summary>
		/// <param name="filePath">Path to desired content to open</param>
		public static void OpenContent(string filePath)
		{
			AJC.CallStatic("OpenContent", Context, filePath);
		}
	}
}
