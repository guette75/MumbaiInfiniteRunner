using System;

// Classe unique dans tout le projet
public static class EventSystem
{
    // Evènement relatif à l'action correspondant à la flexion du joueur. Cette action renvoie un booléen
    // Action unique dans tout le projet
    public static Action<bool> OnPlayerSlideDown;
}
