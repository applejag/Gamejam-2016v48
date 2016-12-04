using UnityEngine;
using System.Collections;
using System;

public class QuestFindNuts : Quest {

	public GameObject[] enableOnStart;

	public override void OnStart() {
		foreach (var go in enableOnStart)
			if (go != null)
				go.SetActive(true);
	}

}
