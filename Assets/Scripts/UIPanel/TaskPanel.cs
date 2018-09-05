using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : BasePanel
{
    public InfoPanel InfoPanel;
    private AddTaskRequest addTaskRequest;
    private UpdateTaskRequest updateTaskRequest;
    private DeleteTaskRequest deleteTaskRequest;
    private ConfirmTaskRequest confirmTaskRequest;
    private GameObject addButton;
    private Text EditButtonText;
    private List<TaskItem> taskItems=new List<TaskItem>();
    private Transform layout;
    private GameObject confirmPanel;
    private GameObject addPanel;
    private int currentId = -1;
    private InputField content;

    private Image fillImage;
    private Text taskText;
    private Text score;

    public Sprite Finished;
    public Sprite UnFinished;
    public Sprite UnConfirmed;

    void Awake()
    {
        Finished = Resources.Load<Sprite>("Sprites/Finished");
        UnFinished = Resources.Load<Sprite>("Sprites/UnFinished");
        UnConfirmed = Resources.Load<Sprite>("Sprites/UnConfirmed");
        addPanel = transform.Find("AddPanel").gameObject;
        addPanel.SetActive(false);
        confirmPanel = transform.Find("ConfirmPanel").gameObject;
        confirmPanel.SetActive(false);
        layout = transform.Find("ScrollPanel/Layout");
        confirmTaskRequest = GetComponent<ConfirmTaskRequest>();
        addTaskRequest = GetComponent<AddTaskRequest>();
        updateTaskRequest = GetComponent<UpdateTaskRequest>();
        deleteTaskRequest = GetComponent<DeleteTaskRequest>();
        transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(Back);
        transform.Find("EditButton").GetComponent<Button>().onClick.AddListener(Edit);
        EditButtonText = transform.Find("EditButton").GetComponent<Text>();
        addButton = transform.Find("AddButton").gameObject;
        addButton.GetComponent<Button>().onClick.AddListener(ShowAddPanel);
        transform.Find("ConfirmPanel/Panel/YesButton").GetComponent<Button>().onClick.AddListener(FinishTask);
        transform.Find("ConfirmPanel/Panel/NoButton").GetComponent<Button>().onClick.AddListener(UnFinishTask);
        addPanel.transform.Find("Panel/SaveButton").GetComponent<Button>().onClick.AddListener(Add);
        content = addPanel.transform.Find("Panel/Content").GetComponent<InputField>();
        taskText = transform.Find("Task/TaskText").GetComponent<Text>();
        fillImage= transform.Find("Task/FillImage").GetComponent<Image>();
        score = fillImage.transform.Find("Score").GetComponent<Text>();
    }

    void Start()
    {
        updateTaskRequest.SendRequest("");
    }

    public void ShowConfirmPanel(int id)
    {
        currentId = id;
        confirmPanel.SetActive(true);
    }
    public void ShowAddPanel()
    {
        addPanel.SetActive(true);
    }
    void Back()
    {
        uiMng.PopPanel();
    }

    internal void UpdateTasks(List<Task> tasks)
    {
        taskItems.Clear();
        for (int i = 0; i < layout.childCount; i++)
        {
            Destroy(layout.GetChild(i).gameObject);
        }
        int unConfirmedCount = 0;
        int finishedCount = 0;
        int unfinishedCount = 0;
        foreach (Task task in tasks)
        {
            Instantiate(Resources.Load<GameObject>("UIItem/TaskItem"), layout).GetComponent<TaskItem>().Set(task, this);
            switch (task.TaskState)
            {
                case TaskState.UnConfrimed:
                    unfinishedCount++;
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
        int totalCount = tasks.Count;
        if (totalCount == 0)
        {
            score.text="0";
            taskText.text = "今日任务已完成(0/0)\n尚有0项未确认";
            fillImage.fillAmount = 0;
        }
        else
        {
            score.text = ((int)(finishedCount*100f/totalCount)).ToString();
            taskText.text = "今日任务已完成("+finishedCount+"/"+totalCount+")\n尚有"+unConfirmedCount+"项未确认";
            fillImage.fillAmount = int.Parse(score.text) * 1f / 100;
        }
        InfoPanel.SetTaskInfo(score.text, taskText.text, fillImage.fillAmount);
    }

    public void AddTaskItem(TaskItem taskItem)
    {
        taskItems.Add(taskItem);
    }
    void Add()
    {
        if (string.IsNullOrEmpty(content.text))
        {
            facade.ShowMessage("任务不能为空");
            return;
        }
        addTaskRequest.SendRequest(content.text);
        addPanel.SetActive(false);
        ClearEdit();
    }

    void ClearEdit()
    {
        content.text = "";
    }
    public void Delete(int id)
    {
        deleteTaskRequest.SendRequest(id.ToString());
    }
    void Edit()
    {
        bool isEdit = !addButton.activeSelf;
        addButton.SetActive(isEdit);
        EditButtonText.text = isEdit ? "完成" : "编辑";
        foreach (TaskItem taskItem in taskItems)
        {
            taskItem.Edit(isEdit);
        }
    }
    public override void OnResume()
    {
        base.OnResume();
        updateTaskRequest.SendRequest("");
    }

    void FinishTask()
    {
        if (currentId < 0) return;
        confirmTaskRequest.SendRequest(currentId,TaskState.Finished);
        currentId = -1;
        confirmPanel.SetActive(false);
    }
    void UnFinishTask()
    {
        if (currentId < 0) return;
        confirmTaskRequest.SendRequest(currentId, TaskState.UnFinished);
        currentId = -1;
        confirmPanel.SetActive(false);
    }
}
