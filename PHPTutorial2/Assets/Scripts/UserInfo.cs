using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    public string UserID { get; private set; }
    string UserName;
    string UserPassword;
    string Level;
    string Coins;

    public void SetCredentials(string username, string password)
    {
        UserName = username;
        UserPassword = password;
    }

    public void SetID(string id)
    {
        UserID = id;
    }
}
