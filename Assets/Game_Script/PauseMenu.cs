using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
}