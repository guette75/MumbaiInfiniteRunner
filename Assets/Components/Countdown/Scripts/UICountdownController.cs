using System;
using TMPro;
using UnityEngine;

public class UICountdownController : MonoBehaviour
{
    // Objet contenant le texte permettant d'afficher le compte à rebours
    [SerializeField] private TMP_Text _countdownText;
    // Fenêtre contenant le texte affichant la valeur du compte à rebours
    [SerializeField] private GameObject _window;
    
    // Booléen indiquant si nous sommes dans le compte à rebours
    private bool _inCountdown;
    // Objet contenant l'état relatif au compte à rebours
    private CountdownState _countdownState;
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Traitement exécuté au réveil de l'objet. Cette méthode est appelée en tout premier.
    /// </summary>
    // -------------------------------------------------------------------------------
    public void Awake()
    {
        // Désactivation de la fenêtre contenant le texte affichant le compte à rebours
        _window.SetActive(false);
        // Abonnement à l'évènement déclenché lors du changement d'état dans la machine à états du jeu
        EventSystem.OnStateChanged += HandleStateChanged;
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois lorsque l'objet est détruit.
    /// </summary>
    // -------------------------------------------------------------------------------
    public void OnDestroy()
    {
        // Désabonnement de l'évènement déclenché lors du changement d'état dans la machine à états du jeu
        EventSystem.OnStateChanged -= HandleStateChanged;
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Traitement à exécuter à chaque frame
    /// </summary>
    // -------------------------------------------------------------------------------
    private void Update()
    {
        // Cas où le compte à rebours n'est pas en cours d'exécution
        if (!_inCountdown)
        {
            // On quitte la méthode
            return;
        }
        // Affichage de la valeur du compte à rebours
        _countdownText.text = _countdownState.Timer.ToString(format:"0");
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée lorsque l'état courant du jeu a changé
    /// </summary>
    /// <param name="state"></param>
    // -------------------------------------------------------------------------------
    private void HandleStateChanged(State state)
    {
        // Cas où l'état passé en paramètre ne correspond pas à un état relatif au compte à rebours
        if (state is not CountdownState countdownState)
        {
            // Mise à jour du booléen indiquant que nous ne sommes pas dans le compte à rebours
            _inCountdown = false;
            // Désactivation de la fenêtre contenant le texte affichant le compte à rebours
            _window.SetActive(false);
            // On quitte la méthode
            return;
        }
        // Mise à jour du booléen indiquant que nous sommes dans le compte à rebours
        _inCountdown = true;
        // Activation de la fenêtre contenant le texte affichant le compte à rebours
        _window.SetActive(true);
        // Affectation de l'état relatif à l'exécution du compte à rebours
        _countdownState = countdownState;
    }
}
