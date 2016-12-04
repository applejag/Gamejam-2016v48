using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class QuestProvider : MonoBehaviour {
	private Quest quest;
	private QuestUI ui;

	void Awake() {
		quest = GetComponent<Quest>();
		if (quest == null) Debug.LogError("No quest found! Please assign a quest to this gameobject! (" + transform.GetPath() + ")");
	}

	void Update() {

	}
}

[System.Serializable]
public abstract class Quest : MonoBehaviour {

	[System.NonSerialized]
	public QuestProvider provider;
	public string title;
	[TextArea]
	public string description;

	public abstract void OnStart();

	public void Finish() {

	}

}
