using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AdController : MonoBehaviour
{

    public static AdController Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Advertisements.Instance.SetUserConsent(true);
        if (Advertisements.Instance.UserConsentWasSet())
        {
            Advertisements.Instance.Initialize();
        }
    }

    void Start()
    {

        showBanner();
    }

    // Show Banner Ad....................
    private void showBanner()
    {
        if (PlayerPrefs.GetString("removeAd", "no").Equals("no"))
            if (Advertisements.Instance.IsBannerAvailable())
                Advertisements.Instance.ShowBanner(BannerPosition.TOP, BannerType.Banner);
    }

    // Interstitial Ad....................
    public void showInterstitial()
    {
        if (PlayerPrefs.GetString("removeAd", "no").Equals("no"))
            Advertisements.Instance.ShowInterstitial(InterstitialClosed);
    }

    // Video Ad....................
    public void showVideoAd()
    {
        if (PlayerPrefs.GetString("removeAd", "no").Equals("no"))
            showUntiyInterstitial();
    }

    // Show Rewarded Ad.............
    public void showRewardAd()
    {
        if (PlayerPrefs.GetString("removeAd", "no").Equals("no"))
            if (Advertisements.Instance.GetInterstitialAdvertisers()[1].advertiserScript.IsRewardVideoAvailable())
                Advertisements.Instance.GetRewardedAdvertisers()[1].advertiserScript.ShowRewardVideo(CompleteMethod);
            else if (Advertisements.Instance.IsRewardVideoAvailable())
                Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
    }



    private void showUntiyInterstitial()
    {
        if (Advertisements.Instance.GetInterstitialAdvertisers()[1].advertiserScript.IsInterstitialAvailable())
        {
            Advertisements.Instance.GetInterstitialAdvertisers()[1].advertiserScript.ShowInterstitial(InterstitialClosed);
        }
        else if (Advertisements.Instance.IsRewardVideoAvailable())
        {
            Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
        }
        else
            showInterstitial();
    }


    private void InterstitialClosed()
    {
        if (Advertisements.Instance.debug)
        {
            Debug.Log("Interstitial closed -> Resume Game ");
            //	GleyMobileAds.ScreenWriter.Write("Interstitial closed -> Resume Game ");
        }
    }

    private void CompleteMethod(bool completed)
    {
        if (Advertisements.Instance.debug)
        {
            Debug.Log("Completed " + completed);
            //	GleyMobileAds.ScreenWriter.Write("Completed " + completed);
            if (completed == true)
            {
                //Debug.Log("check video ad");
            }
            else
            {
                //no reward
            }
        }
    }

}
