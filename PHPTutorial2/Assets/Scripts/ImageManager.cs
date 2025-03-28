using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImageManager : MonoBehaviour
{
    public static ImageManager Instance;
    string _basepath;

    void Start()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this);
            return;
        }

        Instance = this;
        _basepath = Application.persistentDataPath + "/Images/";

        if(!Directory.Exists(_basepath))
        {
            Directory.CreateDirectory(_basepath);
        }
    }

    bool ImageExists(string name)
    {
        return File.Exists(_basepath + name);
    }

    public void SaveImage(string name, byte[] bytes)
    {
        File.WriteAllBytes(_basepath + name, bytes);
    }

    public byte[] LoadImage(string name)
    {
        byte[] bytes = new byte[0];

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
}
