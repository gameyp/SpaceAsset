using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the next scene in the build index (assuming it's the game scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenSettings()
    {
        // Open the settings menu (assuming you have a UI canvas with a settings panel)
        // You can enable/disable the panel, or animate it on/off screen, etc.
        Debug.Log("Open settings menu");
    }

    public void QuitGame()
    {
        // Quit the application (only works in standalone builds, not in editor)
        Application.Quit();
    }
}