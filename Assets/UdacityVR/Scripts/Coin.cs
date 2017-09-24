using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour 
{
    //Create a reference to the CoinPoofPrefab
	public GameObject coinPoofPrefab;

    public void OnCoinClicked() {
        // Instantiate the CoinPoof Prefab where this coin is located
        // Make sure the poof animates vertically
        // Destroy this coin. Check the Unity documentation on how to use Destroy
		GameObject.Instantiate(coinPoofPrefab,transform.position, coinPoofPrefab.transform.rotation);
		PlayerPrefs.SetInt ("numCoins", PlayerPrefs.GetInt ("numCoins") + 1);
		Destroy (gameObject);
    }

	void Update(){
		//Not required, but for fun why not try adding a Key Floating Animation here :)
		transform.Rotate(new Vector3(1f,1f,0));
	}

}
