using System;
using Common;
using UnityEngine;

public class GameFacade : MonoBehaviour
{
    private static GameFacade _instance;

    public static GameFacade Instance
    {
        get
        {
            //if (_instance == null)
            //{
            //    Debug.Log(1);
            //    _instance = GameObject.Find("Facade").GetComponent<GameFacade>();
            //}
            return _instance;
        }
    }


    private UIManager uiMng;
    private RequestManager requestMng;
    private ClientManager clientMng;
    private PlayerManager playerManager;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateManager();
    }

    private void Init()
    {
        clientMng=new ClientManager(this);
        uiMng = new UIManager(this);
        playerManager=new PlayerManager(this);
        requestMng=new RequestManager(this);
        clientMng.OnInit();
        uiMng.OnInit();
        playerManager.OnInit();
        requestMng.OnInit();
    }

    private void DestroyManager()
    {
        clientMng.OnDestroy();
        uiMng.OnDestroy();
        playerManager.OnDestroy();
        requestMng.OnDestroy();
    }

    private void UpdateManager()
    {
        clientMng.Update();
        uiMng.Update();
        playerManager.Update();
        requestMng.Update();
    }

    void OnDestroy()
    {
        DestroyManager();
    }
    #region Request


    public void AddRequest(ActionCode actionCode, BaseRequest Request)
    {
        requestMng.AddRequest(actionCode, Request);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestMng.RemoveRequest(actionCode);
    }
    public void HandleResponse(ActionCode actionCode, string data)
    {
        requestMng.HandleResponse(actionCode, data);
    }
    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        clientMng.SendRequest(requestCode, actionCode, data);
    }

    #endregion

    #region UI


    public UIPanelType GetCurrentPanelType()
    {
        return uiMng.GetCurrentPanelType();
    }

    public BasePanel GetCurrentPanel()
    {
        return uiMng.GetCurrentPanel();
    }
    public void ShowMessage(string msg)
    {
        uiMng.ShowMessage(msg);
    }

    public void SetMessagePos(float y)
    {
        uiMng.SetMessagePos(y);
    }
    #endregion

    public void SetUserData(UserData ud)
    {
        playerManager.UserData = ud;
    }

    public UserData GetUserData()
    {
        return playerManager.UserData;
    }

    public static Sprite TransBytesToSprite(byte[] image)
    {
        Texture2D texture = new Texture2D(400, 400);
        texture.LoadImage(image);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
    }
}