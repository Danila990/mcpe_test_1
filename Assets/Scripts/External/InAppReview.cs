using System.Threading.Tasks;
using Google.Play.Review;
using UnityEngine;

public static class InAppReview
{
	private const string UserRatedUsKey = "UserRatedUs";

	private static readonly string _appBundleUrl = @"https://play.google.com/store/apps/details?id=" + Application.identifier;

	public async static void RateInApp()
	{
		ReviewManager reviewManager = new ReviewManager();
		var requestFlowOperation = reviewManager.RequestReviewFlow();
		while(!requestFlowOperation.IsDone)
		{
			await Task.Yield();
		}

		if(!requestFlowOperation.IsSuccessful)
		{
			return;
		}

		var reviewAsyncOperation = reviewManager.LaunchReviewFlow(requestFlowOperation.GetResult());
		while(!reviewAsyncOperation.IsDone)
		{
			await Task.Yield();
		}
	}

	public static void RateByLink()
	{
		Application.OpenURL(_appBundleUrl);
		PlayerPrefs.SetInt(UserRatedUsKey, 1);
	}
}