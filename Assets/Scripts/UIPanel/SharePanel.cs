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
        transform.Find("InfoButton").GetComponent<Button>().onClick.AddListener(Info);
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

    void Info()
    {
        uiMng.PushPanel(UIPanelType.Info);
    }
    public void ListShares(List<UpdateShareRequest.Share> shares)
    {
        Clear();
        foreach (UpdateShareRequest.Share share in shares)
        {
            Instantiate(shareItem, layout).GetComponent<ShareItem>().Set(GameFacade.TransBytesToSprite(share.Image), share.Content, share.Name).SharePanel =
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
