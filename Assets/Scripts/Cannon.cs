using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Cannon : MonoBehaviour
{
	public int ShootingHeight;
	public int ShootInterval;
	public Vector3 RelativeBallOrigin;
	public Transform CannonballPrefab;

	// Use this for initialization
	void Start ()
	{
		Shoot ();
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void Delay (float waitTime, UnityAction act)
	{
		StartCoroutine (DelayImpl (waitTime, act));
	}
	
	IEnumerator DelayImpl (float waitTime, UnityAction act)
	{
		yield return new WaitForSeconds (waitTime);
		act ();
	}

	void Shoot ()
	{
		// Fire & forget - use plain and dumb cannonballs.
		// TODO Interface for cannonballs
		GameObject ballObject = ((Transform)Instantiate (CannonballPrefab, transform.position + RelativeBallOrigin, Quaternion.identity)).gameObject;
		LinearCannonball ball = ballObject.GetComponent<LinearCannonball> ();
		ball.MaxHeight = ShootingHeight;
		Delay (Random.value, () => {
			Invoke ("Shoot", ShootInterval); });
	}
}
