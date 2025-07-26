using GoogleMobileAds.Api;
using UnityEngine;

public class RewardedAdsManager : MonoBehaviour
{
	private RewardedAd rewardedAd;

#if UNITY_ANDROID
	private string adUnitId = "ca-app-pub-3940256099942544/5224354917"; // Test
#elif UNITY_IPHONE
    private string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string adUnitId = "unused";
#endif

	private void Start()
	{
		MobileAds.Initialize(initStatus =>
		{
			Debug.Log("AdMob initialized");
			LoadRewardedAd();
		});
	}

	public void LoadRewardedAd()
	{
		Debug.Log("Loading rewarded ad...");

		AdRequest request = new AdRequest(); // ✅ KHÔNG cần .Builder() nữa trong SDK 10+

		RewardedAd.Load(adUnitId, request, (RewardedAd ad, LoadAdError error) =>
		{
			if (error != null || ad == null)
			{
				Debug.LogError("Failed to load rewarded ad: " + error);
				return;
			}

			rewardedAd = ad;
			Debug.Log("Rewarded ad loaded");

			// Đăng ký sự kiện full screen content
			ad.OnAdFullScreenContentOpened += () =>
			{
				Debug.Log("Rewarded ad opened");
			};

			ad.OnAdFullScreenContentClosed += () =>
			{
				Debug.Log("Rewarded ad closed");
				LoadRewardedAd(); // load lại sau khi đóng
			};

			ad.OnAdFullScreenContentFailed += (AdError error) =>
			{
				Debug.LogError("Ad failed to show: " + error);
				LoadRewardedAd();
			};
		});
	}

	public void ShowRewardedAd()
	{
		if (rewardedAd != null && rewardedAd.CanShowAd())
		{
			rewardedAd.Show((Reward reward) =>
			{
				Debug.Log($"User earned reward: {reward.Amount} {reward.Type}");
				GiveReward();
			});
		}
		else
		{
			Debug.Log("Rewarded ad not ready");
		}
	}

	private void GiveReward()
	{
		MoneyManager.Instance.AddCoin(1000);
	}
}