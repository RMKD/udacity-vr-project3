using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour 
{
    //Create a reference to the KeyPoofPrefab and Door
	public GameObject keyPoofPrefab;
	public Door door;

	void Update()
	{
		//Not required, but for fun why not try adding a Key Floating Animation here :)
		transform.position = new Vector3(
			transform.position.x,
			1.5f + Mathf.Sin(Time.timeSinceLevelLoad),
			transform.position.z
		);
	}

	public void OnKeyClicked()
	{
        // Instatiate the KeyPoof Prefab where this key is located
        // Make sure the poof animates vertically
        // Call the Unlock() method on the Door
        // Set the Key Collected Variable to true
        // Destroy the key. Check the Unity documentation on how to use Destroy
		GameObject.Instantiate(keyPoofPrefab,transform.position, keyPoofPrefab.transform.rotation);
		Destroy (gameObject);
		// Since the key may generated dynamically, search the scene for a Door object
		GameObject.FindObjectOfType<Door> ().Unlock ();
    }

}
