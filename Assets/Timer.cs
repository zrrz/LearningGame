using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	WordManager manager;
	TextMesh textMesh;
	
	void Start() {
		manager = WordManager.instance;
		
		if(manager == null) {
			Debug.Log( gameObject.name + " could not find GameManager instance." );
			Destroy(this);
		}
		
		textMesh = GetComponent<TextMesh>();
//		transform.LookAt (Camera.main.transform.position + Camera.main.transform.forward * 1000f);
		textMesh.renderer.sortingOrder = 15;
	}
	
	void Update () {
		if(manager) {
			string minutes = ((int)(manager.gameTimer / 60)).ToString();
			string seconds = ((int)(manager.gameTimer % 60f)).ToString();
			
			if(seconds.Length == 1) {
				seconds = "0"+seconds;
			}
			
			textMesh.text = minutes + ":" + seconds;
		}
	}
}