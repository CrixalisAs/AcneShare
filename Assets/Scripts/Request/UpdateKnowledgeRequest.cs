using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Common;
using UnityEngine;

public class UpdateKnowledgeRequest : BaseRequest {

    private KnowledgePanel knowledgePanel;
    private List<Knowledge> knowledges = new List<Knowledge>();
    private bool isUpdate = false;

    public override void Awake()
    {
        knowledgePanel = GetComponent<KnowledgePanel>();
        requestCode = RequestCode.Knowledge;
        actionCode = ActionCode.UpdateKnowledge;
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
                DateTime date=DateTime.Parse(s[2]);
                string content = s[3];
                bool isNew = s[4] == "1";
                KnowledgeType type = (KnowledgeType)Enum.Parse(typeof(KnowledgeType), s[5]);
                knowledges.Add(new Knowledge(id, title, date, content, isNew, type));
            }
        }
        isUpdate = true;
    }
}
