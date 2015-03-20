using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
	public string NextLevel;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void buttonClicked (string id)
	{
		switch (id) {
		case "newGame":
			Application.LoadLevel (NextLevel);
			break;
		case "quit":
			Application.Quit ();
			break;
		}
	}
}
