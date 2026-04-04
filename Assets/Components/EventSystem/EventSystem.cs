using System;

// Classe unique dans tout le projet
public static class EventSystem
{
    // Evènement relatif à l'action correspondant à la flexion du joueur. Cette action renvoie un booléen
    // Action unique dans tout le projet
    public static Action<bool> OnPlayerSlideDown;
    // Evènement relatif à la collision d'un obstacle par le joueur
    public static Action OnPlayerCollision;
    // Evènement relatif à la mise à jour du nombre de vies du joueur
    public static Action<int> OnPlayerLifeUpdated;
}
