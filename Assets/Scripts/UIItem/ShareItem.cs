using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareItem : MonoBehaviour
{
    private Image image;
    private SharePanel sharePanel;
    private Text _content;
    public string Content { get { return _content.text; } }

    private Text name;
	// Use this for initialization
	void Awake ()
	{
	    image = transform.Find("Image").GetComponent<Image>();
	    _content = transform.Find("Content").GetComponent<Text>();
	    name = transform.Find("Name").GetComponent<Text>();
	    transform.Find("Content").GetComponent<Button>().onClick.AddListener(ShareView);
	}

    public ShareItem Set(SharePanel sharePanel,Sprite image, string content, string name)
    {
        this.sharePanel = sharePanel;
        this.image.sprite = image;
        this._content.text = content;
        this.name.text = name;
        return this;
    }

    void ShareView()
    {
        ShareViewPanel panel = sharePanel.ShareView();
        panel.Set(_content.text, name.text, image.sprite);
    }
}
