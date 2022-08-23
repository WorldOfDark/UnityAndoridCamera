package com.self.androidunity;

import android.Manifest;
import android.annotation.SuppressLint;
import android.content.ContentUris;
import android.content.DialogInterface;
import android.content.Intent;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.provider.DocumentsContract;
import android.provider.MediaStore;

import android.app.Activity;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

public class MainActivity extends UnityPlayerActivity  {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        // 获取存储权限,不然的话无法获取图片
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            requestPermissions(new String[]{Manifest.permission.WRITE_EXTERNAL_STORAGE, Manifest.permission.READ_EXTERNAL_STORAGE, Manifest.permission.CAMERA}, 100);
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode == RESULT_OK)
        {
            switch(requestCode)
            {
                case 1:
                    GetCameraPhoto(data);
                    break;
                case 2:
                    UnityPlayer.UnitySendMessage("UnityPlugin","OnGetPhotoPath", GetPath(data));
                    break;                
            }
        }
    }

    public void OpenCamera()
    {
        Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        startActivityForResult(intent,1);
    }

    public void OpenPhotograph()
    {
        Intent intent = new Intent(Intent.ACTION_PICK);
        intent.setType("image/*");
        startActivityForResult(intent, 2);
    }

    public String GetPath(Intent data) {
        Uri uri = data.getData();
        String imagePath;
        // 第二个参数是想要获取的数据
        Cursor cursor = getContentResolver()
                .query(uri, new String[]{MediaStore.Images.ImageColumns.DATA},
                        null, null, null);
        if (cursor == null) {
            imagePath = uri.getPath();
        } else {
            cursor.moveToFirst();
            // 获取数据所在的列下标
            int index = cursor.getColumnIndex(MediaStore.Images.ImageColumns.DATA);
            imagePath = cursor.getString(index);  // 获取指定列的数据
            cursor.close();
        }
        return imagePath;  // 返回图片地址
    }

    public void GetCameraPhoto(Intent data)
    {
        Bitmap bit = (Bitmap)data.getExtras().get("data");
        File f = new File(getExternalCacheDir(),"head.png");
        if (f.exists()) {
            f.delete();
        }
        try {
            FileOutputStream outputStream = new FileOutputStream(f);
            bit.compress(Bitmap.CompressFormat.PNG, 100, outputStream);
            outputStream.flush();
            outputStream.close();
            //向Unity传递路径
            UnityPlayer.UnitySendMessage("UnityPlugin","OnGetPhotoPath", f.getPath());
        } catch (FileNotFoundException var8) {
            var8.printStackTrace();
        } catch (IOException var9) {
            var9.printStackTrace();
        }
    }
}