﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Assets.Scripts.Model;
using HedgehogTeam.EasyTouch;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HistoryPanel : BasePanel
{
    private AddHistoryPhotoRequest addHistoryPhotoRequest;
    private AddHistoryContentRequest addHistoryContentRequest;
    private UpdateHistoryRequest updateHistoryRequest;
    private ScrollRect scrollRect;
    private Transform layout;
    public float smoothing = 4;
    private float[] pageArray;
    private float targetHorizontalPosition = 0;
    private bool isDraging = false;

    private GameObject historyItem;
    public Sprite NullSprite;

    private Text dateText;
    private InputField contentText;

    private Text loadText;

    private GameObject loadPanel;

    private string originLoadText;

    private float timer = 0;
    // Use this for initialization
    void Awake ()
    {
        updateHistoryRequest = GetComponent<UpdateHistoryRequest>();
        addHistoryContentRequest = GetComponent<AddHistoryContentRequest>();
        addHistoryPhotoRequest = GetComponent<AddHistoryPhotoRequest>();
        loadPanel = transform.Find("LoadPanel").gameObject;
        loadText = loadPanel.transform.Find("LoadText").GetComponent<Text>();
        originLoadText = loadText.text;
        NullSprite = Resources.Load<Sprite>("Sprites/Null");
        layout = transform.Find("ScrollPanel/Layout");
        historyItem = Resources.Load<GameObject>("UIItem/HistoryItem");
		transform.Find("BackButton").GetComponent<Button>().onClick.AddListener((() => uiMng.PopPanel()));
        scrollRect = transform.Find("ScrollPanel").GetComponent<ScrollRect>();
        dateText = transform.Find("DateText").GetComponent<Text>();
        contentText = transform.Find("ContentText").GetComponent<InputField>();
        contentText.onEndEdit.AddListener(SaveContent);
    }

    void Start()
    {
        updateHistoryRequest.SendRequest("");
        targetHorizontalPosition = 1;
        dateText.text = DateTime.Today.Year + "年" + DateTime.Today.Date.Month + "月" + DateTime.Today.Date.Day + "日";
    }
	// Update is called once per frame
	void Update ()
	{
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
            SavePhoto(Tools.ReadPNG("E:/5.png"));
#endif
        contentText.readOnly = targetHorizontalPosition != 1;
        if (isDraging == false)
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition,
                targetHorizontalPosition, Time.deltaTime * smoothing);
	    if (loadPanel.activeSelf)
	    {
	        timer += Time.deltaTime;

            if (loadText.text == originLoadText)
	        {
	            loadText.text = "加载中";
	        }
	        if (timer >= 0.3f)
	        {
	            timer = 0;
	            loadText.text += ".";
	        }
	    }
    }

    void SaveContent(string content)
    {
        if (targetHorizontalPosition != 1) return;
        addHistoryContentRequest.SendRequest(content);
        layout.GetChild(layout.childCount - 1).GetComponent<HistoryItem>().Content = content;
    }
    public void ListHistoryItem(Queue<History> histories,DateTime firstDay)
    {
        DateTime now=DateTime.Now;
        DateTime currentDay = firstDay;
        TimeSpan deltaTime = now - firstDay;
        int dayCount = deltaTime.Days + 1;
        pageArray=new float[dayCount];
        float deltaOffset = 1f / (dayCount - 1);
        for (int i = 0; i < dayCount; i++,currentDay+= new TimeSpan(1, 0, 0, 0))
        {
            if (histories.Count!=0&&histories.Peek().Date.ToString("D") == currentDay.ToString("D"))
            {
                HistoryItem history = Instantiate(this.historyItem, layout).GetComponent<HistoryItem>().Set(histories.Dequeue(), this);
                if (i == dayCount - 1)
                {
                    contentText.text = history.Content;
                }
            }
            else
            {
                Instantiate(this.historyItem, layout).GetComponent<HistoryItem>().Set(new History(-1, currentDay, "", null), this);
            }
            pageArray[i] = i * deltaOffset;
            if (i == dayCount - 1)
            {
                pageArray[i] = 1;
            }
        }
        Loaded();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDraging = false;
        float posX = scrollRect.horizontalNormalizedPosition;
        int index = 0;
        float offset = Mathf.Abs(pageArray[index] - posX);
        for (int i = 1; i < pageArray.Length; i++)
        {
            float offsetTemp = Mathf.Abs(pageArray[i] - posX);
            if (offsetTemp < offset)
            {
                index = i;
                offset = offsetTemp;
            }
        }
        targetHorizontalPosition = pageArray[index];
        HistoryItem historyItem = layout.GetChild(index).GetComponent<HistoryItem>();
        if (historyItem != null)
        {
            contentText.text = historyItem.Content;
            dateText.text = historyItem.Date;
        }
    }

    public void SavePhoto(byte[] data) 
    {
        addHistoryPhotoRequest.SendRequest(Tools.PackBytes(data));
    }

    public override void OnResume()
    {
        base.OnResume();

        updateHistoryRequest.SendRequest("");
        loadPanel.SetActive(true);
        targetHorizontalPosition = 1;
    }

    public void OnDrag(PointerEventData eventData)
    { 
        isDraging = true;
    }

    private void Loaded()
    {
        loadPanel.SetActive(false);
    }
}
