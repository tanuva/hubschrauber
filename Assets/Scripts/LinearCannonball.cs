using UnityEngine;
using System.Collections;

public class LinearCannonball : MonoBehaviour
{
	public int MaxHeight = 10;

	// Use this for initialization
	void Start ()
	{
		if (MaxHeight < 0) {
			Debug.LogError ("Max cannonball height must be > 0");
		}
		GetComponent<Rigidbody2D> ().velocity = -Physics2D.gravity * Mathf.Sqrt (2 * MaxHeight / -Physics2D.gravity.y);
	}

	void FixedUpdate ()
	{
//		curVel += Physics2D.gravity * Time.fixedDeltaTime;
//		transform.position += new Vector3 (curVel.x, curVel.y, 0) * Time.fixedDeltaTime;
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		foreach (ContactPoint2D p in collision.contacts) {
			if (p.collider.gameObject.name == "Helicopter") {
				p.collider.gameObject.SendMessage ("OnCannonballImpact", SendMessageOptions.RequireReceiver);
			}
		}

		Destroy (gameObject);
	}
}
