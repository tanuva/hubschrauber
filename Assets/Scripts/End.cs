using UnityEngine;
using System.Collections;

public class End : MonoBehaviour
{
	public string MainMenuLevelName;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void MainMenu ()
	{
		Application.LoadLevel (MainMenuLevelName);
	}
}
