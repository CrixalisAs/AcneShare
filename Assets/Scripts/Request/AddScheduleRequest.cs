using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Common;
using UnityEngine;

public class AddScheduleRequest : BaseRequest {

    private CalendarPanel calendarPanel;
    private List<Schedule> schedules = new List<Schedule>();
    private bool isUpdate = false;

    public override void Awake()
    {
        calendarPanel = GetComponent<CalendarPanel>();
        requestCode = RequestCode.Info;
        actionCode = ActionCode.AddSchedule;
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
    public override void OnResponse(string data)
    {
        base.OnResponse(data);
        if (data == "") return;
        int index = data.IndexOf('$');
        int noticeId = int.Parse(data.Split('$')[0]);
        data = data.Remove(0, index);
        string[] strs = data.Split('$');
        foreach (string s in strs)
        {
            string[] str = s.Split('&');
            int id = int.Parse(str[0]);
            string time = str[1];
            byte isNotice = byte.Parse(str[2]);
            string content = str[3];
            Schedule schedule = new Schedule(id, time, isNotice, content);
            schedules.Add(schedule);
            if (id == noticeId)
            {
                DateTime now = new DateTime();
                now = DateTime.Now;
                DateTime date=new DateTime(schedule.Year,schedule.Month,schedule.Day,schedule.HourStart,schedule.MinuteStart,0);
                TimeSpan deltaTime = date - now;
                int delayMinutes = (int) deltaTime.TotalMinutes;
                NativeToolkit.ScheduleLocalNotification("日程提醒 "+schedule.HourStart+":"+schedule.MinuteStart+"--"+
                    schedule.HourEnd+":"+schedule.MinuteEnd, content, 1, delayMinutes, "sound_notification", true, "ic_notification", "ic_notification_large");

            }
        }
        if (schedules.Count != 0)
            isUpdate = true;

    }
}
