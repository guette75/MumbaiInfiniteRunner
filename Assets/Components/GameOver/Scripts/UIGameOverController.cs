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
        // Abonnement à l'évènement déclenché lors du changement d'état du jeu
        EventSystem.OnStateChanged += HandleStateChanged;
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois lorsque l'objet est détruit.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void OnDestroy()
    {
        // Désabonnement de l'évènement déclenché lors du changement d'état du jeu
        EventSystem.OnStateChanged -= HandleStateChanged;
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
    /// Méthode appelée lorsque l'état courant du jeu a changé
    /// </summary>
    /// <param name="newState"></param>
    // -------------------------------------------------------------------------------
    private void HandleStateChanged(State newState)
    {
        // Activation de l'écran affichant la fin du jeu si l'état passé en paramètre correspond à l'état de fin du jeu
        _gameOverScreen.SetActive(newState is GameOverState);
    }
}
