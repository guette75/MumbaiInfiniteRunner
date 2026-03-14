using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Jump Parameters")]
    // Durée du saut
    [SerializeField] private float _jumpDuration = 1f;
    // Hauteur du saut
    [SerializeField] private float _jumpHeight = 2f;
    // Objet permettant de modifier le déroulement de la phase de la montée du saut
    // (pour que celui-ci ne soit plus linéaire)
    [SerializeField] private AnimationCurve _jumpCurve;
    // Objet permettant de modifier le déroulement de la phase de la descente du saut
    // (pour que celui-ci ne soit plus linéaire)
    [SerializeField] private AnimationCurve _fallCurve;
    
    [Header("Slide Parameters")]
    // Durée du mouvement de glisse
    [SerializeField] private float _slideDuration = 1f;
    // Tableau de vecteurs contenant les destinations cibles du personnage
    [SerializeField] private Transform[] _slideTargets;
    
    [Header("Debug")]
    // Booléen indiquant si le joueur est en train de sauter
    [SerializeField] private bool _isJumping = false;
    // Indice de la ligne sur laquelle se trouve le joueur
    [SerializeField] private int _currentLaneIndex = 1;
    // Booléen indiquant si le joueur est en train de glisser
    [SerializeField] private bool _isSliding = false;
    

    public void Update()
    {
        // Cas de l'appui sur la flèche du haut => Saut
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            // Cas où le joueur est déjà en train de sauter
            if (_isJumping)
            {
                // On quitte la méthode
                return;
            }
            // Démarrage de la coroutine relatif au saut
            StartCoroutine(JumpCoroutine());
        }
        
        // Cas de l'appui sur la flèche de gauche => Glissade sur la gauche
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            // Cas où le joueur est déjà en train de glisser
            if (_isSliding)
            {
                // On quitte la méthode
                return;
            }
            // Cas où l'on se trouve déjà sur la ligne de gauche
            if (_currentLaneIndex == 0)
            {
                // On quitte la méthode
                return;
            }
            // Mise à jour de la ligne sur laquelle se trouve le joueur
            _currentLaneIndex--;
            // Glissade (slide) à gauche de la ligne actuelle
            StartCoroutine(SlideCoroutine(_slideTargets[_currentLaneIndex]));
        }
        
        // Cas de l'appui sur la flèche de droite => Glissade sur la droite
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            // Cas où le joueur est déjà en train de glisser
            if (_isSliding)
            {
                // On quitte la méthode
                return;
            }
            // Cas où l'on se trouve déjà sur la ligne de droite
            if (_currentLaneIndex == _slideTargets.Length - 1)
            {
                // On quitte la méthode
                return;
            }
            // Mise à jour de la ligne sur laquelle se trouve le joueur
            _currentLaneIndex++;
            // Glissade (slide) à droite de la ligne actuelle
            StartCoroutine(SlideCoroutine(_slideTargets[_currentLaneIndex]));
        }
    }

    
    // Coroutine permettant de gérer le saut en arrière plan
    // 2 curves utilisés, mais une seule aurait pu suffire
    private IEnumerator JumpCoroutine()
    {
        // Mise à jour du booléen insiquant que le joueur est en train de sauter
        _isJumping = true;
        // Durée actuelle du saut
        float jumpTimer = 0f;
        // Durée de la moitié du saut (une moitié pour la nomtée, une moitié pour la descente)
        float halfJumpDuration = _jumpDuration / 2f;

        // Itération tant que la durée actuelle de la phase de la montée du saut n'a pas atteint la durée totale
        // prévue pour le saut (la montée n'est pas terminée)
        while (jumpTimer < halfJumpDuration)
        {
            // Mise à jour de la durée du saut en ajoutant le temps écoulé depuis l'exécution de la frame précédente
            jumpTimer += Time.deltaTime;
            //Debug.Log($"Jump Timer: {jumpTimer}");
            // Ratio d'avancement du saut afin de lisser son déroulement dans le temps
            float normalizedTime = jumpTimer / halfJumpDuration;
            // Hauteur du saut en fonction de l'avancement de celui-ci dans le temps
            // Saut linéaire, abandonné
            // float targetHeight = _jumpHeight * normalizedTime;
            // Saut plus réaliste
            float targetHeight = _jumpCurve.Evaluate(normalizedTime) * _jumpHeight;
            // Calcul de la nouvelle position du joueur. On en met à jour que l'axe des Y, seul axe impacté par le saut
            Vector3 targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
            // Mise à jour de la,position du joueur
            transform.position = targetPosition;
            // Indique à Unity que le traitement dans cette frame est terminée. La boucle reprendra dans la boucle
            // suivante si elle n'est pas terminée
            yield return null;
        }
        
        // Réinitialisation du timer reltif à la durée du saut en cours
        jumpTimer = 0f;
        
        // Itération tant que la durée actuelle de la phase de la descente du saut n'a pas atteint la durée totale
        // prévue pour le saut (la descente n'est pas terminée)
        while (jumpTimer < halfJumpDuration)
        {
            // Mise à jour de la durée du saut en ajoutant le temps écoulé depuis l'exécution de la frame précédente
            jumpTimer += Time.deltaTime;
            // Ratio d'avancement du saut afin de lisser son déroulement dans le temps
            float normalizedTime = jumpTimer / halfJumpDuration;
            // Hauteur du saut en fonction de l'avancement de celui-ci dans le temps
            float targetHeight = _fallCurve.Evaluate(normalizedTime) * _jumpHeight;
            // Calcul de la nouvelle position du joueur. On en met à jour que l'axe des Y, seul axe impacté par le saut
            Vector3 targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);
            // Mise à jour de la,position du joueur
            transform.position = targetPosition;
            // Indique à Unity que le traitement dans cette frame est terminée. La boucle reprendra dans la boucle
            // suivante si elle n'est pas terminée
            yield return null;
        }
        
        //Debug.Log("Coroutine finished");
        // Mise à jour du booléen insiquant que le joueur ne saute plus
        _isJumping = false;
    }


    // Coroutine permettant de gérer les glissades en arrière plan (du centre vers la gauche et réciproquement, 
    // ainsi que du centre vers la droite et réciproquement)
    private IEnumerator SlideCoroutine(Transform target)
    {
        // Mise à jour du booléen insiquant que le joueur est en train de glisser
        _isSliding = true;
        // Durée actuelle de la glissade
        float slideTimer = 0f;

        // Itération tant que la durée actuelle de la glissade dn'a pas atteint la durée totale prévue pour la
        // glissade (la glissade n'est pas terminée)
        while (slideTimer < _slideDuration)
        {
            // Mise à jour de la durée de la glissade en ajoutant le temps écoulé depuis l'exécution de la frame
            // précédente
            slideTimer += Time.deltaTime;
            // Ratio d'avancement de la glissade afin de gérer le déplacement du joueur sur l'axe des X en fonction
            // du temps
            float normalizedTime = slideTimer / _slideDuration;
            // Vecteur cible (coordonnées du point visée) à atteindre
            // Attention : On ne touche pas à l'axe des y car un saut peut être en cours au même moment
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            // Appel d'une fonction mathématique permettant de calculer des points situés entre un point de départ A
            // et un point d'arrivée B en fonction du ratio du temps écoulé sur le temps total (interpolation linéaire).
            transform.position = Vector3.Lerp(transform.position, targetPosition, normalizedTime);
            // Indique à Unity que le traitement dans cette frame est terminée. La boucle reprendra dans la boucle
            // suivante si elle n'est pas terminée
            yield return null;
        }
        
        // Mise à jour du booléen insiquant que le joueur ne glisse plus
        _isSliding = false;
    }
}
