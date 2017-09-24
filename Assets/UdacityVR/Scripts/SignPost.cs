using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SignPost : MonoBehaviour
{	
	public Text textController;
	public void ResetScene() 
	{
        // Reset the scene when the user clicks the sign post
		SceneManager.LoadScene("A Maze");
	}

	public void Start(){
		textController = GetComponent<Text> ();

	}

	public void Update(){
		textController.text = string.Format ("YOU WIN\n({0} coins)", PlayerPrefs.GetInt ("numCoins"));
	}
}