using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class ScheduleAmountRequest : BaseRequest
{
    private InfoPanel infoPanel;
    private int amount = -1;
    public override void Awake()
    {
        infoPanel = GetComponent<InfoPanel>();
        requestCode = RequestCode.Info;
        actionCode = ActionCode.ScheduleAmount;
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("");
    }

    // Update is called once per frame
	void Update () {
	    if (amount != -1)
	    {
	        infoPanel.SetAmount(amount);
	        amount = -1;
            Debug.Log("SetAmount");
	    }
	}

    public override void OnResponse(string data)
    {
        base.OnResponse(data);
        Debug.Log(data);
        amount = int.Parse(data);
    }
}
