using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public float jumpSpeed = 20f;
	public float forwardSpeed = 48f;

	private Rigidbody2D body2d;
	private InputState inputState;
	private float hitBlock = 0;

	void Awake(){
		body2d = GetComponent<Rigidbody2D> ();
		inputState = GetComponent<InputState> ();
	}

	void Update () {
		Vector2 velocity = new Vector2 (body2d.velocity.x, body2d.velocity.y);

		velocity.x = inputState.moveAxis * forwardSpeed;

		if (hitBlock != 0 && inputState.standing) {
			if (Mathf.Sign (hitBlock) == Mathf.Sign (inputState.moveAxis)) {
				velocity.y = jumpSpeed;
			}
		}


		body2d.velocity = velocity;
	}

	public void OnCollisionEnter2D(Collision2D collision) {

		if (collision.gameObject.tag == "Box") {
			
			ContactPoint2D[] contacts = collision.contacts;
			
			for (int i = 0; i < contacts.Length; i++) {

				if (contacts[i].normal.x == 1 || contacts[i].normal.x == -1) {

					Vector2 velocity = new Vector2 (body2d.velocity.x, body2d.velocity.y);
					
					if (inputState.standing) {
						velocity.y = jumpSpeed;
					}

					body2d.velocity = velocity;
					break;
				}
			}
		}

	}
}
