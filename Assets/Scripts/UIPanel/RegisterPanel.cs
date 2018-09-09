using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    private InputField usernameIF;
    private InputField passwordIF;
    private InputField rePasswordIF;
    private InputField name;
    private InputField age;
    private Image headImage;
    private Toggle sex;
    private Text sexText;
    private byte[] image = null;
    private RegisterRequest registerRequest;
    private bool isSuccess = false;

    void Awake()
    {
        registerRequest = GetComponent<RegisterRequest>();
        usernameIF = transform.Find("Bg/UsernameLabel/UsernameInput").GetComponent<InputField>();
        passwordIF = transform.Find("Bg/PasswordLabel/PasswordInput").GetComponent<InputField>();
        rePasswordIF = transform.Find("Bg/RePasswordLabel/RePasswordInput").GetComponent<InputField>();
        name = transform.Find("Bg/Name/InputField").GetComponent<InputField>();
        age = transform.Find("Bg/Age/InputField").GetComponent<InputField>();
        sex = transform.Find("Bg/Sex/Toggle").GetComponent<Toggle>();
        headImage= transform.Find("Bg/Head/Image").GetComponent<Image>();
        sexText = sex.transform.Find("Text").GetComponent<Text>();
        sex.onValueChanged.AddListener(x=> sexText.text=x ?"女":"男");
        transform.Find("Bg/Head/PhotoSelect").GetComponent<Button>().onClick.AddListener(OnPickImagePress);
        transform.Find("Bg/Head/Photograph").GetComponent<Button>().onClick.AddListener(OnCameraPress);
        transform.Find("Bg/CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
        transform.Find("Bg/RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
    }

    void OnEnable()
    {
        NativeToolkit.OnImagePicked += ImagePicked;
        NativeToolkit.OnCameraShotComplete += CameraShotComplete;
    }

    void OnDisable()
    {
        NativeToolkit.OnImagePicked -= ImagePicked;
        NativeToolkit.OnCameraShotComplete -= CameraShotComplete;
    }
    void Update()
    {
        if (isSuccess)
        {
            isSuccess = false;
            uiMng.PopPanel();
            
        }
    }
    public void OnCameraPress()
    {
        NativeToolkit.TakeCameraShot();
    }
    public void OnPickImagePress()
    {
        NativeToolkit.PickImage();
    }
    void CameraShotComplete(Texture2D img, string path)
    {
        Sprite sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height),
            new Vector2(0.5f, 0.5f));
        headImage.sprite = sprite;
        headImage.color = Color.white;
        image = Tools.ReadPNG(path);
    }
    void ImagePicked(Texture2D img, string path)
    {
        Sprite sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height),
            new Vector2(0.5f, 0.5f));
        headImage.sprite = sprite;
        headImage.color=Color.white;
        image = Tools.ReadPNG(path);
    }
    private void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
    }
    public override void OnResume()
    {
        base.OnResume();
    }
    private void OnRegisterClick()
    {
        PlayClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空";
        }
        else if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "密码不能为空";
        }
        else if (passwordIF.text != rePasswordIF.text)
        {
            msg += "密码不一致";
        }
        else if (string.IsNullOrEmpty(name.text))
        {
            msg += "昵称不能为空";
        }
        else if (string.IsNullOrEmpty(age.text))
        {
            msg += "年龄不能为空";
        }
        else if (int.Parse(age.text) <= 0)
        {
            msg += "年龄错误";
        }
#if UNITY_EDITOR
        registerRequest.SendRequest(usernameIF.text, passwordIF.text, name.text, age.text, sex.isOn, Tools.PackBytes(Tools.ReadPNG("E:/5.png")));
        return;
#else
        else if (image == null)
        {
            msg += "您还没有选择头像";
        }

#endif
        if (msg != "")
        {
            uiMng.ShowMessage(msg);
            return;
        }
        
        registerRequest.SendRequest(usernameIF.text, passwordIF.text,name.text,age.text,sex.isOn,Tools.PackBytes(image));
    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageSync("注册成功");
            isSuccess = true;
        }
        else
        {
            uiMng.ShowMessageSync("用户名重复");
        }
    }
    
}
