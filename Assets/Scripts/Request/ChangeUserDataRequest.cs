﻿using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class ChangeUserDataRequest : BaseRequest {

	// Use this for initialization
    public override void Awake()
    {
        requestCode = RequestCode.None;
        actionCode = ActionCode.None;
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("");
    }
}
