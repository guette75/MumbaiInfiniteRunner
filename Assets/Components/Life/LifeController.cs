using System;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    // Nombre de vies du joueur au démarrage du jeu
    [SerializeField] private int _lifeCount = 3;
    
    // Nombre actuel de vies du joueur
    private int _currentLifeCount;
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois juste avant la première exécution de la méthode update (exécutée à chaque
    /// nouvelle frame), juste après la création de cette instance.
    /// </summary>
    // // -------------------------------------------------------------------------------
    void Start()
    {
        // Affectation du nombre de vies du joueur au démarrage du jeu
        _currentLifeCount = _lifeCount;
        // Mise à jour de l'évènement relatif au nombre de vies du joueur
        EventSystem.OnPlayerLifeUpdated?.Invoke(_currentLifeCount);
        // Abonnement à l'évènement généré par la collision du joueur avec un obstacle
        EventSystem.OnPlayerCollision += HandlePlayerCollision;
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois lorsque l'objet est détruit.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void OnDestroy()
    {
        // Désabonnement de l'évènement généré par la collision du joueur avec un obstacle
        EventSystem.OnPlayerCollision -= HandlePlayerCollision;
    }

    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée lors de la survenue d'une collision
    /// </summary>
    // -------------------------------------------------------------------------------
    private void HandlePlayerCollision()
    {
        // Cas où le joueur n'a plus de vie
        if (_currentLifeCount - 1 < 0)
        {
            // Le joueur est mort
            return;
        }
        // Mise à jour du nombre de vies
        _currentLifeCount--;
        // Appel de l'évènement relatif à la mise à jour du nombre de vies du joueur
        EventSystem.OnPlayerLifeUpdated?.Invoke(_currentLifeCount);
    }
}
