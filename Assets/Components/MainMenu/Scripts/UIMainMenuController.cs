using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Démarre le jeu.
    /// Lance la scène de démarrage.
    /// </summary>
    // -------------------------------------------------------------------------------
    public void startGame()
    {
        // Démarre le jeu
        SceneLoaderService.loadGame();
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelant la fermeture de l'application
    /// </summary>
    // -------------------------------------------------------------------------------
    public void QuitGame()
    {
        // Cas où le jeu ne s'exécute pas dans l'éditeur
        #if !UNITY_EDITOR
            // Quitte le jeu dans le mode normal (hors éditeur)
            Application.Quit();
        // Cas où le jeu s'exécute dans l'éditeur
        #else
            // quitte le jeu dans le mode éditeur
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
