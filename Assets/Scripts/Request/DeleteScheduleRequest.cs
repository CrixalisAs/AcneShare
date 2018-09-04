using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Common;
using UnityEngine;

public class DeleteScheduleRequest : BaseRequest {

    private CalendarPanel calendarPanel;
    private List<Schedule> schedules = new List<Schedule>();
    private bool isUpdate = false;

    public override void Awake()
    {
        calendarPanel = GetComponent<CalendarPanel>();
        requestCode = RequestCode.Info;
        actionCode = ActionCode.DeleteSchedule;
        base.Awake();
    }


    void Update()
    {
        if (isUpdate)
        {
            isUpdate = false;
            calendarPanel.UpdateSchedules(schedules);
            schedules.Clear();
        }
    }

    public void SendRequest(int id)
    {
        base.SendRequest(id.ToString());
        NativeToolkit.ClearLocalNotification(id);
    }

    public override void OnResponse(string data)
    {
        base.OnResponse(data);
        if (data == "") return;
        string[] strs = data.Split('$');
        foreach (string s in strs)
        {
            string[] str = s.Split('&');
            int id = int.Parse(str[0]);
            string time = str[1];
            byte isNotice = byte.Parse(str[2]);
            string content = str[3];
            schedules.Add(new Schedule(id, time, isNotice, content));
        }
        if (schedules.Count != 0)
            isUpdate = true;
    }
}
