using UnityEngine;

public class CountdownState : State
{
    // Durée initiale du compte à rebours
    private float _initialTime = 3f;
    // Compte à rebours en cours d'exécution
    private float _timer;
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Retourne le compte à rebours en cours d'exécution
    /// </summary>
    /// // -------------------------------------------------------------------------------
    public float Timer => _timer;
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Constructeur.
    /// Le mot clé base signifie que le constructeur du père est appelé
    /// </summary>
    /// <param name="???"></param>
    // -------------------------------------------------------------------------------
    public CountdownState(StateMachine stateMachine) : base(stateMachine)
    {}
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode déclenchée lors de l'activation de cet état
    /// </summary>
    // -------------------------------------------------------------------------------
    public override void Enter()
    {
        Debug.Log("Countdown started");
        // Initialisation du compte à rebours actuel
        _timer = _initialTime;
    }

    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode déclenchée lors de la désactivation de cet état
    /// </summary>
    // -------------------------------------------------------------------------------
    public override void Exit()
    {
        Debug.Log("Countdown exited");
    }

    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode déclenchée lors de la mise à jour de cet état
    /// </summary>
    // -------------------------------------------------------------------------------
    public override void Update()
    {
        Debug.Log("Countdown updated");
        // Mise à jour du compte à rebours actuel
        _timer -= Time.deltaTime;
        // Cas où le compte à rebours actuel n'est pas terminé
        if (_timer > 0f)
        {
            // On quitte la méthode
            return;
        }
        // Activation de l'évènement syivant (le jeu)
        State gameState = new GameState(StateMachine);
        // Affectation du nouvel état dans la machine à états
        StateMachine.ChangeState(gameState);
    }
}
