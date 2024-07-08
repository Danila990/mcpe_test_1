using Scripts.FileLoaders;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FileLoaders
{
	public struct LoadResult
	{
		public readonly long ResponseCode;
		public readonly string Error;
		public readonly string UrlToFile;

		public LoadResult(long responseCode, string error, string urlToFile)
		{
			ResponseCode = responseCode;
			Error = error;
			UrlToFile = urlToFile;
		}

		public LoadStatus GetLoadStatus()
		{
			LoadStatus result;
			if(ResponseCode == 200)
			{
				result = LoadStatus.Success;
			}
			else if(ResponseCode == 0 && !string.IsNullOrEmpty(Error))
			{
				if(Error.Contains("Request aborted"))
				{
					result = LoadStatus.Cancel;
				}
				else if(Error.Contains("Cannot resolve destination host") || Error.Contains("Cannot connect to destination host"))
				{
					result = LoadStatus.ConnectionError;
					ReportEvent();
				}
				else if(Error.Contains("Failed to receive data") || Error.Contains("Received no data in response"))
				{
					result = LoadStatus.ConnectionError;
					ReportEvent();
				}
				else if(Error.Contains("SSL CA certificate error") || Error.Contains("Unable to complete SSL connection"))
				{
					result = LoadStatus.InternalError;
				}
				else if(Error.Contains("502 HTTP/1.1 502 Bad Gateway"))
				{
					result = LoadStatus.InternalError;
					ReportEvent();
				}
				else if(Error.Contains("Request timeout"))
				{
					result = LoadStatus.ConnectionError;
					ReportEvent();
				}
				else
				{
					result = LoadStatus.UnknownError;
					ReportError();
					ReportEvent();
				}
			}
			else if(ResponseCode == 404)
			{
				result = LoadStatus.InternalError;
			}
			else
			{
				result = LoadStatus.UnknownError;
				ReportError();
			}

			return result;
		}

		private void ReportError()
		{
			string error = ResponseCode + " " + Error + " " + UrlToFile;
			Debug.LogError(error);
			//AppMetrica.Instance.ReportError(error, "");
		}

		private void ReportEvent()
		{
			Dictionary<string, object> errorHierarchy = new Dictionary<string, object>();
			errorHierarchy.Add("Error", Error);
			errorHierarchy.Add("UrlToFile", UrlToFile);
			//AppMetrica.Instance.ReportEvent("Connection error", errorHierarchy);
			Debug.Log(Error);
		}
	}
}
