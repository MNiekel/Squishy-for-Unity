using UnityEngine;
using System.Collections;

public class BoxObject : MonoBehaviour {

	private int weight = 0;

	public void Init(Sprite sprite) {
		var renderer = GetComponent<SpriteRenderer> ();
		renderer.sprite = sprite;
		
		switch (sprite.name) {
		case "CardBox":
			weight = 2;
			break;
		case "WoodBox":
			weight = 4;
			break;
		case "MetalBox":
			weight = 6;
			break;
		case "RockBox":
			weight = 8;
			break;
		default:
			weight = 0;
			break;
		}
	}

	public int GetWeight() {
		return weight;
	}
}
