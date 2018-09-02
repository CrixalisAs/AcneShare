using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditSharePanel : BasePanel
{
    public InputField Content;
    private ShareRequest shareRequest;
    private GameObject savePanel;
	// Use this for initialization
	void Start ()
	{
	    shareRequest = GetComponent<ShareRequest>();
	    Content = transform.Find("InputField").GetComponent<InputField>();
        transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(Back);
	    transform.Find("SendButton").GetComponent<Button>().onClick.AddListener(Send);
        savePanel = transform.Find("SavePanel").gameObject;
        savePanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Send()
    {
        if (string.IsNullOrEmpty(Content.text))
        {
            facade.ShowMessage("要发送的内容为空");
            return;
        }
        shareRequest.SendRequest();
        uiMng.PopAndDestroy();
    }

    void Back()
    {
        savePanel.SetActive(true);
    }
    public void No()
    {
        uiMng.PopAndDestroy();
    }
    public void Yes()
    {
        uiMng.PopPanel();
        savePanel.SetActive(false);
    }
}
