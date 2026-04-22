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
        Debug.Log("Game started");
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode déclenchée lors de la désactivation de cet état
    /// </summary>
    // -------------------------------------------------------------------------------
    public override void Exit()
    {
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
}
