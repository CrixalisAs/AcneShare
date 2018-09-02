using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class ShareRequest : BaseRequest
{
    private EditSharePanel editSharePanel;
    public override void Awake()
    {
        editSharePanel = GetComponent<EditSharePanel>();
        requestCode = RequestCode.Share;
        actionCode = ActionCode.Share;
        base.Awake();
    }


    public override void SendRequest()
    {
        base.SendRequest(editSharePanel.Content.text);
    }
}
