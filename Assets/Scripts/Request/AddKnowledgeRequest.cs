using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Model;
using Common;
using LitJson;
using UnityEngine;

public class AddKnowledgeRequest : BaseRequest {

    private KnowledgePanel knowledgePanel;
    private List<Knowledge> knowledges = new List<Knowledge>();
    private bool isUpdate = false;

    public override void Awake()
    {
        knowledgePanel = GetComponent<KnowledgePanel>();
        requestCode = RequestCode.Knowledge;
        actionCode = ActionCode.AddKnowledge;
        base.Awake();
    }


    void Update()
    {
        if (isUpdate)
        {
            isUpdate = false;
            knowledgePanel.ListKnowledges(knowledges);
            knowledges.Clear();
        }
    }

    public override void SendRequest()
    {
        base.SendRequest(ParseKnowledgeJson());
    }

    string ParseKnowledgeJson()
    {
        StringBuilder sb=new StringBuilder();
        TextAsset itemText = Resources.Load<TextAsset>("Json/Knowledges");
        JsonData itemsData = JsonMapper.ToObject(itemText.text);
        foreach (JsonData itemData in itemsData)
        {
            string title = itemData["title"].ToString();
            string content = itemData["content"].ToString();
            KnowledgeType type = (KnowledgeType)Enum.Parse(typeof(KnowledgeType), itemData["type"].ToString());
            sb.Append(title);
            sb.Append('&');
            sb.Append(content);
            sb.Append('&');
            sb.Append(type);
            sb.Append('$');
        }
        if(!string.IsNullOrEmpty(sb.ToString()))
            sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }
    public override void OnResponse(string data)
    {
        base.OnResponse(data);
        if (data != "")
        {
            string[] strs = data.Split('$');
            foreach (string str in strs)
            {
                string[] s = str.Split('&');
                int id = int.Parse(s[0]);
                string title = s[1];
                DateTime date = DateTime.Parse(s[2]);
                string content = s[3];
                bool isNew = s[4] == "1";
                KnowledgeType type = (KnowledgeType) Enum.Parse(typeof(KnowledgeType), s[5]);
                knowledges.Add(new Knowledge(id, title, date, content, isNew,type));
            }
        }
        isUpdate = true;
    }
}
