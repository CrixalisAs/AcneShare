using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{

    private TaskPanel taskPanel;
    private int id;
    private Text content;
    private GameObject deleteButton;
    private Image confirmImage;
    private GameObject confirmButton;
	// Use this for initialization
	void Awake ()
	{
	    content = transform.Find("Content").GetComponent<Text>();
	    deleteButton = transform.Find("DeleteButton").gameObject;
	    deleteButton.GetComponent<Button>().onClick.AddListener(Delete);

	    confirmButton = transform.Find("ConfirmButton").gameObject;
	    confirmButton.GetComponent<Button>().onClick.AddListener(Confirm);
	    confirmImage = confirmButton.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public TaskItem Set(Task task, TaskPanel taskPanel)
    {
        this.taskPanel = taskPanel;
        id = task.Id;
        content.text = task.Content;
        switch (task.TaskState)
        {
            case TaskState.UnConfrimed:
                confirmImage.sprite = taskPanel.UnConfirmed;
                break;
            case TaskState.Finished:
                confirmImage.sprite = taskPanel.Finished;
                confirmButton.GetComponent<Button>().interactable = false;
                break;
            case TaskState.UnFinished:
                confirmImage.sprite = taskPanel.UnFinished;
                confirmButton.GetComponent<Button>().interactable = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        taskPanel.AddTaskItem(this);
        return this;
    }

    void Delete()
    {
        taskPanel.Delete(id);
    }

    void Confirm()
    {
        taskPanel.ShowConfirmPanel(id);
    }

    public void Edit(bool isEdit)
    {
        deleteButton.SetActive(isEdit);
        confirmButton.SetActive(!isEdit);
    }
}
