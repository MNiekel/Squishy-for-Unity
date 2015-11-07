using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject boxPrefab;
	public GameObject inactivePrefab;

	public Sprite[] sprites;

	private int boxWidth = 16;
	private float spawnY;
	private float spawnX;

	private bool gameRunning;
	private GameObject player;
	private GameObject box;
	private GameObject floor;
	private GameObject left;
	private GameObject right;
	private GameObject next;
	private GameObject lamp;
	private GameObject broken;
	private GameObject ui;
	private GameObject logo;
	private GameObject helpText;
	private Sprite nextSprite;
	private GameObject activeBox;

	void Awake(){
		floor = GameObject.Find ("Floor");
		left = GameObject.Find ("Leftside");
		right = GameObject.Find ("Rightside");
		next = GameObject.Find ("Nextbox");
		lamp = GameObject.Find ("Lamp");
		broken = GameObject.Find ("BrokenLamp");
		ui = GameObject.Find ("UI");
		logo = GameObject.Find ("Logo");
		helpText = GameObject.Find ("HelpText");
	}
	
	void Start () {

		var pos = left.transform.position;
		pos.x = -((Screen.width / PixelPerfectCamera.pixelsToUnits) / 2);
		left.transform.position = pos;
		
		pos = right.transform.position;
		pos.x = ((Screen.width / PixelPerfectCamera.pixelsToUnits) / 2);
		right.transform.position = pos;

		pos = floor.transform.position;
		pos.y = -((Screen.height / PixelPerfectCamera.pixelsToUnits) / 2);
		floor.transform.position = pos;

		pos = lamp.transform.position;
		pos.x = -(Screen.width / PixelPerfectCamera.pixelsToUnits) / 4;
		pos.y = (Screen.height / PixelPerfectCamera.pixelsToUnits) / 6;
		lamp.transform.position = pos;

		pos = broken.transform.position;
		pos.x = (Screen.width / PixelPerfectCamera.pixelsToUnits) / 4;
		pos.y = (Screen.height / PixelPerfectCamera.pixelsToUnits) / 6;
		broken.transform.position = pos;

		pos = next.transform.position;
		boxWidth = (int) next.GetComponent<SpriteRenderer> ().bounds.size.x;
		pos.y = -((Screen.height / PixelPerfectCamera.pixelsToUnits) / 2) + boxWidth / 2;
		pos.x = -((Screen.width / PixelPerfectCamera.pixelsToUnits) / 2) + boxWidth;
		next.transform.position = pos;

		spawnY = (Screen.height / PixelPerfectCamera.pixelsToUnits) / 2;

		helpText.SetActive (false);

		gameRunning = false;
	}

	void Update() {
		if (gameRunning) {
			if (Input.GetKeyDown(KeyCode.P)) {
				Debug.Log ("PAUSE");
				PauseGame();
			}
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			gameRunning = true;
			Debug.Log ("Restart game");
			Clear();
			ResetGame();
			return;
		}

		if (Input.GetKeyDown(KeyCode.H)) {
			ToggleHelp();
			return;
		}
	}

	void OnBoxGrounded() {

		activeBox.SetActive (false);
		DrawInactive (activeBox.transform.position, activeBox.GetComponent<SpriteRenderer> ().sprite);
		SpawnFallingBox (player.transform.position.x);
		SetNext ();
	}

	void OnLevelCleared() {

		Debug.Log ("Level cleared");

		broken.GetComponent<Light> ().enabled = true;

		StopGame ();
	}

	
	void OnPlayerKilled(){
		
		Debug.Log ("Player killed");

		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;

		StopGame ();
	}
	
	void ResetGame(){

		broken.GetComponent<Light> ().enabled = false;

		nextSprite = sprites [Random.Range (0, sprites.Length)];
		next.GetComponent<SpriteRenderer> ().sprite = nextSprite;

		player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

		activeBox = Instantiate(boxPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		boxWidth = (int) activeBox.GetComponent<SpriteRenderer> ().bounds.size.x;
		activeBox.SetActive (false);

		var playerDestroyScript = player.GetComponent<DestroyCrushed> ();
		playerDestroyScript.DestroyCallback += OnPlayerKilled;

		var levelClearedScript = broken.GetComponent<ExitLevel> ();
		levelClearedScript.LevelClearedCallback += OnLevelCleared;

		var boxGroundedScript = activeBox.GetComponent<FallingBox> ();
		boxGroundedScript.GroundedCallback += OnBoxGrounded;

		 
		gameRunning = true;
		Time.timeScale = 1f;
		ui.SetActive (false);

		SpawnFallingBox (player.transform.position.x);
	}

	/*
	private string FormatTime(float value){
		TimeSpan t = TimeSpan.FromSeconds (value);
		
		return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
	}
	*/

	private void SpawnFallingBox(float x) {
		spawnX = (float) ((int) (x + Mathf.Sign(x) * boxWidth / 2) / boxWidth) * boxWidth;
		//spawnX = (float) ((int) (x + Mathf.Sign(x) * boxWidth / 2) / boxWidth) * boxWidth + boxWidth;
		activeBox.transform.position = new Vector3 (spawnX, spawnY, 0);
		activeBox.GetComponent<BoxObject> ().Init (nextSprite);
		activeBox.SetActive (true);
	}

	private void SetNext() {
		nextSprite = sprites [Random.Range (0, sprites.Length)];
		next.GetComponent<SpriteRenderer> ().sprite = nextSprite;
	}

	private void DrawInactive(Vector3 position, Sprite sprite) {
		var rounded = position;
		rounded.y = (float) (Mathf.RoundToInt(position.y / boxWidth) * boxWidth);
		if (rounded.y >= spawnY) {
			Debug.Log ("Tower of boxes too high");
			StopGame();
		}
		GameObject box = Instantiate (inactivePrefab) as GameObject;
		box.transform.position = rounded;
		box.GetComponent<BoxObject> ().Init (sprite);
	}

	private void StopGame() {

		var playerDestroyScript = player.GetComponent<DestroyCrushed> ();
		playerDestroyScript.DestroyCallback -= OnPlayerKilled;

		var levelClearedScript = broken.GetComponent<ExitLevel> ();
		levelClearedScript.LevelClearedCallback -= OnLevelCleared;

		var boxGroundedScript = activeBox.GetComponent<FallingBox> ();
		boxGroundedScript.GroundedCallback -= OnBoxGrounded;

		Time.timeScale = 0f;
		gameRunning = false;
		ui.SetActive (true);
		logo.SetActive (true);
		helpText.SetActive (false);
	}

	private void Clear() {
		GameObject[] boxes = GameObject.FindGameObjectsWithTag ("Box");

		foreach (GameObject box in boxes) {
			Destroy (box);
		}

		Destroy (player);
	}

	private void PauseGame() {
		Time.timeScale = (Time.timeScale == 0f) ? 1f : 0f;
	}

	private void ToggleHelp() {
		logo.SetActive (!logo.activeSelf);
		helpText.SetActive (!helpText.activeSelf);
	}
}
