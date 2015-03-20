using UnityEngine;
using System.Collections;

public class Passenger : MonoBehaviour
{
	public GameObject Helicopter;
	public float GroundOffset;
	public float MinDistanceToPlayer;
	public float MaxDistanceToPlayer;
	public float GetInDistance;
	public float StopDistance;
	public float Speed;
	public LayerMask FloorLayers;
	public LayerMask BlockingLayers;
	Helicopter heliScript;

	// Use this for initialization
	void Start ()
	{
		heliScript = Helicopter.GetComponent<Helicopter> ();
		Walk ();
	}

	void Walk ()
	{
		// Cast downward
		RaycastHit2D hit = Physics2D.Raycast (transform.position, -Vector2.up, Mathf.Infinity, FloorLayers);
		if (hit.collider == null) {
			// We seem to be below ground
			Vector3 pos = transform.position;
			pos.y += 1000;
			transform.position = pos;
			Walk ();
		}

		Vector3 walkingDir = Vector3.right;
		if (Helicopter.transform.position.x < transform.position.x) {
			walkingDir = -Vector3.right;
		}
		Vector3 newPos = transform.position;
		newPos.y = hit.point.y + GroundOffset;

		// Cast in walking direction (we don't wanna hurt our head!)
		// Cast as far as our speed is so that we don't end up inside a prop
		hit = Physics2D.Raycast (transform.position, walkingDir, StopDistance, BlockingLayers);
		if (hit.collider == null) {
			newPos.x += walkingDir.x * Speed;
		}

		transform.position = newPos;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float xDistance = Mathf.Abs (Helicopter.transform.position.x - transform.position.x);
		if (heliScript.HasLanded 
			&& xDistance > MinDistanceToPlayer
			&& xDistance < MaxDistanceToPlayer) {
			if (xDistance < GetInDistance && heliScript.GetIn ()) {
				Destroy (gameObject);
			} else {
				Walk ();
			}
		}
	}
}
