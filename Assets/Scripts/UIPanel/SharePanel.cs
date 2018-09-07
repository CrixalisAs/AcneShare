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
    List<ShareItem> shareItems=new List<ShareItem>();
    private List<UpdateShareRequest.Share> shares=new List<UpdateShareRequest.Share>();

    void Awake()
    {
        updateShareRequest = GetComponent<UpdateShareRequest>();
        layout = transform.Find("ScrollPanel/Layout");
        transform.Find("ShareButton").GetComponent<Button>().onClick.AddListener(Share);
        transform.Find("InfoButton").GetComponent<Button>().onClick.AddListener(Info);
        transform.Find("KnowledgeButton").GetComponent<Button>().onClick.AddListener(Knowledge);
        transform.Find("SelectPanel/InputField").GetComponent<InputField>().onValueChanged.AddListener(Select);
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

    void Knowledge()
    {
        uiMng.PushPanel(UIPanelType.Knowledge);
    }
    void Select(string param)
    {
        if (string.IsNullOrEmpty(param))
        {
            ListShares(shares);
            return;
        }
        string[] datas=new string[shareItems.Count];
        for (int i = 0; i < datas.Length; i++)
        {
            datas[i] = shareItems[i].Content;
        }
        datas = Tools.Search(param, datas);
        for (int i = 0; i < datas.Length; i++)
        {
            int index = shareItems.FindIndex(item => item.Content == datas[i]);
            shareItems[index].transform.SetSiblingIndex(i);
        }
    }
    void Info()
    {
        uiMng.PushPanel(UIPanelType.Info);
    }
    public void ListShares(List<UpdateShareRequest.Share> shares)
    {
        Clear(); shareItems.Clear();
        this.shares = new List<UpdateShareRequest.Share>(shares);
        foreach (UpdateShareRequest.Share share in shares)
        {
            shareItems.Add(Instantiate(shareItem, layout).GetComponent<ShareItem>().Set(this,GameFacade.TransBytesToSprite(share.Image), share.Content, share.Name));
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
