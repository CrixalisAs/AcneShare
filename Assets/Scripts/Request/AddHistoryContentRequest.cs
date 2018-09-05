using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Common;
using UnityEngine;

public class AddHistoryContentRequest : BaseRequest {

    private HistoryPanel historyPanel;
    private Queue<History> histories = new Queue<History>();
    DateTime dateTime = DateTime.MinValue;
    private bool isUpdate = false;

    public override void Awake()
    {
        historyPanel = GetComponent<HistoryPanel>();
        requestCode = RequestCode.Info;
        actionCode = ActionCode.AddHistoryContent;
        base.Awake();
    }


    void Update()
    {
        if (isUpdate)
        {
            isUpdate = false;
            historyPanel.ListHistoryItem(histories, dateTime);
            histories.Clear();
            dateTime = DateTime.MinValue;
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
                DateTime date = DateTime.Parse(s[1]);
                if (dateTime == DateTime.MinValue)
                {
                    dateTime = date;
                }
                byte[] image = s[2] == "" ? null : Tools.ParseBytes(s[2]);
                string content = s[3];
                histories.Enqueue(new History(id, date, content, image));
            }
        }
        if (dateTime == DateTime.MinValue)
        {
            dateTime = DateTime.Today;
        }
        isUpdate = true;
    }
}
