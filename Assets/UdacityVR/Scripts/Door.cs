using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour 
{
    // Create a boolean value called "locked" that can be checked in OnDoorClicked() 
    // Create a boolean value called "opening" that can be checked in Update() 
	private bool locked = true;
	private bool isOpening = false;
	public float raisedHeight;

    void Update() {
        // If the door is opening and it is not fully raised
            // Animate the door raising up
		if (isOpening && transform.position.y < raisedHeight) {
			
			transform.Translate (new Vector3 (0, 1f, 0) * Time.deltaTime);
		} else if (transform.position.y >= raisedHeight) {
			isOpening = false;
		}
    }

    public void OnDoorClicked() {
        // If the door is clicked and unlocked
            // Set the "opening" boolean to true
        // (optionally) Else
            // Play a sound to indicate the door is locked
		Debug.Log("Door Clicked");
		if (!locked) {
			isOpening = true;
		}
    }

    public void Unlock()
    {
        // You'll need to set "locked" to false here
		locked = false;
    }
}
