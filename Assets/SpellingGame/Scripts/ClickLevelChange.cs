using UnityEngine;
using System.Collections;

public class ClickLevelChange : MonoBehaviour {

	public string level = "Main";

	public bool waitTillAnimDone = false;
	
	void OnMouseDown() {
		if(waitTillAnimDone) {
			if(animation) {
				if(animation.isPlaying)
					return;
			}
		}
		Application.LoadLevel (level);
	}
}