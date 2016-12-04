using UnityEngine;
using System.Collections;

public class Pickupcoin : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter()
    {
        // Currency object shall disappear
        // Update to global currency script as increase of currency
        // eventually display the change in the UI
        GameObject.Destroy(this);
    }
   
}
