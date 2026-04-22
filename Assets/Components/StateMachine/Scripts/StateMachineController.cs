using UnityEngine;

public class StateMachineController : MonoBehaviour
{
    // Instance contenant la machine à état
    private StateMachine  _stateMachine;
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois juste avant la première exécution de la méthode update (exécutée à chaque
    /// nouvelle frame), juste après la création de cette instance.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void Start()
    {
        // Initialisation de la machine à état
        _stateMachine = new StateMachine();
        // Création de l'état initial (compte à rebours)
        State initialState = new CountdownState(_stateMachine);
        // Affectation de cet état initial à la machine à états
        _stateMachine.ChangeState(initialState);
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Traitement à exécuter à chaque frame
    /// </summary>
    // -------------------------------------------------------------------------------
    private void Update()
    {
        // A chaque exécution d'une nouvelle frame, l'état est mis à jour
        _stateMachine.Update();
    }
}
