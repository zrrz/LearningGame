using UnityEngine;
using System.Collections;

public class EscapeToClose : MonoBehaviour {
	
	void Start () {
		gameObject.name += "Master";
		if(GameObject.Find("EscapeManager"))
			DestroyImmediate(GameObject.Find("EscapeManager"));

		DontDestroyOnLoad (gameObject);
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
}
