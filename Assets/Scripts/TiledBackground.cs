using UnityEngine;
using System.Collections;

public class TiledBackground : MonoBehaviour {

	public int textureWidth = 32;
	public int textureHeight = 32;
	public bool scaleHorizontially = true;
	public bool scaleVertically = true;

	// Use this for initialization
	void Start () {
	
		var newWidth = !scaleHorizontially ? 1 : Mathf.Ceil (Screen.width / (textureWidth * PixelPerfectCamera.scale));
		var newHeight = !scaleVertically ? 1 : Mathf.Ceil (Screen.height / (textureHeight * PixelPerfectCamera.scale));

		transform.localScale = new Vector3 (newWidth * textureWidth, newHeight * textureHeight, 1);

		GetComponent<Renderer> ().material.mainTextureScale = new Vector3 (newWidth, newHeight, 1);
	}

}
