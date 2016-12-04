using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class PathfinderBuilder : SingletonBase<PathfinderBuilder> {

	public float rayCastHeight = 1;
	//public float boxCastWidth = 1;
	public float maxDistance = 30;
	public float sizeMultiplier = 1.1f;
	public float margin = 1;
	[Tag]
	public string obstacleTag;

	private List<PathPoint> _list = null;
	public static List<PathPoint> list { get { return instance._list; } }

#if UNITY_EDITOR
	public bool vizualize = false;
	void OnDrawGizmos() {
		if (!transform.IsSelected()) vizualize = false;

		if (!vizualize) {
			if (!Application.isPlaying)
				_list = null;
			return;
		}
		if (!Application.isPlaying) instance = this;
		if (_list == null) GeneratePathfinding();

		for (int i=0; i<_list.Count; i++) {
			PathPoint point = _list[i];
			Gizmos.color = Color.red;
			Gizmos.DrawRay(point.asVector3, Vector3.down * Mathf.Max(rayCastHeight,1));
			Gizmos.color = Color.cyan;
			foreach (var c in point.connections)
				Gizmos.DrawLine(point.asVector3, c.asVector3);
		}
	}
#endif

	protected override void Awake() {
		base.Awake();
		GeneratePathfinding();
	}

	// Most generic name evah'
	void GeneratePathfinding() {
		Debug.Log("Generating pathfinding... May take some seconds.");
		Time.timeScale = 0;
		_list = new List<PathPoint>();
		var buildings = new List<GameObject>(GameObject.FindGameObjectsWithTag(obstacleTag));

		// Find all points
		foreach (var go in buildings) {
			Bounds? bounds = null;

			// Teporarily enlarge building
			go.transform.localScale *= sizeMultiplier;

			foreach (var col in go.GetComponentsInChildren<Collider>()) {
				if (!bounds.HasValue) bounds = col.bounds;
				else bounds.Value.SetMinMax(Vector3.Min(bounds.Value.min, col.bounds.min), Vector3.Max(bounds.Value.max, col.bounds.max));
			}

			if (bounds.HasValue) {
				Vector3 max = bounds.Value.max;
				Vector3 min = bounds.Value.min;
				_list.Add(new PathPoint(max.x + margin, max.z + margin));
				_list.Add(new PathPoint(max.x + margin, min.z - margin));
				_list.Add(new PathPoint(min.x - margin, max.z + margin));
				_list.Add(new PathPoint(min.x - margin, min.z - margin));
			}
		}

		_list.RemoveAll(point => {
			// Look if inside any bound
			foreach (var go in GameObject.FindGameObjectsWithTag(obstacleTag)) {
				Bounds? bounds = null;

				foreach (var col in go.GetComponentsInChildren<Collider>()) {
					if (!bounds.HasValue) bounds = col.bounds;
					else bounds.Value.SetMinMax(Vector3.Min(bounds.Value.min, col.bounds.min), Vector3.Max(bounds.Value.max, col.bounds.max));
				}

				if (bounds.HasValue && bounds.Value.Contains(point.asVector3))
					return true;
			}

			// Look for connections
			point.connections.AddRange(_list.FindAll(other => {
				// Dont count self as other...
				if (point == other) return false;
				if (point.connections.Contains(other)) return false;

				// Dont include if cant raycast successfully
				Vector3 delta = other.asVector3 - point.asVector3;
				float distance = delta.magnitude;
				if (distance > maxDistance) return false;
				//return !Physics.BoxCast(VectorHelper.Average(point.asVector3, other.asVector3), new Vector3(boxCastWidth, distance, 0.5f), delta.normalized);
				if (Physics.Linecast(point.asVector3, other.asVector3, Physics.DefaultRaycastLayers))
					// Hit something, dont include it
					return false;
				else {
					// Didn't hit anything, add it to me and other
					other.connections.Add(point);
					return true;
				}
			}));

			// Remove if no connections found
			return point.connections.Count == 0;
		});


		foreach (var go in buildings) {
			// Teporarily enlarge building
			go.transform.localScale /= sizeMultiplier;
		}

			Time.timeScale = 1;
		Debug.Log("Generation complete!");
	}

	public class PathPoint {
		public readonly float x;
		public float y { get { return instance.rayCastHeight; } }
		public readonly float z;
		/// <summary>
		/// Note: Y values in vectors represent Z values.
		/// </summary>
		public List<PathPoint> connections = new List<PathPoint>();

		public PathPoint(float x, float z) {
			this.x = x;
			this.z = z;
		}

		private Vector3? _vector3 = null;
		public Vector3 asVector3 { get { return _vector3.HasValue ? _vector3.Value : (_vector3 = new Vector3(x, y, z)).Value; } }
	}

}
