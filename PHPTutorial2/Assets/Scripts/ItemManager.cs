using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using SimpleJSON;

public class ItemManager : MonoBehaviour
{
    Action<string> _createItemsCallback;
    void Start()
    {
        _createItemsCallback = (jsonArrayString) => {
            StartCoroutine(CreateItemsRoutine(jsonArrayString));
        };

        CreateItems();
    }

    public void CreateItems()
    {
        string userId = Main.Instance.UserInfo.UserID;
        StartCoroutine(Main.Instance.Web.GetItemsIDs(userId, _createItemsCallback));
    }

    IEnumerator CreateItemsRoutine(string jsonArrayString)
    {
        //parsing json array string as an array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //Create local variables
            bool isDone = false; //are we done downloading
            string itemId = jsonArray[i].AsObject["itemID"];
            string id = jsonArray[i].AsObject["ID"];

            JSONObject itemInfoJson = new JSONObject();

            //Create a callback to get the information from Web.cs
            Action<string> getItenInfoCallback = (itemInfo) => {
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
            };

            StartCoroutine(Main.Instance.Web.GetItem(itemId, getItenInfoCallback));

            //Wait until the callback is called from WEB (info finsihed downloading)
            yield return new WaitUntil(() => isDone == true);

            //Instantiate Gameobject item prefab
            GameObject itemGo = Instantiate(Resources.Load("Prefabs/Item") as GameObject);
            Item item = itemGo.AddComponent<Item>();

            item.ID = id;
            item.ItemID = itemId;

            itemGo.transform.SetParent(this.transform);
            itemGo.transform.localScale = Vector3.one;
            itemGo.transform.localPosition = Vector3.zero;

            //Fill Information
            itemGo.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["name"];
            itemGo.transform.Find("Price").GetComponent<Text>().text = itemInfoJson["price"];
            itemGo.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"];

            int ImgVer = itemInfoJson["ImgVer"].AsInt;

            byte[] bytes = ImageManager.Instance.LoadImage(itemId, ImgVer);

            if (bytes.Length == 0)
            {
                //Create a callback to get the SPRITE from Web.cs
                Action<byte[]> getItemIconCallback = (downloadbyte) => {
                    Sprite sprite = ImageManager.Instance.BytestoSprite(downloadbyte);
                    itemGo.transform.Find("Image").GetComponent<Image>().sprite = sprite;
                    ImageManager.Instance.SaveImage(itemId, downloadbyte, ImgVer);
                    ImageManager.Instance.SaveVersionJSON();
                };
                StartCoroutine(Main.Instance.Web.GetItemIcon(itemId, getItemIconCallback));
            }
            else
            {
                Sprite sprite = ImageManager.Instance.BytestoSprite(bytes);
                itemGo.transform.Find("Image").GetComponent<Image>().sprite = sprite;
            }

            //Set sell button
            itemGo.transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(() => {
                string idInventory = id;
                string iId = itemId;
                string userId = Main.Instance.UserInfo.UserID;
                StartCoroutine(Main.Instance.Web.SellItem(idInventory, itemId, userId));
            });
            //continue to the next item

        }
    }
}
