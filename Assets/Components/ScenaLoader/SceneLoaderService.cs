using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoaderService
{
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Charge la scène principale contenant le démarrage du jeu
    /// </summary>
    // -------------------------------------------------------------------------------
    public static void loadGame()
    {
        Debug.Log("Loading game...");
        // Chargement de la scène "Level"
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
        // Chargement de la scène "LevelUI" en additif
        SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
        Debug.Log("Game loaded");
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Charge la scène contenant le menu de démarrage
    /// </summary>
    // -------------------------------------------------------------------------------
    public static void loadMainMenu()
    {
        Debug.Log("Loading main menu...");
        // Chargement de la scène "MainMenu"
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Debug.Log("Main menu loaded");
    }
}
