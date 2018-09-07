using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : BasePanel
{
    private ScheduleAmountRequest scheduleAmountRequest;
    private TaskInfoRequest taskInfoRequest;
    private Text name;

    private Text age;

    private Text sex;

    private CircleImage image;
    private Text amount;

    private const string man = "♂";
    private const string women = "♀";
    private Color manColor=new Color(0,70f/255,1);
    private Color womenColor = new Color(1, 0, 87f/255);

    private Image fillImage;
    private Text taskText;
    private Text score;
    // Use this for initialization
    void Awake ()
    {
        taskInfoRequest = GetComponent<TaskInfoRequest>();
        scheduleAmountRequest = GetComponent<ScheduleAmountRequest>();
	    Transform info = transform.Find("Info");
	    Transform calendar = transform.Find("Calendar");
	    Transform task = transform.Find("Task");
        name = info.Find("Name").GetComponent<Text>();
	    age = info.Find("Age").GetComponent<Text>();
	    sex = info.Find("Sex").GetComponent<Text>();
	    image = info.Find("Image").GetComponent<CircleImage>();
        transform.Find("ShareButton").GetComponent<Button>().onClick.AddListener(OnShareButtonClick);
        transform.Find("KnowledgeButton").GetComponent<Button>().onClick.AddListener(Knowledge);
        amount = calendar.Find("Amount").GetComponent<Text>();
        calendar.Find("DetailsButton").GetComponent<Button>().onClick.AddListener(Calendar);
        task.Find("DetailsButton").GetComponent<Button>().onClick.AddListener(Task);
        transform.Find("History/DetailsButton").GetComponent<Button>().onClick.AddListener(History);
        taskText = transform.Find("Task/TaskText").GetComponent<Text>();
        fillImage = transform.Find("Task/FillImage").GetComponent<Image>();
        score = fillImage.transform.Find("Score").GetComponent<Text>();
    }

    void Start()
    {
        SetInfo();
        scheduleAmountRequest.SendRequest();
        taskInfoRequest.SendRequest();
    }
    void Calendar()
    {
        uiMng.PushPanel(UIPanelType.Calendar);
    }

    void History()
    {
        uiMng.PushPanel(UIPanelType.History);
    }
    void OnShareButtonClick()
    {
        uiMng.PopAndDestroy();
    }

    void Knowledge()
    {
        uiMng.PopPanel();
        uiMng.PushPanel(UIPanelType.Knowledge);
    }
    void Task()
    {
        TaskPanel taskPanel = uiMng.PushPanel(UIPanelType.Task) as TaskPanel;
        taskPanel.InfoPanel = this;
    }
    public override void OnResume()
    {
        base.OnResume();
        SetInfo();
        scheduleAmountRequest.SendRequest();
    }

    public void SetAmount(int amount)
    {
        this.amount.text = amount.ToString();
    }
    void SetInfo()
    {
        UserData ud = facade.GetUserData();
        name.text = ud.Name;
        age.text = ud.Age.ToString();
        sex.text = ud.Sex ? women : man;
        sex.color = ud.Sex ? womenColor : manColor;
        image.sprite = GameFacade.TransBytesToSprite(ud.Image);
    }

    public void SetTaskInfo(string score, string taskText, float fillAmount)
    {
        this.score.text = score;
        this.taskText.text = taskText;
        this.fillImage.fillAmount = fillAmount;
    }
}
