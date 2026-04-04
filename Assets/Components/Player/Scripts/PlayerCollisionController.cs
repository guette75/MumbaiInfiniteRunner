using System;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    [Header("Parameters")]
    // Position relative au centre de la sphère reliée au personnage pour la gestion des collisions
    [SerializeField] private Vector3 _sphereCenter;
    // Rayon de la sphère reliée au personnage pour la gestion des collisions
    [SerializeField] private float _sphereRadius;
    
    // Booléen indiquant qu'un obstacle a été percuté
    private bool _isHit;
    
    // Position de la sphère de détection des collisions du joueur (initialisée à la position du joueur)
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Retourne la position de la sphère de détection des collisions du joueur
    /// Ici, la position du joueur + la position de la sphère par rapport au joueur
    /// </summary>
    /// // -------------------------------------------------------------------------------
    private Vector3 PlayerSpherePosition => transform.position + _sphereCenter;


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois juste avant la première exécution de la méthode update (exécutée à chaque
    /// nouvelle frame), juste après la création de cette instance.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void Start()
    {
        // Mise en place de l'écoute de l'évènement relatif à la flexion du joueur et appel de la méthode associée
        // lors du déclenchement de cet évènement
        EventSystem.OnPlayerSlideDown += ShrinkCollider;
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée à chaque exécution d'un frame
    /// Permet de vérifier les collisions entre le joueur et les obstacles
    /// </summary>
    // -------------------------------------------------------------------------------
    private void Update()
    {
        // Récupération des zones de collision contenues dans la sphère virtuelle entourant le joueur
        Collider[] hitColliders = Physics.OverlapSphere(PlayerSpherePosition, _sphereRadius);
        // Cas où au moins une zone de collision est contenue dans cette sphère virtuelle
        // (le joueur a heurté un obstacle) et qu'aucun obstacle n'avait été percuté dans les frames précédentes
        if (hitColliders.Length > 0 && !_isHit)
        {
            Debug.Log("Player hit something");
            // Affectation du booléen indiquant qu'un obstacle a été percuté
            _isHit = true;
        }
        // Cas où aucun obstacle n'a été percuté
        if (hitColliders.Length == 0)
        {
            Debug.Log("Player no hit something");
            // Affectation du booléen indiquant qu'aucun obstacle n'a été percuté
            _isHit = false;
        }
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode permettant de réduire la taille de la sphère de détection des collisions du joueur lorsque celui-ci
    /// se baisse
    /// </summary>
    /// // -------------------------------------------------------------------------------
    public void ShrinkCollider(bool isSlidingDown)
    {
        
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Dessine le gizmo de la sphère du joueur (espace contenue dans la zone de collision) lors de la sélection de l'objet
    /// </summary>
    // -------------------------------------------------------------------------------
    private void OnDrawGizmosSelected()
    {
        // Affectation de la couleur rouge pour le gizmo
        Gizmos.color = Color.red;
        // Dessin du gizmo de la sphère
        Gizmos.DrawWireSphere(PlayerSpherePosition, _sphereRadius);
    }
}
