using UnityEngine;
using System.Collections;

public class ConstHeightCamera : MonoBehaviour
{
	public Transform Target;
	public float Height;
	public float Distance;
	public float limitLeft;
	public float limitRight;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Target != null
			&& Target.transform.position.x < limitRight
			&& Target.transform.position.x > limitLeft) {
			transform.position = new Vector3 (Target.transform.position.x, Height, -Distance);
		}
	}
	
	// Array of limits, 0 = upper, 1 = left, 2 = right
	public float[] GetViewportLimits ()
	{
		float[] limits = new float[4];
		limits [0] = GetComponent<Camera> ().ViewportToWorldPoint (new Vector3 (0, 1, Distance)).y;
		limits [1] = GetComponent<Camera> ().ViewportToWorldPoint (new Vector3 (0, 0, Distance)).y;
		limits [2] = limitLeft;
		limits [3] = limitRight;
		return limits;
	}
}
