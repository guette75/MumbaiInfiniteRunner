public abstract class State
{
    // Variable pointant vers la machine à états
    protected readonly StateMachine StateMachine;
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Constructeur.
    /// Le mot clé protected indique que tous les enfants de cette classe ont accès à ce constructeur
    /// </summary>
    /// <param name="???"></param>
    // -------------------------------------------------------------------------------
    protected State(StateMachine stateMachine)
    {
        // Affectation de la machine à état dans cet objet
        StateMachine = stateMachine;
    }
    
    
    // Déclaration abstraite de la méthode "Enter", relatif à l'arrivée dans un nouvel état.
    // Cette méthode doit être implémentée par les classes enfants
    public abstract void Enter();
    // Déclaration abstraite de la méthode "Exit", relatif à la sortie d'un état.
    // Cette méthode doit être implémentée par les classes enfants
    public abstract void Exit();
    // Déclaration abstraite de la méthode "Update", relatif à la mise à jour de l'état.
    // Cette méthode doit être implémentée par les classes enfants
    public abstract void Update();
}
