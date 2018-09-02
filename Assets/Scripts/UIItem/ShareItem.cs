using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareItem : MonoBehaviour
{
    private Image image;
    public SharePanel SharePanel;
    private Text content;

    private Text name;
	// Use this for initialization
	void Awake ()
	{
	    image = transform.Find("Image").GetComponent<Image>();
	    content = transform.Find("Content").GetComponent<Text>();
	    name = transform.Find("Name").GetComponent<Text>();
	    transform.Find("Content").GetComponent<Button>().onClick.AddListener(ShareView);
	}

    public ShareItem Set(Sprite image, string content, string name)
    {
        this.image.sprite = image;
        this.content.text = content;
        this.name.text = name;
        return this;
    }

    void ShareView()
    {
        ShareViewPanel panel = SharePanel.ShareView();
        panel.Set(content.text, name.text, image.sprite);
    }
}
