using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Model;
using UI.Dates;
using UnityEngine;
using UnityEngine.UI;

public class CalendarPanel : BasePanel
{
    private AddScheduleRequest addScheduleRequest;
    private UpdateScheduleRequest updateScheduleRequest;
    private DeleteScheduleRequest deleteScheduleRequest;
    private DatePicker datePicker;
    private GameObject scheduleItem;
    private Transform layout;
    private GameObject addPanel;
    private Text dateText;
    InputField[] DateRange=new InputField[4];
    private Toggle noticeToggle;
    private InputField content;
	// Use this for initialization
	void Awake ()
	{
	    layout = transform.Find("ScrollPanel/Layout");
	    scheduleItem = Resources.Load<GameObject>("UIItem/ScheduleItem");
	    addScheduleRequest = GetComponent<AddScheduleRequest>();
	    updateScheduleRequest = GetComponent<UpdateScheduleRequest>();
	    deleteScheduleRequest = GetComponent<DeleteScheduleRequest>();
	    datePicker = transform.Find("Calendar").GetComponent<DatePicker>();
        transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(Back);
	    transform.Find("AddButton").GetComponent<Button>().onClick.AddListener(Add);
	    addPanel = transform.Find("AddPanel").gameObject;
	    dateText = addPanel.transform.Find("Date").GetComponent<Text>();
        addPanel.transform.Find("SaveButton").GetComponent<Button>().onClick.AddListener(Save);
	    content = addPanel.transform.Find("Content").GetComponent<InputField>();
	    noticeToggle = addPanel.transform.Find("NoticeToggle").GetComponent<Toggle>();
        for (int i = 0; i < 4; i++)
	    {
	        DateRange[i] = addPanel.transform.Find("DateRange/InputField" + i).GetComponent<InputField>();
	        var j = i;
	        DateRange[i].onEndEdit.AddListener(x=> DateRange[j].text=(int.Parse(x)%(j%2==0?24:60)).ToString());
	    }
	    addPanel.SetActive(false);
        Debug.Log(datePicker.VisibleDate);
    }

    void Start()
    {
        facade.SetMessagePos(-848);
        updateScheduleRequest.SendRequest("");
    }
    public void OnDaySelected()
    {
        Debug.Log(datePicker.SelectedDate);
        dateText.text = datePicker.VisibleDate.ToString().Split(' ')[0];
    }

    void Back()
    {
        uiMng.PopAndDestroy();
    }

    void Add()
    {
        addPanel.SetActive(true);
        dateText.text = datePicker.VisibleDate.ToString().Split(' ')[0];
    }

    public void UpdateSchedules(List<Schedule> schedules)
    {
        for (int i = 0; i < layout.childCount; i++)
        {
            Destroy(layout.GetChild(i).gameObject);
        }
        foreach (Schedule schedule in schedules)
        {
            string time = schedule.Year+"年"+schedule.Month+"月"+schedule.Day+"日 " + schedule.HourStart + ":" + schedule.MinuteStart + " 到 " + schedule.HourEnd + ":" +
                          schedule.MinuteEnd;
            Instantiate(scheduleItem, layout).GetComponent<ScheduleItem>().Set(schedule.Id,time, schedule.Content).CalendarPanel =
                this;
        }
    }

    public void Delete(int id)
    {
        deleteScheduleRequest.SendRequest(id);
    }
    void Save()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(dateText.text);

        int hourEnd = int.Parse(DateRange[2].text);
        int hourStart = int.Parse(DateRange[0].text);
        int minuteEnd= int.Parse(DateRange[3].text);
        int minuteStart = int.Parse(DateRange[1].text);
        if (hourEnd < hourStart)
        {
            facade.ShowMessage("时间错误");
            return;
        }
        else if (hourEnd == hourStart)
        {
            if (minuteEnd <= minuteStart)
            {
                facade.ShowMessage("时间错误");
                return;
            }
        }
        for (int i = 0; i < DateRange.Length; i++)
        {
            string time = DateRange[i].text;
            if (string.IsNullOrEmpty(time))
            {
                facade.ShowMessage("时间为空");
                return;
            }
            sb.Append('|');
            sb.Append(time);
        }
        sb.Append('$');
        sb.Append(noticeToggle.isOn ? 1 : 0);
        sb.Append('$');
        if (string.IsNullOrEmpty(content.text))
        {
            facade.ShowMessage("内容为空");
            return;
        }
        sb.Append(content.text);
        addScheduleRequest.SendRequest(sb.ToString());
        ClearAddPanel();
    }

    void ClearAddPanel()
    {
        foreach (InputField inputField in DateRange)
        {
            inputField.text = "";
        }
        content.text = "";
        addPanel.SetActive(false);
    }
}
