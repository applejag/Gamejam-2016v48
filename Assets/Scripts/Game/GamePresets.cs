using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public sealed class GamePresets : SingletonBase<GamePresets> {

	[Header("Existing objects")]
	public Canvas _canvas;
	private RectTransform _canvasRect;

	// Existing objects
	public static Canvas canvas { get { return instance._canvas; } }
	public static RectTransform canvasRect { get { return instance._canvasRect; } }

	protected override void Awake() {
		base.Awake();
		_canvasRect = _canvas.GetComponent<RectTransform>();
	}
}
