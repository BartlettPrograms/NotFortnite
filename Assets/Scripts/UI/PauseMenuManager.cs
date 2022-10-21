using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    
    public void PauseButtonPressed()
    {
        if (pauseMenu.activeInHierarchy)
            pauseMenu.SetActive(false);
        else
            pauseMenu.SetActive(true);
    }
    
    public void ResetLevel()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
