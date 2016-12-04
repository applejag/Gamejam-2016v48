using UnityEngine;
using System.Collections;

public class TowerGoal : MonoBehaviour {

	public int bitsNeeded = 1;

	public bool visible = false;

	void Update() {
		visible = !Physics.Linecast(Camera.main.transform.position, transform.position);
	}

}
