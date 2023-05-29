using UnityEngine;
using UnityEngine.SceneManagement;


public class BacktoMenuTransition : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + -1);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        Debug.Log("Juego reanudado");
    }
}
