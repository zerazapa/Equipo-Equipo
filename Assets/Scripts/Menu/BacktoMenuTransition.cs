using UnityEngine;
using UnityEngine.SceneManagement;


public class BacktoMenuTransition : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        Debug.Log("Juego reanudado");
    }
}
