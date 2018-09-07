using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArticlePanel : BasePanel
{
    private Text title;

    private Text content;

    private Text date;
	// Use this for initialization
	void Awake ()
	{
	    title = transform.Find("Title").GetComponent<Text>();
	    content = transform.Find("ScrollPanel/Layout/Content").GetComponent<Text>();
	    date = transform.Find("Date").GetComponent<Text>();
        transform.Find("BackButton").GetComponent<Button>().onClick.AddListener((() => uiMng.PopPanel()));
    }

    public void Set(string title,string content,DateTime date)
    {
        this.title.text = title;  
        this.content.text = content;
        this.date.text = date.ToString();
    }
}
