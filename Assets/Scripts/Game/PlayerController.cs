using UnityEngine;
using System.Collections;
using ExtensionMethods;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CameraController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {

	public bool repeatingJump;
	public float mouseXSpeed = 1;
	public float mouseYSpeed = 1;
	public float mouseIdleAfter = 2;
	public float moveAnimMultiplier = 1;

	public bool transforming { get; private set; }

	private CharacterMovement character;
	private CameraController _cam;
	private Animator anim;
	private float _turnAutoMoveSpeed;
	private float _mouseIdleTime;

	public PlayerTransformation current;
	private List<PlayerTransformation> allStates = new List<PlayerTransformation>();

	void Awake() {
		character = GetComponent<CharacterMovement> ();
		_cam = GetComponent<CameraController>();
		_turnAutoMoveSpeed = _cam.autoTurnMoveSpeed;
		anim = GetComponent<Animator>();
		allStates.AddRange(GetComponentsInChildren<PlayerTransformation>());
	}

	void Start() {
		GotoState(current);
	}

	void Update() {
		Vector3 axis = _cam.forward * Input.GetAxis("Vertical")
				+ _cam.right * Input.GetAxis("Horizontal");
		bool isMoving = axis.magnitude > float.Epsilon;

		// Move player
		if (isMoving)
			character.Move (axis.normalized);

		// Add mouse input to angles
		Vector2 mouse = new Vector2(Input.GetAxis("Mouse horizontal"), Input.GetAxis("Mouse vertical"));
		_cam.angle = Mathf.Clamp(_cam.angle + mouse.y * Time.deltaTime * mouseYSpeed, -10, 90);
		_cam.targetPlaneAngle += mouse.x * Time.deltaTime * mouseXSpeed;// + character.angleDelta;
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

		foreach (var state in allStates) {
			if (Input.GetKeyDown((KeyCode) (48 + (int)state.form)))
				GotoState(state);
		}

		// Tell animator
		if (current != null && current.anim != null) {

			current.anim.SetFloat("Speed", character.body.velocity.xz().magnitude * moveAnimMultiplier);
			current.anim.SetBool("Midair", !character.grounded);
			current.anim.SetBool("Walking", isMoving);

		}

		// Transforming transition
		if (transforming) {
			transformTime += Time.deltaTime * 0.5f;
			Transforming(transformTime);
		}
	}

	void FixedUpdate() {
		// Because rigidbody restrictions dont work 100%
		transform.eulerAngles = Vector3.up * transform.eulerAngles.y;
	}

	float transformTime = 0;
	void GotoState(PlayerTransformation state, bool force = false) {
		if (!transforming && (current != state || force)) {
			state.gameObject.SetActive(true);
			transforming = true;
			anim.SetTrigger("GotoState" + (int)state.form);
			current = state;
			transformTime = 0;
		}
	}
	
	void Transforming(float t) {
		_cam.distMultiplier = Mathf.Lerp(_cam.distMultiplier, current.distMultiplier, t);
		_cam.yOffset = Mathf.Lerp(_cam.yOffset, current.yOffset, t);
		character.moveSpeed = Mathf.Lerp(character.moveSpeed, current.moveSpeed, t);
		character.jumpForce = Mathf.Lerp(character.jumpForce, current.jumpForce, t);
		mouseXSpeed = Mathf.Lerp(mouseXSpeed, current.mouseXSpeed, t);
		mouseYSpeed = Mathf.Lerp(mouseYSpeed, current.mouseYSpeed, t);
		moveAnimMultiplier = Mathf.Lerp(moveAnimMultiplier, current.moveAnimMultiplier, t);
	}

	void TransformComplete() {
		transforming = false;

		// Disable all others
		foreach (var state in allStates)
			if (state != current)
				state.gameObject.SetActive(false);

		Transforming(1);
	}

}
