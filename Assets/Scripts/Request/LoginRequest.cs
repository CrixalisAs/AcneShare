using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class LoginRequest : BaseRequest
{
    private LoginPanel loginPanel;
    // Use this for initialization
    public override void Awake()
    {
        loginPanel = GetComponent<LoginPanel>();
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
        base.Awake();
    }

    public void SendRequest(string username, string password)
    {
        string data = username + "," + password;
        SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        base.OnResponse(data);
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        if (returnCode == ReturnCode.Success)
        {
            int id = int.Parse(strs[1]);
            string username = strs[2];
            string name = strs[3];
            int age = int.Parse(strs[4]);
            bool sex = strs[5] == "1";
            byte[] image = Tools.ParseBytes(strs[6]);
            UserData userData = new UserData(id, username,name,age,sex,image);
            facade.SetUserData(userData);
        }
        loginPanel.OnLoginResponse(returnCode);
    }
}
