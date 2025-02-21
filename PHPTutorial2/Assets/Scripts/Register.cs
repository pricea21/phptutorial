using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField confirmPassInput;
    public Button submitButton;
    public GameObject alreadyExist;

    // Start is called before the first frame update
    void Start()
    {
        submitButton.onClick.AddListener(() =>
        {
            if (passwordInput.text == confirmPassInput.text)
            {
                alreadyExist.SetActive(false);
                StartCoroutine(Main.Instance.Web.RegisterUser(usernameInput.text, passwordInput.text));
            }
            else
            {
                alreadyExist.SetActive(true);
            }
        });
    }
}
