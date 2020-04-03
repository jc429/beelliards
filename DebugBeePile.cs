using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBeePile : MonoBehaviour
{
	public GameObject beePrefab;
	// Start is called before the first frame update
	void Start()
	{
			
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.RightAlt)){
			string time = System.DateTime.Now.ToString("yyyy'-'MM'-'dd'--'HH'-'mm'-'ss");
			string path = System.IO.Path.Combine(Application.persistentDataPath, "Pictures/screenshot " + time + ".png");
			ScreenCapture.CaptureScreenshot(path);
			Debug.Log("Screen capture saved! " + path);
		}
		if(Input.GetKey(KeyCode.B)){
			GameObject bee = Instantiate(beePrefab);
			bee.transform.position = new Vector3(Random.Range(-5, 5), 10, Random.Range(-5, 5) );
		}
	}
}
