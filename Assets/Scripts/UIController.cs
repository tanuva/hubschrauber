using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;

public class UIController : MonoBehaviour
{
	public Helicopter Player;
	public Slider HealthSlider;
	public Transform LevelCompleteScreen;
	public Text SavedCountText;
	public int PassengersInLevel { set { _passengersInLevel = value; } }
	int _passengersInLevel;
	int _lastSavedCount = 0;
	
	// Use this for initialization
	void Start ()
	{
		Player.PassengerCountChanged += delegate(Helicopter sender, int passengerCount) {
			updateSavedCountText ();
		};
		GameObject[] airports = GameObject.FindGameObjectsWithTag ("Airport");
		foreach (GameObject airportObject in airports) {
			Airport airport = airportObject.GetComponent<Airport> ();
			airport.PassengersSaved += delegate(Airport sender, int passengersSaved) {
				_lastSavedCount += passengersSaved;
				updateSavedCountText ();
			};
		}
	}
	
	void updateSavedCountText ()
	{
		SavedCountText.text = string.Format ("Saved: {0}/{1}\nOnboard: {2}/{3}",
		                                     _lastSavedCount,
		                                     _passengersInLevel,
		                                     Player.PassengerCount,
		                                     Player.PassengerCapacity);
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Could cache this. Not necessary as we're not rendering text here
		// and the slider is drawn from scratch anyway.
		HealthSlider.value = Player.Health;
	}

	public void ShowLevelDoneScreen (UnityAction buttonHandler)
	{
		// Dynamic UI elements need to be prepared as prefabs.
		// (To preserve UI flexibility, they say. Might even be true!)
		var canvas = GameObject.Find ("Canvas");
		GameObject lcScreen = ((Transform)Instantiate (LevelCompleteScreen, canvas.transform.position, Quaternion.identity)).gameObject;
		lcScreen.transform.SetParent (canvas.transform);
		lcScreen.GetComponentInChildren<Button> ().onClick.AddListener (buttonHandler);
	}
}
