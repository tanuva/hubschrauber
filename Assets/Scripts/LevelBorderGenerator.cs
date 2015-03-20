using UnityEngine;
using System.Collections;

public class LevelBorderGenerator : MonoBehaviour
{
	public ConstHeightCamera CameraScript;

	// Use this for initialization
	void Start ()
	{
		// Generate level bounds
		float[] limits = CameraScript.GetViewportLimits ();
		EdgeCollider2D[] colliders = GetComponents<EdgeCollider2D> ();
		// Upper collider
		Vector2[] points = new Vector2[2];
		points [0] = new Vector2 (limits [2], limits [0]);
		points [1] = new Vector2 (limits [3], limits [0]);
		colliders [0].points = points;
		// Lower
		points [0] = new Vector2 (limits [2], limits [1]);
		points [1] = new Vector2 (limits [3], limits [1]);
		colliders [1].points = points;
		// Left
		points [0] = new Vector2 (limits [2], limits [0]);
		points [1] = new Vector2 (limits [2], limits [1]);
		colliders [2].points = points;
		// Right 
		points [0] = new Vector2 (limits [3], limits [0]);
		points [1] = new Vector2 (limits [3], limits [1]);
		colliders [3].points = points;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
