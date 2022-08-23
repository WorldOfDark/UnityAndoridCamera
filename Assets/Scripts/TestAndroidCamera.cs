/***********************************************
Copyright (C) 2018 The Company Name
File Name:           TestAndroidCamera.cs
Author:              拾忆丶夜
UnityVersion：       #UnityVersion#
CreateTime:          #CreateTime#
Description:         安卓相册调用测试
***********************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TestAndroidCamera : MonoBehaviour
{
    // 直接拖过来button和按钮上的image
    public Button btn;
    public RawImage ImageView;
    AndroidJavaObject jo;

    private void Awake()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        btn.onClick.AddListener(CallAndroid);
    }

    /// <summary>
    /// CALL>>>ANDROID>>>打开相册
    /// </summary>
    void CallAndroid()
    {
        jo.Call("startPhoto");
    }

    /// <summary>
    /// 给Android调用的
    /// </summary>
    /// <param name="str"></param>
    public void CallUnity(string str)
    {
        ShowImage(str);
        jo.Call("CallAndroid", string.Format("图片Address>>>>" + str));

        //string path = "file://"  + str;
        //StartCoroutine(LoadTexturePreview(path));
    }

    //使用www方式获取图片
    IEnumerator LoadTexturePreview(string path)
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path);
        yield return uwr.SendWebRequest();
        Debug.Log("图片地址:" + path);
        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log("错误" + uwr.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            ImageView.texture = texture;
        }
    }

    //使用文件流读取图片
    private void ShowImage(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        byte[] bye = new byte[fileStream.Length];
        fileStream.Read(bye, 0, (int)bye.Length);
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        Texture2D texture = new Texture2D(100, 50);
        texture.LoadImage(bye);
        //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //ImageView.sprite = sprite;

        FillScreenShotImage(ImageView, texture);
    }

    /// <summary>
    /// 保持图片原比例填充到rawimage中
    /// </summary>
    /// <param name="rawImage"></param>
    /// <param name="tex"></param>
    public void FillScreenShotImage(RawImage rawImage, Texture2D tex)
    {
        float texWidth = tex.width;
        float texHeight = tex.height;
        float imageheight = rawImage.rectTransform.rect.height;
        float imagewidth = texWidth / texHeight * imageheight;
        rawImage.rectTransform.sizeDelta = new Vector2(imagewidth, imageheight);
        rawImage.texture = tex;
    }
}

