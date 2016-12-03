using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class CameraController : MonoBehaviour {

	[Range(-10,90)]
	public float angle = 45;
	public float angleOffset;
	public float turnLerpSpeed = 5;
	public float tiltLerpSpeed = 2;
	public float autoTurnMoveSpeed = 20;
	[System.NonSerialized]
	[Header("Raycasting")]
	public float targetDist = 10;
	[Range(0,1)]
	public float hitExtraMult = .5f;
	public LayerMask raycastLayer;

	
	public float planeAngle { get; private set; }
	public float tiltAngle { get; private set; }
	public float dist { get; private set; }

	public Vector3 forward { get { return (planeAngle).FromDegrees().yzx(0); } }
	public Vector3 back { get { return (planeAngle+180).FromDegrees().yzx(0); } }
	public Vector3 left { get { return (planeAngle+270).FromDegrees().yzx(0); } }
	public Vector3 right { get { return (planeAngle+90).FromDegrees().yzx(0); } }

	void Start() {
		if (Camera.main == null) {
			Debug.LogError ("No camera found! Please asign a camera with the \"MainCamera\" tag!");
			return;
		}

		planeAngle = transform.eulerAngles.y;
		tiltAngle = angle;
	}

	void LateUpdate() {
		if (Camera.main == null) {
			enabled = false;
			return;
		}

		// First calculate angles
		angleOffset = Mathf.MoveTowardsAngle(angleOffset, 0, autoTurnMoveSpeed * Time.deltaTime);
		float targetAngle = transform.eulerAngles.y + angleOffset;
		planeAngle = Mathf.LerpAngle (planeAngle, targetAngle, turnLerpSpeed * Time.deltaTime);
		tiltAngle = Mathf.LerpAngle(tiltAngle, angle, tiltLerpSpeed * Time.deltaTime);
		Camera.main.transform.eulerAngles = new Vector3(tiltAngle, planeAngle, 0);

		// Then set the distance
		dist = Mathf.Lerp(dist, targetDist, Time.deltaTime);
		var extra = hitExtraMult * Mathf.Clamp01(Mathf.InverseLerp(80, -10, tiltAngle));

		// Lastly raycastin'
		RaycastHit hit;
		Ray ray = new Ray(transform.position, -Camera.main.transform.forward);
		if (Physics.Raycast(ray, out hit, targetDist * (1+extra), raycastLayer)) {
			Camera.main.transform.position = hit.point - ray.direction * extra * targetDist;
		} else {
			Camera.main.transform.position = transform.position - Camera.main.transform.forward * targetDist;
		}

	}

}
