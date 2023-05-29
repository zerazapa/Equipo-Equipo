using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
        public GameObject Transition;

    public void StartGame()
    {
        Transition.SetActive(true);
    }    
}