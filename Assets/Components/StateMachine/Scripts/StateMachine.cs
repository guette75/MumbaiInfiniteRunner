using UnityEngine;

public class StateMachine
{
    // Etat courant
    public State CurrentState;
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Modifie l'état courant
    /// </summary>
    /// <param name="newState"></param>
    // -------------------------------------------------------------------------------
    public void ChangeState(State newState)
    {
        Debug.Log("Changing state from : " + CurrentState?.GetType().Name + " to : " + newState.GetType().Name);
        // Exécution de la méthode de sortie de l'état courant avant le passage à un nouvel état
        CurrentState?.Exit();
        // Mise à jour de l'état courant
        CurrentState = newState;
        // Exécute le traitement associé à l'état courant
        CurrentState.Enter();
        // Déclenchement de l'évènement indiquant que l'état courant du jeu a changé
        EventSystem.OnStateChanged?.Invoke(CurrentState);
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode mettant à jour l'état courant
    /// </summary>
    // -------------------------------------------------------------------------------
    public void Update()
    {
        CurrentState?.Update();
    }
    
}
