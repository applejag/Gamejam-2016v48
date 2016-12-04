using UnityEngine;
using System.Collections;
using ExtensionMethods;

[RequireComponent(typeof(CharacterMovement))]
public class PedestrianAI : MonoBehaviour {
	
	public float distanceThreshhold = 2;
	public Animator anim;

	private CharacterMovement character;
	private PathfinderBuilder.PathPoint targetPathPoint;
	private Vector3 target;
	private float sleep;

#if UNITY_EDITOR
	public bool vizualize = false;
	void OnDrawGizmos() {
		if (!vizualize || targetPathPoint == null) {
			return;
		}

		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, target);
	}
#endif

	void Awake() {
		character = GetComponent<CharacterMovement>();
	}

	void Start() {
		NewPathTarget();
	}

	void Update() {
		anim.SetBool("Walking", sleep <= float.Epsilon);
		if (sleep > 0) {
			sleep -= Time.deltaTime;
		} else {
			Vector3 delta = target - transform.position;
			character.Move(delta.normalized);
			if (delta.magnitude < distanceThreshhold) {
				NewPathTarget();
				sleep = Random.Range(-3, 4);
			}

			anim.SetFloat("Speed", character.body.velocity.xz().magnitude);
		}
	}

	void NewPathTarget() {
		if (targetPathPoint == null) {
			// Take nearest one
			PathfinderBuilder.PathPoint closest = null;
			float closestDist = 0;
			PathfinderBuilder.list.ForEach(point => {
				float dist = Vector2.Distance(transform.position.xz(), point.asVector3.xz());
				if (closest == null || dist < closestDist) {
					closest = point;
					closestDist = dist;
				}
			});

			targetPathPoint = closest;
		} else {
			targetPathPoint = targetPathPoint.connections.GetRandom();
		}

		target = targetPathPoint.asVector3.SetY(transform.position.y);
	}

}
