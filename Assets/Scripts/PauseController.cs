using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public void PauseGame()
    {
        Time.timeScale = 0;
        Debug.Log("Juego pausado");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Debug.Log("Juego reanudado");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego");
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
        Debug.Log("Cargando " + levelName);
    }
}
