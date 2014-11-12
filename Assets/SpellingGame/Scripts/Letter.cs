using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour {

	public float moveSpeed = 1f;

	public GameObject goodParticles, badParticles;

	bool move = true;

	void Start () {
	
	}

	void Update () {
		if(move)
			transform.Translate(0f, moveSpeed*Time.deltaTime, 0f);
		if(!renderer.isVisible) {
			Destroy(gameObject);
		}
	}

	public void OnShot() {
		if(move) {
			if(!WordManager.instance.changingWord) {
				if(WordManager.IsNextLetter(gameObject)) {
					Instantiate(goodParticles, transform.position + Vector3.back, Quaternion.identity);
					move = false;
					WordManager.MoveLetter(gameObject);
					//if(collider2D)
					this.enabled = false;
				} else {
					Destroy(gameObject);
					Instantiate(badParticles, transform.position + Vector3.back, Quaternion.identity);
				}
			}
		}
	}
}