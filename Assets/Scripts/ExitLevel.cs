using UnityEngine;
using System.Collections;

public class ExitLevel : MonoBehaviour {

	public delegate void OnLevelCleared();
	public event OnLevelCleared LevelClearedCallback;

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Player") {

			if (LevelClearedCallback != null) {
				LevelClearedCallback();
			}
		}
	}
}
