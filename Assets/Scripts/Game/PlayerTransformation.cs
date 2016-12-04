using UnityEngine;
using System.Collections;

public class PlayerTransformation : MonoBehaviour {

	public Form form;
	public Animator anim;
	[Header("Variables")]
	public float distMultiplier = 1;
	public float yOffset = 4;
	public float jumpForce = 20;
	public float moveSpeed = 3500;
	public float mouseXSpeed = 4;
	public float mouseYSpeed = -3;

	public enum Form {
		Mus = 1,
		Kiwi,
		Elephant
	}
}
