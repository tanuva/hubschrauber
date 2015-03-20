using UnityEngine;
using System.Collections;

public class CenteringCamera : MonoBehaviour
{
	// The target that is tracked by the camera
	public GameObject target;
	// The distance the camera keeps to the target (Z depth in 2D)
	public float distance;
	// Normally, the camera points right at the target. This specifies the
	// target's offset from the camera's center.
	public Vector2 offset;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (target != null) {
			gameObject.transform.position = target.transform.position 
				+ new Vector3 (0, 0, -distance)
				+ new Vector3 (offset.x, offset.y, 0);
		}
	}
}
