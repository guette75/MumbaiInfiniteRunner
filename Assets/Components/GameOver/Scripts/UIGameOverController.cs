using UnityEngine;

public class UIGameOverController : MonoBehaviour
{
    // Objet contenant l'écran de fin de jeu
    [SerializeField] private GameObject _gameOverScreen;
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Traitement exécuté au réveil de l'objet. Cette méthode est appelée en tout premier.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void Awake()
    {
        // Désactivation de l'écran de fin de jeu
        _gameOverScreen.SetActive(false);
        // Abonnement à l'évènement déclenché lors de la mise à jour du nombre de vies du joueur
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLife;
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois lorsque l'objet est détruit.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void OnDestroy()
    {
        // Désabonnement de l'évènement déclenché lors de la mise à jour du nombre de vies du joueur
        EventSystem.OnPlayerLifeUpdated -= HandlePlayerLife;
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Charge le menu principal lorsque le joueur est mort
    /// </summary>
    // -------------------------------------------------------------------------------
    public void LoadMainMenu()
    {
        // Chargement de la scène contenant le menu principal suite à la mort du joueur
        SceneLoaderService.loadMainMenu();
    }
    

    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée lorsque le nombre de vies du joueur a évolué (à la baisse)
    /// </summary>
    /// <param name="playerLifeCount"></param>
    // -------------------------------------------------------------------------------
    private void HandlePlayerLife(int playerLifeCount)
    {
        // Cas où le joueur possède encore des vies
        if (playerLifeCount > 0)
        {
            // On quitte la méthode
            return;
        }
        // Activation de l'écran de fin de jeu
        _gameOverScreen.SetActive(true);
    }
}
