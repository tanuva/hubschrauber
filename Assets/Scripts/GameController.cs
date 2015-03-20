using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	int _savedCount = 0;
	int _passengersInLevel;
	GameObject[] _airports;

	public Helicopter Player;
	public UIController UI;
	public string NextLevel;

	// Use this for initialization
	void Start ()
	{
		_passengersInLevel = GameObject.FindGameObjectsWithTag ("Passenger").Length;
		UI.PassengersInLevel = _passengersInLevel;
		_airports = GameObject.FindGameObjectsWithTag ("Airport");
		foreach (GameObject airportObject in _airports) {
			Airport airport = airportObject.GetComponent<Airport> ();
			airport.PassengersSaved += delegate(Airport sender, int passengersSaved) {
				_savedCount += passengersSaved;
				if (_savedCount >= _passengersInLevel) {
					Player.enabled = false;
					UI.ShowLevelDoneScreen (LoadNextLevel);
				}
			};
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void LoadNextLevel ()
	{
		// Well. Ugly, but perfectly fine for this game...
		Application.LoadLevel (NextLevel);
	}
}
