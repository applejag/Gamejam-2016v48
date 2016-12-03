using UnityEngine;
using System.Collections.Generic;
using ExtensionMethods;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour {

	const float _minGroundAngle = 10;

	public float jumpForce = 10;
	public float moveSpeed = 5;
	public float turnSpeed = 5;

	public bool grounded { get { return _ground.Count > 0; } }
	private HashSet<Collider> _ground = new HashSet<Collider> ();

	private Rigidbody body;
	private Vector3? forward = null;

	void Awake() {
		body = GetComponent<Rigidbody> ();
	}

	/// <summary>
	/// Make the character jump (if grounded).
	/// </summary>
	public void Jump() {
		if (grounded) {
			Vector3 vel = body.velocity;
			vel.y = jumpForce;
			body.velocity = vel;
		}
	}

	/// <summary>
	/// Move the character. To be run in normal updates.
	/// </summary>
	public void Move(Vector3 axis) {
		axis.y = 0;
		body.AddForce (axis * moveSpeed * Time.deltaTime);
		forward = forward.HasValue ? forward + axis : axis;
	}

	/// <summary>
	/// Move the character. To be run in fixed updates.
	/// </summary>
	public void MoveFixed(Vector3 axis) {
		axis.y = 0;
		body.AddForce (axis * moveSpeed * Time.fixedDeltaTime);
		forward = forward.HasValue ? forward + axis : axis;
	}

	void OnCollisionStay(Collision col) {
		bool isGround = false;

		// Check normals
		for (int i = 0; i < col.contacts.Length; i++) {
			if (Vector3.Angle (Vector3.up, col.contacts[i].normal) < _minGroundAngle) {
				isGround = true;
				break;
			}
		}

		if (isGround) _ground.Add (col.collider);
		else _ground.Remove (col.collider);
	}

	void OnCollisionExit(Collision col) {
		if (_ground.Contains (col.collider))
			_ground.Remove (col.collider);
	}

	void FixedUpdate() {
		if (forward.HasValue) {
			float current = transform.eulerAngles.y;
			float target = forward.Value.zx ().ToDegrees();
			body.rotation = (Quaternion.Euler(0,
				Mathf.MoveTowardsAngle(current, target, turnSpeed * Time.fixedDeltaTime),
			0));
			
			forward = null;
		}
	}
}
