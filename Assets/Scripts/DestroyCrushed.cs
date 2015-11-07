using UnityEngine;
using System.Collections;

public class DestroyCrushed : MonoBehaviour {

	public delegate void OnDestroy();
	public event OnDestroy DestroyCallback;

	public void DestroyObject() {

		Destroy (gameObject);
		
		if (DestroyCallback != null) {
			DestroyCallback();
		}
	}
}
