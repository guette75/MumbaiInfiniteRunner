using System;
using TMPro;
using UnityEngine;

public class UITimeController : MonoBehaviour
{
    // Champ affiché dans l'UI contenant la valeur du chronomètre
    [SerializeField] private TMP_Text _timeText;
    
    // Booléen indiquant si l'état actuel correspond bien à l'exécution du jeu
    private bool _inGameState;
    // Référence à l'état représentant l'exécution actuelle du jeu
    private GameState _gameState;
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Traitement exécuté au réveil de l'objet. Cette méthode est appelée en tout premier.
    /// </summary>
    // -------------------------------------------------------------------------------
    public void Awake()
    {
        // Abonnement à l'évènement déclenché lors du changement d'état du jeu
        EventSystem.OnStateChanged += HandleStateChanged;
        // Désactive l'affichage du Timer. Celui-ci ne doit être affiché que lors de l'exécution du jeu
        _timeText.gameObject.SetActive(false);
    }
    

    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée à chaque exécution d'un frame
    /// Permet le déplacement des plans Unity à chaque exécution d'une nouvelle frame
    /// </summary>
    // -------------------------------------------------------------------------------
    void Update()
    {
        // Cas où le jeu n'est pas encore en cours d'exécution
        if (!_inGameState)
        {
            // On quitte la méthode, car il n'y a rien à faire
            return;
        }
        // Objet contenant le chronomètre au format date/heure afin de pouvoir l'afficher
        TimeSpan timeSpan = new TimeSpan(0, 0, Mathf.RoundToInt(_gameState.Timer));
        // Affichage du chronomètre dans le format mm:ss
        _timeText.text = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
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
    /// Méthode appelée lorsque l'état courant du jeu a changé
    /// </summary>
    /// <param name="newState"></param>
    // -------------------------------------------------------------------------------
    private void HandleStateChanged(State newState)
    {
        // Cas où l'état passé en paramètre ne correspond pas à un état relatif à l'exécution du jeu
        if (newState is not GameState gameState)
        {
            // Mise à jour du booléen indiquant que le jeu n'est pas en cours d'exécution
            _inGameState = false;
            // Désactive l'affichage du Timer car le jeu n'est pas en cours d'exécution
            _timeText.gameObject.SetActive(false);
            // On quitte la méthode
            return;
        }
        // Mise à jour du booléen indiquant que le jeu est bien en cours d'exécution
        _inGameState = true;
        // Affectation de l'état représentant l'exécution du jeu
        _gameState = gameState;
        // Active l'affichage du Timer car le jeu est en cours d'exécution
        _timeText.gameObject.SetActive(true);
    }
}
