using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewUser : MonoBehaviour
{
    public GameObject registerScreen;
    public GameObject loginScreen;
    public void NewUsers()
    {
        loginScreen.SetActive(false);
        registerScreen.SetActive(true);
    }
}
