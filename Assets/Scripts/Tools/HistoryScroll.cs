using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HistoryScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public HistoryPanel HistoryPanel;
    public void OnBeginDrag(PointerEventData eventData)
    {
        HistoryPanel.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        HistoryPanel.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HistoryPanel.OnEndDrag(eventData);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
