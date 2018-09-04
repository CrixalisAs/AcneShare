using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleItem : MonoBehaviour
{
    private int id;
    private Text time;
    private Text content;
    public CalendarPanel CalendarPanel;

	// Use this for initialization
	void Awake ()
	{
	    time = transform.Find("Time").GetComponent<Text>();
	    content = transform.Find("Content").GetComponent<Text>();
	    transform.Find("DeleteButton").GetComponent<Button>().onClick.AddListener(DeleteSchedule);
	}

    public ScheduleItem Set(int id,string time,string content)
    {
        this.id = id;
        this.time.text = time;
        this.content.text = content;
        return this;
    }

    void DeleteSchedule()
    {
        CalendarPanel.Delete(id);
    }
}
