using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class UpdateShareRequest : BaseRequest
{
    private bool isUpdate = false;
    private SharePanel sharePanel;
    public override void Awake()
    {
        sharePanel = GetComponent<SharePanel>();
        requestCode = RequestCode.Share;
        actionCode = ActionCode.UpdateShare;
        base.Awake();
    }

    public class Share
    {
        public string Name;
        public byte[] Image;
        public string Content;

        public Share(string name, byte[] image, string content)
        {
            Name = name;
            Image = image;
            Content = content;
        }
    }
    List<Share> shares=new List<Share>();

    void Update()
    {
        if (isUpdate&& shares.Count!=0)
        {
            isUpdate = false;
            sharePanel.ListShares(shares);
            shares.Clear();
        }
    }

    public override void SendRequest()
    {
        Debug.Log("Send");
        base.SendRequest("");
    }

    public override void OnResponse(string data)
    {
        base.OnResponse(data);
        string[] strs = data.Split('$');
        
        foreach (string str in strs)
        {
            string[] s = str.Split('&');
            string name = s[0];
            byte[] image = Tools.ParseBytes(s[1]);
            string content = s[2];
            shares.Add(new Share(name, image, content));
        }
        isUpdate = true;
    }
}
