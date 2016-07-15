using UnityEngine;
using System.Collections.Generic;
using Facebook.Unity;
//using AudienceNetwork;

public class FacebookController : MonoBehaviour
{
    private Main _main;

    public void Initialize(Main main)
    {
        _main = main;

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
        //_main.DebugText = string.Format("Connected with Facebook. AppId ={0}, IsLoggedIn = {1}, IsInitialized = {2}", FB.AppId, FB.IsLoggedIn, FB.IsInitialized);
        //Debug.Log(_main.DebugText);

        if (FB.IsLoggedIn == false)
        {
            CallFacebookLogin();
        }
    }

    private void CallFacebookLogin()
    {
        var permisions = new List<string>() { "public_profile", "email", "user_friends" };

        FB.LogInWithReadPermissions(permisions, HandleResult);
    }

    private void HandleResult(ILoginResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }

        if (FB.IsLoggedIn)
        {
            FB.API("/me?fields=id,first_name,last_name", HttpMethod.GET, (nameResult) =>
            {
                if (nameResult.Error != null)
                {
                    Debug.Log(nameResult.Error);
                    return;
                }
                
                var user = new User
                {
                    FacebookId = long.Parse(nameResult.ResultDictionary["id"].ToString()),
                    Name = nameResult.ResultDictionary["first_name"].ToString() + " " + nameResult.ResultDictionary["last_name"].ToString()
                };

                _main.FacebookInitializationComplete(user);
            });

            //FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, (picResult) =>
            //{
            //    if (picResult.Error != null)
            //    {
            //        Debug.Log(picResult.Error);
            //        return;
            //    }

            //    if (picResult.Texture != null)
            //    {
            //        _main.SetProfilePicture(picResult.Texture);
            //    }
            //    else
            //    {
            //        Debug.Log("no Profile Pic.");
            //    }

            //});
        }
        else
        {
            Debug.Log("Log in failed...");
            _main.FacebookInitializationComplete();
        }
    }

    public void GetProfilePicture()
    {
        FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, (picResult) =>
        {
            if (picResult.Error != null)
            {
                Debug.Log(picResult.Error);
                return;
            }

            if (picResult.Texture != null)
            {
                _main.SetProfilePicture(picResult.Texture);
            }
            else
            {
                Debug.Log("no Profile Pic.");
            }

        });
    }

    private void OnHideUnity(bool isunityshown)
    {

    }
}