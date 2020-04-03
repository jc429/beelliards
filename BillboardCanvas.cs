using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{

	Quaternion originalRotation;

	void Start()
	{
		originalRotation = Quaternion.Euler(0,0,0);
	}

	void Update()
	{
		transform.rotation = Camera.main.transform.rotation * originalRotation;   
	}
}
