using UnityEngine;

public class GameState : State
{
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Constructeur.
    /// Le mot clé base signifie que le constructeur du père est appelé
    /// </summary>
    /// <param name="???"></param>
    // -------------------------------------------------------------------------------
    public GameState(StateMachine stateMachine) : base(stateMachine)
    {}
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode déclenchée lors de l'activation de cet état
    /// </summary>
    // -------------------------------------------------------------------------------
    public override void Enter()
    {
        // Abonnement à l'évènement relatif à la mise à jour du nombre de vies du joueur
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLifeUpdated;
        Debug.Log("Game started");
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode déclenchée lors de la désactivation de cet état
    /// </summary>
    // -------------------------------------------------------------------------------
    public override void Exit()
    {
        // Désabonnement de l'évènement déclenché lors de la mise à jour du nombre de vies du joueur
        EventSystem.OnPlayerLifeUpdated -= HandlePlayerLifeUpdated;
        Debug.Log("Game exited");
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode déclenchée lors de la mise à jour de cet état
    /// </summary>
    // -------------------------------------------------------------------------------
    public override void Update()
    {
        Debug.Log("Game updated");
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée lorsque le nombre de vies du joueur a évolué (à la baisse)
    /// </summary>
    /// <param name="playerLifeCount"></param>
    // -------------------------------------------------------------------------------
    private void HandlePlayerLifeUpdated(int playerLifeCount)
    {
        // Cas où le joueur possède encore des vies
        if (playerLifeCount > 0)
        {
            // On quitte la méthode car il n'y a rien à faire
            return;
        }
        // Création de l'état relatif à la fin du jeu
        GameOverState gameOverState = new GameOverState(StateMachine);
        // Changement de l'état du jeu
        StateMachine.ChangeState(gameOverState);
    }
}
