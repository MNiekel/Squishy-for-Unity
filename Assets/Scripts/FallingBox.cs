using UnityEngine;
using System.Collections;

public class FallingBox : MonoBehaviour {

	public delegate void OnBoxGrounded();
	public event OnBoxGrounded GroundedCallback;

	public Vector2 velocity = Vector2.zero;
	
	private Rigidbody2D body2d;

	void Awake() {
		body2d = GetComponent<Rigidbody2D> ();
	}

	void OnEnable() {
		body2d.velocity = velocity;
	}

	void OnDisable() {
		body2d.velocity = Vector2.zero;
	}
	
	void OnCollisionEnter2D(Collision2D collision) {

		GameObject other = collision.gameObject;
		
		if (other.tag == "Player") {
			
			ContactPoint2D[] contacts = collision.contacts;
			
			for (int i = 0; i < contacts.Length; i++) {
				
				if (contacts[i].normal.y == 1) {
					other.GetComponent<DestroyCrushed>().DestroyObject();
					break;
				}
			}
			return;
		}
		
		if (collision.gameObject.tag == "Box") {
			
			if (other.transform.position.x != this.transform.position.x)
				return;
			
			if (other.transform.position.y > this.transform.position.y)
				return;
			
			int thisWeight = GetComponent<BoxObject>().GetWeight();
			int otherWeight = other.GetComponent<BoxObject>().GetWeight ();
			
			if (thisWeight > otherWeight) {
				other.GetComponent<DestroyCrushed>().DestroyObject();
				body2d.velocity = velocity;
			} else {
				Grounded();
			}
			
			return;
		}
		
		if (other.gameObject.name == "Floor") {
			Grounded();
		}
	}

	private void Grounded() {
		if (GroundedCallback != null) {
			GroundedCallback ();
		}
	}
}
