using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public class ImageManager : MonoBehaviour
{
    public static ImageManager Instance;
    string _basepath;
    string _versionsJSONPath;
    JSONObject _versionJSON;

    void Start()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this);
            return;
        }

        Instance = this;
        _basepath = Application.persistentDataPath + "/Images/";

        if (!Directory.Exists(_basepath))
        {
            Directory.CreateDirectory(_basepath);
        }

        _versionJSON = new JSONObject();
        _versionsJSONPath = _basepath + "VersionJSON";

        if(!File.Exists(_versionsJSONPath))
        {
            string jsonString = File.ReadAllText(_versionsJSONPath);
            _versionJSON = JSON.Parse(jsonString) as JSONObject;
        }
    }

    bool ImageExists(string name)
    {
        return File.Exists(_basepath + name);
    }

    public void SaveImage(string name, byte[] bytes, int ImgVer)
    {
        File.WriteAllBytes(_basepath + name, bytes);
        UpdateVersionJSON(name, ImgVer);
    }

    public byte[] LoadImage(string name, int ImgVer)
    {
        byte[] bytes = new byte[0];

        if(!IsImageUpToDAte(name, ImgVer))
        {
            return bytes;
        }

        if (ImageExists(name))
        {
            bytes = File.ReadAllBytes(_basepath + name);
        }
        return bytes;
    }

    public Sprite BytestoSprite(byte[] bytes)
    {
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    void UpdateVersionJSON(string name, int ver)
    {
        _versionJSON[name] = ver;
    }

    bool IsImageUpToDAte(string name, int ver)
    {
        if(_versionJSON[name] != null)
        {
            return _versionJSON[name].AsInt == ver;
        }
        return false;
    }

    public void SaveVersionJSON()
    {
        File.WriteAllText(_versionsJSONPath, _versionJSON.ToString());
    }
}
