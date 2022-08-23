/***********************************************
Copyright (C) 2018 The Company Name
File Name:           OpenCameraAndSaveImage.cs
Author:              拾忆丶夜
UnityVersion：       #UnityVersion#
CreateTime:          #CreateTime#
Description:         拍摄照片和相册保存
***********************************************/

using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class OpenCameraAndSaveImage : MonoBehaviour
{
    // UI 相关参数
    public RawImage rawImage;

    public Button 拍照;

    public Button 相册;

    public Text pathText;

    private void Start()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        AndroidJavaObject jcontext = jc.GetStatic<AndroidJavaObject>("currentActivity");

        拍照.onClick.AddListener(() =>
        {
            jcontext.Call("OpenCamera");
        });

        相册.onClick.AddListener(() =>
        {
            jcontext.Call("OpenPhotograph");
        });
    }

    public void OnGetPhotoPath(string path)
    {
        pathText.text = path;

        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        byte[] bye = new byte[fileStream.Length];
        fileStream.Read(bye, 0, (int)bye.Length);
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        Texture2D texture = new Texture2D(100, 50);
        texture.LoadImage(bye);

        FillScreenShotImage(rawImage,texture);
    }

    /// <summary>
    /// 保持图片原比例填充到rawimage中
    /// </summary>
    /// <param name="rawImage"></param>
    /// <param name="tex"></param>
    public void FillScreenShotImage(RawImage rawImage, Texture tex)
    {
        float texWidth = tex.width;
        float texHeight = tex.height;
        float imageheight = rawImage.rectTransform.rect.height;
        float imagewidth = texWidth / texHeight * imageheight;
        rawImage.rectTransform.sizeDelta = new Vector2(imagewidth, imageheight);
        rawImage.texture = tex;
    }
}
