using UnityEngine;
using System.Collections;
using OSC;

public class InputOSC : MonoBehaviour {
	
	public bool doLevelChange = true;

	void Start () {
		if (GameObject.Find (gameObject.name + "Master")) {
			Destroy (gameObject);
		} else {
			GameObject.DontDestroyOnLoad (gameObject);
			gameObject.name += "Master";
		}
	}

	void Update () {
		if(Input.GetButtonDown("Fire1")) {
			Fire (Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
		if(Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	void Fire(Vector2 pos) {
		if (doLevelChange) {
			Application.LoadLevel(Application.loadedLevelName == "Intro" ? 1 : 0);
			if(Application.loadedLevel == 0)
				doLevelChange = false;
		} else {
			RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
			if(hit.collider != null) {
				hit.collider.SendMessage("OnShot", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public void OSCMessageReceived(OSC.NET.OSCMessage message){
		float x = (float)message.Values[0];
		float y = (float)message.Values[1];
		Fire(new Vector2(x, y));
		//    	if(message.Address == "/endGame"){
		//      		AdjustGameSetting("Quit Game", true);
		//      		timer = 0;
		//    	} else if(message.Address == "/timeChange"){
		//      		ArrayList args = message.Values;
		//	      	print(args[0]);
		//	     	string recieved = args[0].ToString();
		//	     	float time;
		//	     	float.TryParse(recieved, out time);
		//	      	AdjustGameSetting("Game Time", time);
		//    	}
	}

}
