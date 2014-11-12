using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WordManager : MonoBehaviour {

	static WordManager s_instance;

	[System.NonSerialized]
	public int currentLetterSpot = 0;

	public List<GameObject> wordPrefabs;

	public List<GameObject> letterPrefabs;

	public GameObject underscore;

	public List<GameObject> textPrefabs;

	public GameObject winText;

	Dictionary<string, GameObject> letterDictionary;

	[System.NonSerialized]
	public List<GameObject> lettersToSpawn;

	[System.NonSerialized]
	public List<GameObject> spawnedLetters;

	[System.NonSerialized]
	public GameObject currentWord;

	public Transform wordSpot;

	public Transform letterSpot;
	public float letterSpotXVariation = 4f;

	public float minLetterTime = 0.5f, maxLetterTime = 1.2f;

	float spawnTime;
	float timer = 0f;

	[System.NonSerialized]
	public bool changingWord = false;

	//public int wordsToWin = 1;

	[System.NonSerialized]
	public int wordsDone = 0;

	[System.NonSerialized]
	public bool gameWon = false;

	public float gameTimer = 0f;
	public float gameTime = 90.0f;

	void Awake() {
		s_instance = this;
	}

	void Start () {
		letterDictionary = new Dictionary<string, GameObject>();

		for(int i = 0; i < letterPrefabs.Count; i++) {
			letterDictionary.Add(letterPrefabs[i].name, letterPrefabs[i]);
		}

		lettersToSpawn = new List<GameObject> ();
		spawnedLetters = new List<GameObject> ();

		StartCoroutine("NewWord", false);

		gameTimer = gameTime;
	}

	void Update() {
		if(!gameWon) {
			timer += Time.deltaTime;
			if(timer > spawnTime) {
				timer = 0f;
				spawnTime = Random.Range(minLetterTime, maxLetterTime);

				int randomNum = Random.Range(0, lettersToSpawn.Count);
				GameObject t_obj = (GameObject)Instantiate(lettersToSpawn[randomNum], letterSpot.position + (Vector3.right * Random.Range(-letterSpotXVariation, letterSpotXVariation)), Quaternion.identity);
				lettersToSpawn.RemoveAt(randomNum);
				spawnedLetters.Add(t_obj);

				if(lettersToSpawn.Count == 0) {
					NewLetters();
				}
			}
			if(gameTimer > 0)
				gameTimer -= Time.deltaTime;
			if(gameTimer <= 0) {
				gameTimer = 0f;
				//Win game
				s_instance.gameWon = true;
				GameObject text = (GameObject) Instantiate(s_instance.winText);
				foreach (TextMesh t in text.transform.GetComponentsInChildren<TextMesh>()) {
					if(wordsDone == 1)
						t.text = wordsDone + " points!";
					else
						t.text = wordsDone + " points!";
				}
				StartCoroutine("DoNextLevelToggle");

				foreach(GameObject obj in s_instance.spawnedLetters) {
					Destroy(obj);
				}
			}
		}
	}

	IEnumerator DoNextLevelToggle() { 
		yield return new WaitForSeconds (1.5f);
		GameObject.Find("InputManagerMaster").GetComponent<InputOSC>().doLevelChange = true;
	}

	void NewLetters() {
		GameObject prefab = letterDictionary[currentWord.name[currentLetterSpot].ToString()];
		lettersToSpawn.Add(prefab);

		// Random letters
		for(int j = 0; j < 4; j++) {
			lettersToSpawn.Add(letterPrefabs[Random.Range(0, letterPrefabs.Count)]);
		}
	}
	
	IEnumerator NewWord(bool wait) {
		changingWord = true;

		int currentWordIndex = -1;

		if(currentWord) {
			for(int i = 0; i < wordPrefabs.Count; i++) {
				if(wordPrefabs[i].name == currentWord.name)
					currentWordIndex = i;
			}
		}
		currentLetterSpot = 0;

		if(currentWord) {
			Instantiate(textPrefabs[Random.Range(0, textPrefabs.Count)], currentWord.transform.position + Vector3.up * 1.5f, Quaternion.identity);
			yield return new WaitForSeconds(1.2f);
			Destroy(currentWord);
		}

		int randNum = Random.Range(0, wordPrefabs.Count - 1);

		while(randNum == currentWordIndex) {
			randNum = Random.Range(0, wordPrefabs.Count - 1);
		}

		currentWord = (GameObject)Instantiate(wordPrefabs[randNum], wordSpot.position, Quaternion.identity);
		currentWord.name = currentWord.name.Split('(')[0];

		for(int i = 0; i < currentWord.name.Length; i++) {
			GameObject t_obj = (GameObject) Instantiate(underscore, currentWord.transform.position + (Vector3.down * 2.5f) + (Vector3.right * i) - (Vector3.right * ((currentWord.name.Length - 1)/2f)), Quaternion.identity);
			t_obj.transform.parent = currentWord.transform;
		}

		NewLetters();
		changingWord = false;
	}

	public static WordManager instance {
		get {
			return s_instance;
		}
	}

	public static bool IsNextLetter(GameObject letter) {
		if(s_instance.currentWord.name[s_instance.currentLetterSpot] == letter.name[0]) {
			s_instance.spawnedLetters.Remove(letter);
			return true;
		} else {
			return false;
		}
	}

	public static void MoveLetter(GameObject letter) {
		letter.transform.parent = s_instance.currentWord.transform.GetChild(s_instance.currentLetterSpot);
		letter.transform.localPosition = new Vector3(0f, 0.6f);

		s_instance.currentLetterSpot++; 

		if(s_instance.currentLetterSpot > s_instance.currentWord.name.Length - 1) {
			s_instance.wordsDone++;
			s_instance.StartCoroutine("NewWord", true);
		}
	} 
}