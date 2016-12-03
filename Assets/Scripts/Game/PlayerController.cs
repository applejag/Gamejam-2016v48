using UnityEngine;
using System.Collections;
using ExtensionMethods;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CameraController))]
public class PlayerController : MonoBehaviour {

	public float mouseXSpeed = 1;
	public float mouseYSpeed = 1;
	public float mouseIdleAfter = 2;

	private CharacterMovement character;
	private CameraController _cam;
	private float _turnAutoMoveSpeed;
	private float _mouseIdleTime;

	void Awake() {
		character = GetComponent<CharacterMovement> ();
		_cam = GetComponent<CameraController>();
		_turnAutoMoveSpeed = _cam.autoTurnMoveSpeed;
	}

	void Update() {
		Vector3 axis = _cam.forward * Input.GetAxis("Vertical")
				+ _cam.right * Input.GetAxis("Horizontal");

		// Move player
		if (axis.magnitude > float.Epsilon)
			character.Move (axis.normalized);

		// Add mouse input to angles
		Vector2 mouse = new Vector2(Input.GetAxis("Mouse horizontal"), Input.GetAxis("Mouse vertical"));
		_cam.angle = Mathf.Clamp(_cam.angle + mouse.y * Time.deltaTime * mouseYSpeed, -10, 90);
		_cam.angleOffset += mouse.x * Time.deltaTime * mouseXSpeed + character.angleDelta;
		_cam.angleOffset %= 360;
		character.angleDelta = 0;
		_cam.targetDist = Mathf.Clamp(_cam.targetDist - Input.mouseScrollDelta.y, 5, 25);

		// Count how long the mouse has been idle
		if (mouse.magnitude <= float.Epsilon)
			_mouseIdleTime += Time.deltaTime;
		else _mouseIdleTime = 0;

		// Set the auto turn speed depending on idle time
		_cam.autoTurnMoveSpeed = _turnAutoMoveSpeed * Mathf.Clamp01(_mouseIdleTime - mouseIdleAfter);
	}

	void FixedUpdate() {
		// Because rigidbody restrictions dont work 100%
		transform.eulerAngles = Vector3.up * transform.eulerAngles.y;
	}

}
