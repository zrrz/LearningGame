using UnityEngine;
using System.Collections;

public class DestroyAfterAnim : MonoBehaviour {

	void Start () {
		Destroy(gameObject, animation.clip.length);
	}
}