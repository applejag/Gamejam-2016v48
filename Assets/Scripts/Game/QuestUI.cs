using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class QuestUI : MonoBehaviour {

	public Text title;
	public Text description;

	void Start() {
		var rect = GetComponent<RectTransform>();
		rect.pivot = 
		rect.anchorMin =
		rect.anchorMax = Vector2.one * 0.5f;
		rect.localPosition = Vector3.zero;
		rect.sizeDelta = new Vector2(900, 500);
	}

	public void OnOKButton() {

	}

	public static void Create() {
		Instantiate(GameGlobals.questUI, GameGlobals.canvasRect);
	}

}
