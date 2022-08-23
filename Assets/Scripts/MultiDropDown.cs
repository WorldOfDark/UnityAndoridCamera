/***********************************************
Copyright (C) 2018 The Company Name
File Name:           MultiDropDown.cs
Author:              拾忆丶夜
UnityVersion：       #UnityVersion#
CreateTime:          #CreateTime#
Description:         多选下拉框
***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiDropDown : MonoBehaviour
{
    public Text valueText;

    public Image btnImage;

    public RectTransform dropdownScrollView;

    public RectTransform content;

    public GameObject togglePerfab;

    private Toggle selfToggle;

    private List<Toggle> selectedList = new List<Toggle>();

    private RectTransform canvasRect;

    void Start()
    {
        canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();

        selfToggle = GetComponent<Toggle>();

        string[] test = new string[] { "test1", "test2", "test3", "test4" };

        InitScrollView(test);

        selfToggle.onValueChanged.AddListener((ison) => 
        {
            btnImage.rectTransform.localEulerAngles = new Vector3(0, 0, ison ? 90 : 45);

            if (ison)
            {
                ShowScrollViewOnTopLayer();
            }
            else
            {
                dropdownScrollView.gameObject.SetActive(false);
            }
        });
    }

    public void InitScrollView(string[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            GameObject item = Instantiate(togglePerfab, content);

            item.SetActive(true);

            Toggle toggle = item.GetComponent<Toggle>();

            Text nameText = item.GetComponentInChildren<Text>();

            nameText.text = values[i];

            string nowValue = values[i];

            toggle.onValueChanged.AddListener((ison) =>
            {
                if (ison)
                {
                    selectedList.Add(toggle);
                }
                else
                {
                    selectedList.Remove(toggle);
                }

                UpdateValueText();
            });
        }
    }

    private void UpdateValueText()
    {
        valueText.text = "";

        for (int i = 0; i < selectedList.Count; i++)
        {
            string a = selectedList[i].GetComponentInChildren<Text>().text;

            valueText.text += (valueText.text != "" ? "," : "") + a;
        }
    }

    private void ShowScrollViewOnTopLayer()
    {
        dropdownScrollView.SetParent(canvasRect);

        dropdownScrollView.SetAsLastSibling();

        dropdownScrollView.position = transform.position;

        dropdownScrollView.gameObject.SetActive(true);
    }
}
