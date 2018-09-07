using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class KnowledgePanel : BasePanel
{
    private Transform layout;
    private Transform classifyPanel;
    private GameObject knowledgeItem;
    private List<KnowledgeItem> knowledgeItems=new List<KnowledgeItem>();
    List<Knowledge> knowledges=new List<Knowledge>();
    private UpdateKnowledgeRequest updateKnowledgeRequest;

    private AddKnowledgeRequest addKnowledgeRequest;

    private float targetPosx = 0;

    public float smoothing = 4;

    private float showPosx=0;

    private float hidePosx;
	// Use this for initialization
	void Awake ()
	{
	    updateKnowledgeRequest = GetComponent<UpdateKnowledgeRequest>();
#if UNITY_EDITOR
	    addKnowledgeRequest = gameObject.AddComponent<AddKnowledgeRequest>();
#endif
        knowledgeItem = Resources.Load<GameObject>("UIItem/KnowledgeItem");
	    layout = transform.Find("ScrollPanel/Layout");
	    classifyPanel = transform.Find("ClassifyPanel");
	    hidePosx = -classifyPanel.GetComponent<RectTransform>().rect.width;
	    targetPosx = hidePosx;
        for (int i = 0; i < Enum.GetNames((new KnowledgeType()).GetType()).Length; i++)
	    {
	        classifyPanel.Find("Button" +i).GetComponent<Button>().onClick.AddListener(() => ListKnowledges(knowledges,(KnowledgeType)i));
        }
	    transform.Find("SelectPanel/InputField").GetComponent<InputField>().onValueChanged.AddListener(Select);
        transform.Find("ShareButton").GetComponent<Button>().onClick.AddListener(OnShareButtonClick);
	    transform.Find("InfoButton").GetComponent<Button>().onClick.AddListener(OnInfoButtonClick);
        transform.Find("ClassifyButton").GetComponent<Button>().onClick.AddListener(OnClassifyButtonClick);
    }

    void Start()
    {
        updateKnowledgeRequest.SendRequest("");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            addKnowledgeRequest.SendRequest();
        }
        classifyPanel.position=Vector3.Lerp(classifyPanel.position,new Vector3(targetPosx,classifyPanel.position.y, classifyPanel.position.z),smoothing*Time.deltaTime );
    }

    public void Article(KnowledgeItem item)
    {
        (uiMng.PushPanel(UIPanelType.Article) as ArticlePanel).Set(item.Title,item.Content,item.Date);
    }
    void OnClassifyButtonClick()
    {
        if (targetPosx == hidePosx)
        {
            targetPosx = showPosx;
        }
        else
        {
            targetPosx = hidePosx;
        }
    }
    void OnShareButtonClick()
    {
        uiMng.PopAndDestroy();
    }

    void OnInfoButtonClick()
    {
        uiMng.PopPanel();
        uiMng.PushPanel(UIPanelType.Info);
    }
    void Select(string param)
    {
        if (string.IsNullOrEmpty(param))
        {
            ListKnowledges(knowledges);
            return;
        }
        string[] datas = new string[knowledgeItems.Count];
        for (int i = 0; i < datas.Length; i++)
        {
            datas[i] = knowledgeItems[i].Title;
        }
        datas = Tools.Search(param, datas);
        for (int i = 0; i < datas.Length; i++)
        {
            int index = knowledgeItems.FindIndex(item => item.Title == datas[i]);
            knowledgeItems[index].transform.SetSiblingIndex(i);
        }
    }
    public void ListKnowledges(List<Knowledge> knowledges,KnowledgeType type=KnowledgeType.All)
    {
        for (int i = 0; i < layout.childCount; i++)
        {
            Destroy(layout.GetChild(i).gameObject);
        }
        knowledgeItems.Clear();
        this.knowledges = new List<Knowledge>(knowledges);
        foreach (Knowledge knowledge in knowledges)
        {
            if (type != KnowledgeType.All&&knowledge.Type==type)
            {
                knowledgeItems.Add(Instantiate(knowledgeItem, layout).GetComponent<KnowledgeItem>().Set(knowledge, this));
            }
            else
            {
                knowledgeItems.Add(Instantiate(knowledgeItem, layout).GetComponent<KnowledgeItem>().Set(knowledge, this));
            }
        }
    }
}
