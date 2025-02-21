using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Web : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(GetDate());
        //StartCoroutine(Login("testuser", "123456"));
        //StartCoroutine(RegisterUser("testuser3", "123456"));
    }

    public void ShowUserItems()
    {
        StartCoroutine(GetItemsIDs(Main.Instance.UserInfo.UserID));
    }
    IEnumerator GetDate()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/UnityBackendTutorial/GetDate.php"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("www.error");
            }
            else
            {
                //Show results as text
                Debug.Log(www.downloadHandler.text);

                //or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
            }
        }
    }

    IEnumerator GetUsers()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/UnityBackendTutorial/GetUsers.php"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("www.error");
            }
            else
            {
                //Show results as text
                Debug.Log(www.downloadHandler.text);

                //or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
            }
        }
    }

    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/UnityBackendTutorial/Login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                Main.Instance.UserInfo.SetCredentials(username, password);
                Main.Instance.UserInfo.SetID(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator RegisterUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/UnityBackendTutorial/RegisterUser.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    IEnumerator GetItemsIDs(string userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/UnityBackendTutorial/GetItemsID.php", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("www.error");
            }
            else
            {
                //Show results as text
                Debug.Log(www.downloadHandler.text);
                string jsonArray = www.downloadHandler.text;

                //Call callback function to pass results
            }
        }
    }
}

