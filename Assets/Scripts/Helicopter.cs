using UnityEngine;
using System.Collections.Generic;

public class Helicopter : MonoBehaviour
{
	float _curHorzAccel = 0;
	bool _hasLanded = false;
	int _passengerCount;

	public Vector3 Speed = new Vector3 (0, 0, 0);
	public float VertAccel = .1f;
	public float HorzAccel = .1f;
	public float VertMax = 1f;
	public float HorzMax = 1f;
	public float Slowdown = 1f;
	public int AccelerationPitch = 30;
	public float Health = 1.0f;
	public bool HasLanded {
		get { return _hasLanded; }
	}
	public int PassengerCount {
		get { return _passengerCount; }
		set {
			_passengerCount = value;
			PassengerCountChanged (this, _passengerCount); 
		}
	}
	public int PassengerCapacity;
	List<Airport> _airports = new List<Airport> (5);
	Dictionary<KeyCode, bool> keyDown = new Dictionary<KeyCode, bool> (4);

	public delegate void PassengerCountChangedDelegate (Helicopter sender,int passengerCount);
	public event PassengerCountChangedDelegate PassengerCountChanged;

	// Use this for initialization
	void Start ()
	{
		if (PassengerCapacity < 0) {
			Debug.Log ("Warning: Passenger capacity is set < 0. Defaulting to 0.");
		}
		
		GameObject[] airports = GameObject.FindGameObjectsWithTag ("Airport");
		foreach (GameObject airportObject in airports) {
			Airport airport = airportObject.GetComponent<Airport> ();
			_airports.Add (airport);
		}
	}

	void CheckInput ()
	{
		// Copying key presses into defined locations in keyDown allows us
		// to change the key mapping easily without mixing up anything else.
		// TODO controller input
		keyDown [KeyCode.UpArrow] = Input.GetKey (KeyCode.UpArrow);
		keyDown [KeyCode.DownArrow] = Input.GetKey (KeyCode.DownArrow);
		keyDown [KeyCode.LeftArrow] = Input.GetKey (KeyCode.LeftArrow);
		keyDown [KeyCode.RightArrow] = Input.GetKey (KeyCode.RightArrow);
	}

	public bool GetIn ()
	{
		// Data distribution between Helicopter and GameController is kinda ugly.
		if (PassengerCount < PassengerCapacity) {
			PassengerCount++;
			return true;
		}
		return false;
	}

	public void DropOff (Airport airport)
	{
		airport.savePassengers (_passengerCount);
		PassengerCount = 0;
	}

	void ProcessMovementKeys ()
	{
		if (keyDown [KeyCode.UpArrow]) {
			Speed.y += VertAccel;
		} 
		if (keyDown [KeyCode.DownArrow]) {
			Speed.y -= VertAccel;
		}
		if (keyDown [KeyCode.LeftArrow]) {
			Speed.x -= HorzAccel;
			_curHorzAccel = -HorzAccel;
		}
		if (keyDown [KeyCode.RightArrow]) {
			Speed.x += HorzAccel;
			_curHorzAccel = HorzAccel;
		}
		if (!keyDown [KeyCode.RightArrow] && !keyDown [KeyCode.LeftArrow]) {
			_curHorzAccel = 0;
		}
	}

	void ApplyDrag ()
	{
		if (Speed.y > 0) {
			var newYSpeed = Speed.y - VertAccel * Slowdown * (keyDown [KeyCode.UpArrow] ? 0 : 1);
			Speed.y = Mathf.Max (newYSpeed, 0);
			
		} else if (Speed.y < 0) {
			var newYSpeed = Speed.y + VertAccel * Slowdown * (keyDown [KeyCode.DownArrow] ? 0 : 1);
			Speed.y = Mathf.Min (newYSpeed, 0);
		}
		if (Speed.x > 0) {
			var newXSpeed = Speed.x - HorzAccel * Slowdown * (keyDown [KeyCode.RightArrow] ? 0 : 1);
			Speed.x = Mathf.Max (newXSpeed, 0);
			
		} else if (Speed.x < 0) {
			var newXSpeed = Speed.x + HorzAccel * Slowdown * (keyDown [KeyCode.LeftArrow] ? 0 : 1);
			Speed.x = Mathf.Min (newXSpeed, 0);
		}
	}

	void EnforceSpeedLimit ()
	{
		if (Speed.y > 0) {
			Speed.y = Mathf.Min (Speed.y, VertMax);
			
		} else if (Speed.y < 0) {
			Speed.y = Mathf.Max (Speed.y, -VertMax);
		}
		if (Speed.x > 0) {
			Speed.x = Mathf.Min (Speed.x, HorzMax);
			
		} else if (Speed.x < 0) {
			Speed.x = Mathf.Max (Speed.x, -HorzMax);
		}
	}

	void Animate ()
	{
		Vector3 rotAxis = new Vector3 (0, 0, 1);

		if (_curHorzAccel > 0) {
			transform.localEulerAngles = rotAxis * -AccelerationPitch;
		} else if (_curHorzAccel < 0) {
			transform.localEulerAngles = rotAxis * AccelerationPitch;
		} else {
			transform.localEulerAngles = rotAxis * 0;
		}
	}

	void UpdateHealth (float dHealth)
	{
		Health += dHealth;

		if (Health <= 0) {
			Debug.Log ("Game Over!");
			Destroy (gameObject);
		}
	}

	// FixedUpdate is called once per physics iteration
	void FixedUpdate ()
	{
		EnforceSpeedLimit ();
		ApplyDrag ();
		gameObject.transform.position += Speed * Time.fixedDeltaTime;
	}

	// Update is called once per frame
	void Update ()
	{
		CheckInput ();
		ProcessMovementKeys ();
		Animate ();
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Airport" && PassengerCount > 0) {
			foreach (Airport airport in _airports) {
				if (airport.gameObject == coll.gameObject) {
					DropOff (airport);
				}
			}
		}
	}
	
	void OnCollisionEnter2D (Collision2D coll)
	{
		foreach (ContactPoint2D p in coll.contacts) {
			if (p.collider.name == "Floor Sprite") {
				_hasLanded = true;
			} else if (p.collider.name.StartsWith ("Cannonball")) {
				GetComponent<Rigidbody2D> ().isKinematic = true;
			}

			// Stop movement in direction of collision
			if (p.normal == Vector2.up) {
				Speed.y = Mathf.Max (Speed.y, 0f);
			} else if (p.normal == -Vector2.up) {
				Speed.y = Mathf.Min (Speed.y, 0f);
			} else if (p.normal == Vector2.right) {
				Speed.x = Mathf.Max (Speed.x, 0f);
			} else if (p.normal == -Vector2.right) {
				Speed.x = Mathf.Min (Speed.x, 0f);
			}
		}
	}

	void OnCollisionExit2D (Collision2D coll)
	{
		foreach (ContactPoint2D p in coll.contacts) {
			if (p.collider.name == "Floor Sprite") {
				_hasLanded = false;
			}
		}
	}

	void OnCollisionStay2D (Collision2D coll)
	{
		foreach (ContactPoint2D p in coll.contacts) {
			Debug.DrawLine (p.point, p.point + p.normal);
		}
	}

	void OnCannonballImpact ()
	{
		UpdateHealth (-0.2f);
		GetComponent<Rigidbody2D> ().isKinematic = false;
	}
}
