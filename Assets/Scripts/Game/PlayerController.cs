using UnityEngine;
using System.Collections;
using ExtensionMethods;
using System;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CameraController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {

	public bool repeatingJump;
	public float mouseXSpeed = 1;
	public float mouseYSpeed = 1;
	public float mouseIdleAfter = 2;
	public Form currentForm = Form.Kiwi;

	public bool transforming { get; private set; }

	private CharacterMovement character;
	private CameraController _cam;
	private Animator anim;
	private float _turnAutoMoveSpeed;
	private float _mouseIdleTime;

	void Awake() {
		character = GetComponent<CharacterMovement> ();
		_cam = GetComponent<CameraController>();
		_turnAutoMoveSpeed = _cam.autoTurnMoveSpeed;
		anim = GetComponent<Animator>();
	}

	void Start() {
		GotoState(currentForm);
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

		if (repeatingJump ? Input.GetButton("Jump") : Input.GetButtonDown("Jump")) {
			character.Jump();
		}

		foreach (int x in Enum.GetValues(typeof(Form))) {
			if (Input.GetKeyDown((KeyCode) (48 + x)))
				GotoState((Form)x);
		}
	}

	void FixedUpdate() {
		// Because rigidbody restrictions dont work 100%
		transform.eulerAngles = Vector3.up * transform.eulerAngles.y;
	}

	void GotoState(Form form) {
		if (!transforming && currentForm != form) {
			transforming = true;
			anim.SetTrigger("GotoState" + (int)form);
			currentForm = form;
		}
	}
	
	void TransformComplete() {
		transforming = false;
	}

	public enum Form {
		Mus = 1,
		Kiwi,
		Elephant
	}

}
