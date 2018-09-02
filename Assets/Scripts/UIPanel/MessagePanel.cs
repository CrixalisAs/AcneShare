using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : BasePanel
{
    private Text text;
    private float showTime = 2;
    private string message = null;
    private float targetAlpha = 0;
    public float smoothing = 4;
    private Queue<int> showQueue = new Queue<int>();
    void Update()
    {
        if (message != null)
        {
            ShowMessage(message);
            message = null;
        }
        if (text.color.a != targetAlpha)
        {
            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, targetAlpha), smoothing * Time.deltaTime);
            if (Mathf.Abs(text.color.a - targetAlpha) < 0.01)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, targetAlpha);
            }
        }
        SetUiIndex();
    }

    private void SetUiIndex()
    {
        int count = transform.parent.childCount;
        //参数为物体在当前所在的子物体列表中的顺序
        //count-1指把child物体在当前子物体列表的顺序设置为最后一个，0为第一个
        transform.SetSiblingIndex(count - 1);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        text = GetComponent<Text>();
        text.enabled = false;
        uiMng.InjectMsgPanel(this);
    }


    public void ShowMessageAsync(string msg)
    {
        message = msg;
    }
    public void ShowMessage(string msg)
    {
        targetAlpha = 1;
        text.text = msg;
        text.enabled = true;
        Invoke("Hide", Mathf.Max(msg.Length / 4, 2));
        showQueue.Enqueue(1);
    }

    private void Hide()
    {
        showQueue.Dequeue();
        if (showQueue.Count > 0) return;
        targetAlpha = 0;
    }
}
