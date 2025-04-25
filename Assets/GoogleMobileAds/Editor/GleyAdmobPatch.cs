using UnityEditor;

namespace GoogleMobileAds.Editor
{
    public class GleyAdmobPatch
    {
        public static void SetAdmobAppID(string androidAppId, string iosAppID)
        {
#if USE_ADMOB
            GoogleMobileAdsSettings.LoadInstance().DelayAppMeasurementInit = true;
            GoogleMobileAdsSettings.LoadInstance().GoogleMobileAdsAndroidAppId = androidAppId;
            GoogleMobileAdsSettings.LoadInstance().GoogleMobileAdsIOSAppId = iosAppID;
            EditorUtility.SetDirty(GoogleMobileAdsSettings.LoadInstance());
#endif
        }
    }
}