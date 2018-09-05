using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HistoryPanel : BasePanel, IBeginDragHandler, IEndDragHandler
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
    // Use this for initialization
    void Awake ()
    {
        updateHistoryRequest = GetComponent<UpdateHistoryRequest>();
        addHistoryContentRequest = GetComponent<AddHistoryContentRequest>();
        addHistoryPhotoRequest = GetComponent<AddHistoryPhotoRequest>();
        NullSprite = Resources.Load<Sprite>("Sprites/Null");
        layout = transform.Find("ScrollPanel/Layout");
        historyItem = Resources.Load<GameObject>("UIItem/HistoryItem");
		transform.Find("BackButton").GetComponent<Button>().onClick.AddListener((() => uiMng.PopAndDestroy()));
        scrollRect = transform.Find("ScrollPanel").GetComponent<ScrollRect>();
        dateText = transform.Find("DateText").GetComponent<Text>();
        contentText = transform.Find("ContentText").GetComponent<InputField>();
        contentText.onEndEdit.AddListener(SaveContent);
    }

    void Start()
    {
        updateHistoryRequest.SendRequest("");
        targetHorizontalPosition = 1;
    }
	// Update is called once per frame
	void Update ()
	{
	    contentText.readOnly = targetHorizontalPosition != 1;
        if (isDraging == false)
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition,
                targetHorizontalPosition, Time.deltaTime * smoothing);
    }

    void SaveContent(string content)
    {
        if (targetHorizontalPosition != 1) return;
        addHistoryContentRequest.SendRequest(content);
    }
    public void ListHistoryItem(Queue<History> histories,DateTime firstDay)
    {
        DateTime now=DateTime.Today;
        DateTime currentDay = firstDay;
        TimeSpan deltaTime = now - firstDay;
        int dayCount = deltaTime.Days + 1;
        pageArray=new float[dayCount];
        float deltaOffset = 1f / (dayCount - 1);
        for (int i = 0; i < dayCount; i++,currentDay+= new TimeSpan(1, 0, 0, 0))
        {
            if (histories.Count!=0&&histories.Peek().Date == currentDay)
            {
                Instantiate(this.historyItem, layout).GetComponent<HistoryItem>().Set(histories.Dequeue(), this);
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
        }
    }

    public void SavePhoto(string data)
    {
        addHistoryPhotoRequest.SendRequest(data);
    }
}
