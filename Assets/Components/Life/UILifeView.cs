using TMPro;
using UnityEngine;

public class UILifeView : MonoBehaviour
{
    // Objet contenant le texte affichant le nombre de vies du joueur
    [SerializeField] private TMP_Text _lifeText;
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Traitement exécuté au réveil de l'objet. Cette méthode est appelée en tout premier.
    /// </summary>
    // -------------------------------------------------------------------------------
    void Awake()
    {
        // Abonnement à l'évènement déclenché lors de la mise à jour du nombre de vies du joueur
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLifeUpdated;
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois lorsque l'objet est détruit.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void OnDestroy()
    {
        // Désabonnement de l'évènement déclenché lors de la mise à jour du nombre de vies du joueur
        EventSystem.OnPlayerLifeUpdated -= HandlePlayerLifeUpdated;
    }

    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode mettant à jour le texte affichant le nombre de vies restant du joueur
    /// </summary>
    /// <param name="newLifeCount"></param>
    // -------------------------------------------------------------------------------
    private void HandlePlayerLifeUpdated(int newLifeCount)
    {
        _lifeText.text = "Lives: " + newLifeCount;
    }
    
}
