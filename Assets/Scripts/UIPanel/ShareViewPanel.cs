using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareViewPanel : BasePanel
{

    private Text content;

    private Text name;

    private Image image;
	// Use this for initialization
	void Awake ()
	{
	    content = transform.Find("ScrollPanel/Layout/Content").GetComponent<Text>();
	    name = transform.Find("Name").GetComponent<Text>();
	    image = transform.Find("Image").GetComponent<Image>();
        transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(Back);
	}

    public void Set(string content, string name, Sprite image)
    {
        this.content.text = content;
        this.name.text = name;
        this.image.sprite = image;
    }

    void Back()
    {
        uiMng.PopPanel();
    }
}
