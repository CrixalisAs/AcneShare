using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Common;
using UnityEngine;

public class TaskInfoRequest : BaseRequest {

    private InfoPanel infoPanel;
    private bool isUpdate = false;

    int unConfirmedCount = 0;
    int finishedCount = 0;
    int unfinishedCount = 0;
    public override void Awake()
    {
        infoPanel = GetComponent<InfoPanel>();
        requestCode = RequestCode.Info;
        actionCode = ActionCode.TaskInfo;
        base.Awake();
    }
    public override void SendRequest()
    {
        base.SendRequest("");
    }

    void Update()
    {
        if (isUpdate)
        {
            isUpdate = false;

            int totalCount = unConfirmedCount+finishedCount+unfinishedCount;
            if (totalCount == 0)
            {
                infoPanel.SetTaskInfo("0", "今日任务已完成(0/0)\n尚有0项未确认", 0);
            }
            else
            {
                int score = ((int) (finishedCount * 100f / totalCount));
                infoPanel.SetTaskInfo(score.ToString(),
                    "今日任务已完成(" + finishedCount + "/" + totalCount + ")\n尚有" + unConfirmedCount + "项未确认",
                    score * 1f / 100);
            }

            unConfirmedCount = 0;
            finishedCount = 0;
            unfinishedCount = 0;
        }
    }

    public override void OnResponse(string data)
    {
        base.OnResponse(data);
        Debug.Log("TaskInfoResponse");
        if (data != "")
        {
            string[] strs = data.Split('$');
            foreach (string str in strs)
            {
                string[] s = str.Split('&');
                TaskState state = (TaskState)Enum.Parse(typeof(TaskState), s[1]);
                switch (state)
                {
                    case TaskState.UnConfrimed:
                        unConfirmedCount++;
                        break;
                    case TaskState.Finished:
                        finishedCount++;
                        break;
                    case TaskState.UnFinished:
                        unfinishedCount++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        isUpdate = true;
    }
}
