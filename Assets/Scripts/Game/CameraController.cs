using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class CameraController : MonoBehaviour {

	[Range(-10,90)]
	public float angle = 45;
	public float yOffset = 1;
	public float turnLerpSpeed = 5;
	public float tiltLerpSpeed = 2;
	public float autoTurnMoveSpeed = 20;
	[System.NonSerialized]
	[Header("Raycasting")]
	public float targetDist = 10;
	public float distMultiplier = 1;
	[Range(0,1)]
	public float hitExtraMult = .5f;
	public LayerMask raycastLayer;
	
	public float planeAngle { get; private set; }
	public float tiltAngle { get; private set; }
	public float dist { get; private set; }
	public float currYOffset { get; private set; }

	public Vector3 forward { get { return (planeAngle).FromDegrees().yzx(0); } }
	public Vector3 back { get { return (planeAngle+180).FromDegrees().yzx(0); } }
	public Vector3 left { get { return (planeAngle+270).FromDegrees().yzx(0); } }
	public Vector3 right { get { return (planeAngle+90).FromDegrees().yzx(0); } }

	public Vector3 pivotPos { get { return transform.position + Vector3.up * currYOffset; } }
	public float targetPlaneAngle { get; set; }

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
		planeAngle = Mathf.LerpAngle (planeAngle, targetPlaneAngle, turnLerpSpeed * Time.deltaTime) % 360;
		tiltAngle = Mathf.LerpAngle(tiltAngle, angle, tiltLerpSpeed * Time.deltaTime) % 360;
		Camera.main.transform.eulerAngles = new Vector3(tiltAngle, planeAngle, 0);

		// Then set the distance
		dist = Mathf.Lerp(dist, targetDist * distMultiplier, Time.deltaTime);
		var extra = hitExtraMult * Mathf.Clamp01(Mathf.InverseLerp(80, -10, tiltAngle));
		currYOffset = Mathf.Lerp(currYOffset, yOffset, Time.deltaTime);

		// Lastly raycastin'
		RaycastHit hit;
		Ray ray = new Ray(pivotPos, -Camera.main.transform.forward);
		if (Physics.Raycast(ray, out hit, targetDist * (1+extra) * distMultiplier, raycastLayer)) {
			Camera.main.transform.position = hit.point - ray.direction * extra * targetDist * distMultiplier;
		} else {
			Camera.main.transform.position = pivotPos - Camera.main.transform.forward * targetDist * distMultiplier;
		}

	}

}
