using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerInput : MonoBehaviour {

	private CharacterMovement character;

	void Awake() {
		character = GetComponent<CharacterMovement> ();
	}

	void Update() {
		Vector3 axis = Vector3.zero;
		if (Input.GetKey (KeyCode.W)) {
			axis.z += 1;
		}
		if (Input.GetKey (KeyCode.S)) {
			axis.z -= 1;
		}
		if (Input.GetKey (KeyCode.D)) {
			axis.x += 1;
		}
		if (Input.GetKey (KeyCode.A)) {
			axis.x -= 1;
		}

		if (axis.magnitude > float.Epsilon)
			character.Move (axis.normalized);
	}

}
