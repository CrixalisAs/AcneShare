using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SharePanel : BasePanel
{
    private Transform layout;
    private UpdateShareRequest updateShareRequest;
    private GameObject shareItem;


    void Awake()
    {
        updateShareRequest = GetComponent<UpdateShareRequest>();
        layout = transform.Find("ScrollPanel/Layout");
        transform.Find("ShareButton").GetComponent<Button>().onClick.AddListener(Share);
        shareItem = Resources.Load<GameObject>("UIItem/ShareItem");
        facade.ShowMessage("");
    }

    
    void Start()
    {
        updateShareRequest.SendRequest();
    }
    void Share()
    {
        uiMng.PushPanel(UIPanelType.EditShare);
    }

    public void ListShares(List<UpdateShareRequest.Share> shares)
    {
        Clear();
        foreach (UpdateShareRequest.Share share in shares)
        {
            Texture2D texture = new Texture2D(400, 400);
            texture.LoadImage(share.Image);
            Sprite Image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            Instantiate(shareItem, layout).GetComponent<ShareItem>().Set(Image, share.Content, share.Name).SharePanel =
                this;
        }
    }

    public override void OnResume()
    {
        base.OnResume();
        updateShareRequest.SendRequest();

    }

    void Clear()
    {
        for (int i = 0; i < layout.childCount; i++)
        {
            Destroy(layout.GetChild(i).gameObject);
        }
    }

    public ShareViewPanel ShareView()
    {
        return uiMng.PushPanel(UIPanelType.ShareView) as ShareViewPanel;
    }
}
