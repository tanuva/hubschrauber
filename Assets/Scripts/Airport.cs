using UnityEngine;
using System.Collections;

public class Airport : MonoBehaviour
{
	BoxCollider2D _landingTrigger;

	public delegate void PassengersSavedDelegate (Airport sender,int passengersSaved);
	public event PassengersSavedDelegate PassengersSaved;

	// Use this for initialization
	void Start ()
	{
		_landingTrigger = gameObject.GetComponent<BoxCollider2D> ();
		if (!_landingTrigger.isTrigger) {
			Debug.Log ("Warning: GameController has non-trigger collider as _landingTrigger");
		}
	}
	
	public void savePassengers (int passengerCount)
	{
		PassengersSaved (this, passengerCount);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
