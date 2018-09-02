using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnScrollDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{

    private ScrollRect scrollRect;
    private bool isDraging = false;
    // Use this for initialization
    void Start () {

        scrollRect = GetComponent<ScrollRect>();
    }
	
	// Update is called once per frame
	void Update () {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDraging = false;
        Debug.Log(scrollRect.verticalNormalizedPosition);
    }
}
