using System;
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
    private bool isFirstOpen = false;
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
        Save();
        uiMng = new UIManager(this);
        uiMng.OnInit();
    }

    private void DestroyManager()
    {
        uiMng.OnDestroy();
    }

    private void UpdateManager()
    {
        uiMng.Update();
    }

    void Save()
    {
        if (PlayerPrefs.HasKey(this.gameObject.name))
        {
            return;
        }
        else
        {
            isFirstOpen = true;
            PlayerPrefs.SetInt(gameObject.name,1);
        }
    }
    void OnDestroy()
    {
        DestroyManager();
    }
}