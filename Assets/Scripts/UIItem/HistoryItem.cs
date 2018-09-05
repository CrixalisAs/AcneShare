using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

public class HistoryItem : MonoBehaviour
{

    public HistoryPanel HistoryPanel;
    private int id;
    public string Content;
    private Image photo;
    private Button selectPhotoButton;
    private Button photographButton;
    private Text date;
	// Use this for initialization
	void Awake ()
	{
	    selectPhotoButton = transform.Find("SelectPhotoButton").GetComponent<Button>();
        selectPhotoButton.onClick.AddListener(SelectPhoto);
	    photographButton = transform.Find("PhotographButton").GetComponent<Button>();
        photographButton.onClick.AddListener(Photograph);
        photo = transform.Find("Photo").GetComponent<Image>();
	    date = transform.Find("Date").GetComponent<Text>();
	    selectPhotoButton.gameObject.SetActive(false);
	    photographButton.gameObject.SetActive(false);
    }

    public HistoryItem Set(History history, HistoryPanel historyPanel)
    {
        HistoryPanel = historyPanel;
        if (history.Date == DateTime.Today)
        {
            selectPhotoButton.gameObject.SetActive(true);
            photographButton.gameObject.SetActive(true);
            photo.sprite = Resources.Load<Sprite>("Sprites/Add");
        }
        else if(history.Photo != null)
        {
             photo.sprite = GameFacade.TransBytesToSprite(history.Photo,600,900);
        }
        date.text = history.Date.Year + "年\n" + history.Date.Month + "月" + history.Date.Day + "日";
        id = history.Id;
        Content = history.Content;
        return this;
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
    void CameraShotComplete(Texture2D img, string path)
    {
        Sprite sprite = Sprite.Create(img, new Rect(0, 0, 600, 900),
            new Vector2(0.5f, 0.5f));
        photo.sprite = sprite;
        photo.color = Color.white;
        string imageData = Tools.PackBytes(Tools.ReadPNG(path));
        HistoryPanel.SavePhoto(imageData);
        selectPhotoButton.gameObject.SetActive(false);
        photographButton.gameObject.SetActive(false);
    }
    void ImagePicked(Texture2D img, string path)
    {
        Sprite sprite = Sprite.Create(img, new Rect(0, 0, 600, 900),
            new Vector2(0.5f, 0.5f));
        photo.sprite = sprite;
        photo.color = Color.white;
        string imageData = Tools.PackBytes(Tools.ReadPNG(path));
        HistoryPanel.SavePhoto(imageData);
        selectPhotoButton.gameObject.SetActive(false);
        photographButton.gameObject.SetActive(false);
    }
    void SelectPhoto()
    {
        NativeToolkit.PickImage();
    }

    void Photograph()
    {
        NativeToolkit.TakeCameraShot();
    }
}
