using UnityEngine;
using System.Collections;

public class InputState : MonoBehaviour {

	public bool actionButton;
	public float moveAxis;
	public float absVelX = 0f;
	public float absVelY = 0f;
	public bool standing;
	public float standingThreshold = 1;

	private Rigidbody2D body2d;

	void Awake(){
		body2d = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {
		moveAxis = Input.GetAxis ("Horizontal");
	}

	void FixedUpdate(){
		absVelX = Mathf.Abs (body2d.velocity.x);
		absVelY = Mathf.Abs (body2d.velocity.y);

		standing = absVelY <= standingThreshold;
	}
}
