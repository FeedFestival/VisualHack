using System.Collections.Generic;
using AudienceNetwork;
using AudienceNetwork.Utility;
using Facebook.Unity;
using UnityEngine;

//using AudienceNetwork;

public class FacebookController : MonoBehaviour
{
    private Main _main;

    private AdView _adView;
    private InterstitialAd _interstitialAd;

    public bool IsShowingBanner;

    public void Initialize(Main main)
    {
        _main = main;
    }

    public void Login()
    {
        if (FB.IsInitialized)
        {
            CallFacebookLogin();
        }
        else
        {
            FB.Init(OnInitComplete, OnHideUnity);
        }
    }

    private void OnInitComplete()
    {
        if (FB.IsLoggedIn == false)
        {
            CallFacebookLogin();
        }
    }

    private void CallFacebookLogin()
    {
        var permisions = new List<string> { "public_profile", "email", "user_friends" };

        FB.LogInWithReadPermissions(permisions, HandleResult);
    }

    private void HandleResult(ILoginResult result)
    {
        if (result.Error != null)
        {
            _main.DebugText = result.Error;
        }

        if (FB.IsLoggedIn)
        {
            FB.API("/me?fields=id,first_name,last_name,middle_name,link,email,token_for_business", HttpMethod.GET, nameResult =>
            {
                if (nameResult.Error != null)
                {
                    _main.DebugText = nameResult.Error;
                    return;
                }
                var user = new User
                {
                    FacebookApp = new FacebookApp
                    {
                        FacebookId = long.Parse(nameResult.ResultDictionary["id"].ToString()),
                        BToken = nameResult.ResultDictionary["token_for_business"].ToString()
                    },
                    FirstName = nameResult.ResultDictionary["first_name"].ToString(),
                    MiddleName = nameResult.ResultDictionary["middle_name"].ToString(),
                    LastName = nameResult.ResultDictionary["last_name"].ToString(),
                    Email = nameResult.ResultDictionary["email"].ToString()
                };

                user.Name = (user.FirstName + "." + (string.IsNullOrEmpty(user.MiddleName) ? "" : user.MiddleName + ".") +
                            user.LastName).ToLower();

                _main.FacebookInitializationComplete(user);
            });
        }
        else
        {
            _main.DebugText = "Log in failed...";
            _main.FacebookInitializationComplete();
        }
    }

    public void GetProfilePicture()
    {
        FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, picResult =>
        {
            if (picResult.Error != null)
            {
                _main.DebugText = picResult.Error;
                return;
            }
            if (picResult.Texture != null)
            {
                _main.SetProfilePicture(picResult.Texture);
            }
            else
            {
                _main.DebugText = "no Profile Pic.";
            }
        });
    }

    private void OnHideUnity(bool isunityshown)
    {

    }

    /*
     * In MAIN MENU
         */

    public void ShowAndroidBanner(bool value)
    {
        IsShowingBanner = value;
        if (Application.platform == RuntimePlatform.Android)
        {
            if (value)
            {
                // Create a banner's ad view with a unique placement ID (generate your own on the Facebook app settings).
                // Use different ID for each ad placement in your app.
                AdView adView = new AdView("267365840301857_269678260070615", AdSize.BANNER_HEIGHT_50);
                _adView = adView;
                _adView.Register(_main.Canvas);

                // Set delegates to get notified on changes or when the user interacts with the ad.
                _adView.AdViewDidLoad = delegate
                {
                    _main.DebugText = "Ad view loaded.";
                    //adView.Show(100);
                    adView.Show(AdUtility.height() - 50);
                };
                adView.AdViewDidFailWithError = delegate (string error)
                {
                    _main.DebugText = "Ad view failed to load with error: " + error;
                };
                adView.AdViewWillLogImpression = delegate
                {
                    _main.DebugText = "Ad view logged impression.";
                };
                adView.AdViewDidClick = delegate
                {
                    _main.DebugText = "Ad view clicked.";
                };

                // Initiate a request to load an ad.
                adView.LoadAd();
                return;
            }
            _main.DebugText = "Ad view disposed.";
            _adView.Dispose();
        }
    }

    /*
     * End GAME
         */

    public void LoadInterstitial()
    {
        if (_interstitialAd == null)
        {
            _main.DebugText = "Loading interstitial ad...";

            // Create the interstitial unit with a placement ID (generate your own on the Facebook app settings).
            // Use different ID for each ad placement in your app.
            var interstitialAd = new InterstitialAd("267365840301857_269678813403893");
            _interstitialAd = interstitialAd;
            _interstitialAd.Register(_main.Canvas);

            // Set delegates to get notified on changes or when the user interacts with the ad.
            _interstitialAd.InterstitialAdDidLoad = delegate
            {
                _main.DebugText = "Interstitial ad loaded. Click show to present!";

                var val = _interstitialAd.Show();

                _main.DebugText = "Interstitial ad loaded value: " + val;
            };
            interstitialAd.InterstitialAdDidFailWithError = delegate (string error)
            {
                _main.DebugText = "Interstitial ad failed to load. Check console for details." + error;
            };
            interstitialAd.InterstitialAdWillLogImpression = delegate ()
            {
                _main.DebugText = "Interstitial ad logged impression.";
            };
            interstitialAd.InterstitialAdDidClick = delegate
            {
                _main.DebugText = "Interstitial ad clicked.";
            };

            // Initiate the request to load the ad.
            _interstitialAd.LoadAd();
        }
    }

    void OnDestroy()
    {
        // Dispose of banner ad when the scene is destroyed
        if (_adView)
        {
            _adView.Dispose();
        }
        _main.DebugText = "AdViewTest was destroyed!";

        // Dispose of interstitial ad when the scene is destroyed
        if (_interstitialAd != null)
        {
            _interstitialAd.Dispose();
        }
        _main.DebugText = "InterstitialAdTest was destroyed!";
    }
}