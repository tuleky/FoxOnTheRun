using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

    public GameObject[] makeInvisible;
    public GameObject[] makeVisible;
    public GameObject menuImage;
    public GameObject levelMenu;
    public GameObject backButton;

    public Button[] levelButtons;


    private void Start()
    {

        int levelReached = PlayerPrefs.GetInt("levelReached", 1); 
        

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i+1 > levelReached)
                levelButtons[i].interactable = false;
        }
    }

    public void StopGame()
    {
        //oyunu durdur
        Time.timeScale = 0;

        //zıplama ve hareket etme butonlarının görünürlüğünü kapat
        for (int i = 0; i < makeInvisible.Length; i++)
        {
            makeInvisible[i].SetActive(false);
        }

        menuImage.SetActive(true);
        levelMenu.SetActive(false);
        backButton.SetActive(false);
    }

    public void LevelsMenu()
    {
        menuImage.SetActive(false);
        levelMenu.SetActive(true);
        backButton.SetActive(true);
    }

    public void ResumeGame()
    {
        //oyunu devam ettir
        Time.timeScale = 1;

        for (int i = 0; i < makeVisible.Length; i++)
        {
            makeVisible[i].SetActive(true);
        }

        menuImage.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }


    public void Select(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
		Time.timeScale = 1;
    }
}
