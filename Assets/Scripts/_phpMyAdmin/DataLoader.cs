using System;
using UnityEngine;
using System.Collections;

public class DataLoader : MonoBehaviour
{
    public string[] Users;

    void Start()
    {
        StartCoroutine(GetUsers());

        //CreateUser(
        //    new User
        //    {
        //        Name = "TEST",
        //        Maps = 0,
        //        IsUsingSound = false,
        //        ControllerType = 0,
        //        FacebookId = 0
        //    });
    }

    // Use this for initialization
    IEnumerator GetUsers()
    {
        WWW userData = new WWW("http://localhost:8080/GameScript,Service/UserService/addUser.php");

        yield return userData;

        Users = userData.text.Split(';');

        if (Users.Length == 2)
        {
            var user = User.FillData(Users[0]);

            Debug.Log(user.ToString());
        }
        else
            foreach (var userRow in Users)
            {
                if (string.IsNullOrEmpty(userRow))
                    continue;

                var user = User.FillData(userRow);
                Debug.Log(user.ToString());
            }
    }

    public void CreateUser(User user)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", user.Name);
        form.AddField("maps", user.Maps.ToString());
        form.AddField("isUsingSound", user.IsUsingSound ? "1" : "0");
        form.AddField("controllerType", user.ControllerType.ToString());
        form.AddField("facebookId", user.FacebookApp.FacebookId.ToString());

        WWW userData = new WWW("http://localhost:8080/FeedFestStudio/InsertUser.php", form);
    }
}