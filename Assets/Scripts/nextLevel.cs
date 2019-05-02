using UnityEngine;
using UnityEngine.SceneManagement;

public class nextLevel : MonoBehaviour {

	
	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D collider) {
        if (collider.CompareTag("Player"))
        {
			GoNextLevel();
        }      
    }

	public static void GoNextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		PlayerPrefs.SetInt("levelReached", PlayerPrefs.GetInt("levelReached") + 1);
	}
}
