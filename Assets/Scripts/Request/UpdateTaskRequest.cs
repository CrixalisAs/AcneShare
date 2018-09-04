using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Common;
using UnityEngine;

public class UpdateTaskRequest : BaseRequest {

    private TaskPanel taskPanel;
    private List<Task> tasks = new List<Task>();
    private bool isUpdate = false;

    public override void Awake()
    {
        taskPanel = GetComponent<TaskPanel>();
        requestCode = RequestCode.Info;
        actionCode = ActionCode.UpdateTask;
        base.Awake();
    }


    void Update()
    {
        if (isUpdate)
        {
            isUpdate = false;
            taskPanel.UpdateTasks(tasks);
            tasks.Clear();
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
                TaskState state = (TaskState)Enum.Parse(typeof(TaskState), s[1]);
                string content = s[2];
                tasks.Add(new Task(id, content, state));
            }
        }
        isUpdate = true;
    }
}
