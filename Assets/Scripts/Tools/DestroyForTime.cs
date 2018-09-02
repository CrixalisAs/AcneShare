using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyForTime : MonoBehaviour
{

    public float Time;
	// Use this for initialization
	void Start () {
		Destroy(gameObject,Time);
	}
	
}
