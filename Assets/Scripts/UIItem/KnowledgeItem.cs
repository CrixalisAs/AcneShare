using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

public class KnowledgeItem : MonoBehaviour
{
    private int id;

    private Text content;
    public string Content { get { return content.text; } }

    private Text title;
    public string Title { get { return title.text; } }

    private DateTime date;
    public DateTime Date { get { return date; } }

    private KnowledgePanel knowledgePanel;

    private Text isNew;
	// Use this for initialization
	void Awake ()
	{
	    isNew = transform.Find("New").GetComponent<Text>();
	    content = transform.Find("Content").GetComponent<Text>();
	    title = transform.Find("Title").GetComponent<Text>();
        GetComponent<Button>().onClick.AddListener((() => knowledgePanel.Article(this)));
	}

    public KnowledgeItem Set(Knowledge knowledge, KnowledgePanel knowledgePanel)
    {
        this.knowledgePanel = knowledgePanel;
        date = knowledge.Date;
        content.text = date+"\n"+knowledge.Content;
        title.text = knowledge.Title;
        if(knowledge.IsNew)
            isNew.gameObject.SetActive(true);
        return this;
    }
}
