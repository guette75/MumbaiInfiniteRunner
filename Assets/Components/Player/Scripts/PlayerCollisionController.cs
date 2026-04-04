using System;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    [Header("Parameters")]
    // Position relative au centre de la sphère reliée au personnage pour la gestion des collisions
    [SerializeField] private Vector3 _sphereCenter;
    // Rayon de la sphère reliée au personnage pour la gestion des collisions
    [SerializeField] private float _sphereRadius;
    // Position relative au centre de la sphère reliée au personnage pour la gestion des collisions lorsque celui-ci
    // est baissé
    [SerializeField] private Vector3 _shrinkSphereCenter;
    // Rayon de la sphère reliée au personnage pour la gestion des collisions lorsque celui-ci est baissé
    [SerializeField] private float _shrinkSphereRadius;
    
    // Booléen indiquant qu'un obstacle a été percuté
    private bool _isHit;
    // Position actuelle du centre de la sphère reliée au personnage pour la gestion des collisions
    private Vector3 _currentSphereCenter;
    // Rayon actuel de la sphère reliée au personnage pour la gestion des collisions
    private float _currentSphereRadius;
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Retourne la position de la sphère de détection des collisions du joueur
    /// Ici, la position du joueur + la position de la sphère par rapport au joueur
    /// </summary>
    /// // -------------------------------------------------------------------------------
    private Vector3 PlayerSpherePosition => transform.position + _currentSphereCenter;


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois juste avant la première exécution de la méthode update (exécutée à chaque
    /// nouvelle frame), juste après la création de cette instance.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void Start()
    {
        // Au démarrage le joueur est debout : la sphère de détection de collision correspond donc à celle mise en
        // place lorsque le joueur est debout
        _currentSphereCenter = _sphereCenter;
        _currentSphereRadius = _sphereRadius;
        // Mise en place de l'écoute de l'évènement relatif à la flexion du joueur et appel de la méthode associée
        // lors du déclenchement de cet évènement
        EventSystem.OnPlayerSlideDown += ShrinkCollider;
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois lorsque l'objet est détruit.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void OnDestroy()
    {
        // Suppression de l'écoute de l'évènement relatif à la flexion du joueur
        EventSystem.OnPlayerSlideDown -= ShrinkCollider;
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
        Collider[] hitColliders = Physics.OverlapSphere(PlayerSpherePosition, _currentSphereRadius);
        // Cas où au moins une zone de collision est contenue dans cette sphère virtuelle
        // (le joueur a heurté un obstacle) et qu'aucun obstacle n'avait été percuté dans les frames précédentes
        if (hitColliders.Length > 0 && !_isHit)
        {
            Debug.Log("Player hit something");
            // Déclenchement de l'évènement indiquant qu'une collision avec le joueur vient d'avoir lieu
            EventSystem.OnPlayerCollision?.Invoke();
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
    /// se baisse. Est invoké lorsqu'un évènement se produit (mouvement de flexion du joueur)
    /// </summary>
    /// // -------------------------------------------------------------------------------
    public void ShrinkCollider(bool isSlidingDown)
    {
        Debug.Log("Shrinking collider");
        // Cas où le joueur est en train de se baisser
        if (isSlidingDown)
        {
            // La sphère de détection de collision correspond donc à celle mise en place lorsque le joueur est
            // en flexion
            _currentSphereCenter =  _shrinkSphereCenter;
            _currentSphereRadius = _shrinkSphereRadius;
        }
        // Cas où le joueur est en train de se baisser
        else
        {
            // La sphère de détection de collision correspond donc à celle mise en place lorsque le joueur est debout
            _currentSphereCenter = _sphereCenter;
            _currentSphereRadius = _sphereRadius;
        }
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Dessine le gizmo de la sphère du joueur (espace contenue dans la zone de collision) lors de la sélection de l'objet
    /// </summary>
    // -------------------------------------------------------------------------------
    private void OnDrawGizmosSelected()
    {
        // Gizmo lorsque le joueur est debout
        // Affectation de la couleur rouge pour le gizmo
        Gizmos.color = Color.red;
        // Dessin du gizmo de la sphère
        Gizmos.DrawWireSphere(transform.position + _sphereCenter, _sphereRadius);
        // Gizmo lorsque le joueur est baissé
        // Affectation de la couleur verte pour le gizmo
        Gizmos.color = Color.green;
        // Dessin du gizmo de la sphère
        Gizmos.DrawWireSphere(transform.position + _shrinkSphereCenter, _shrinkSphereRadius);
        // Gizmo durant l'exécution du jeu
        // Affectation de la couleur jaune pour le gizmo
        Gizmos.color = Color.yellow;
        // Dessin du gizmo de la sphère
        Gizmos.DrawWireSphere(PlayerSpherePosition, _currentSphereRadius);
    }
}
