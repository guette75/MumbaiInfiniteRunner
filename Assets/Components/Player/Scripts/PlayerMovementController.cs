using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Jump Parameters")]
    // Durée du saut
    [SerializeField, Tooltip("Jump duration in seconds")] private float _jumpDuration = 1f;
    // Hauteur du saut
    [SerializeField, Tooltip("Jump height in meters")] private float _jumpHeight = 2f;
    // Objet permettant de modifier le déroulement de la phase de la montée du saut
    // (pour que celui-ci ne soit plus linéaire)
    [SerializeField] private AnimationCurve _jumpCurve;
    // Objet permettant de modifier le déroulement de la phase de la descente du saut
    // (pour que celui-ci ne soit plus linéaire)
    [SerializeField] private AnimationCurve _fallCurve;
    
    [Header("Slide Parameters")]
    // Durée du mouvement de glisse
    [SerializeField, Tooltip("Slide duration in seconds")] private float _slideDuration = 1f;
    // Tableau de vecteurs contenant les destinations cibles du personnage
    [SerializeField] private Transform[] _slideTargets;
    
    [Header("Slide Down Parameters")]
    // Durée du mouvement de flexion
    [SerializeField, Tooltip("Slide down duration in seconds")] private float _slideDownDuration = 1.5f;
    
    [Header("Components")]
    // Objet Animator relié au personnage
    [SerializeField] private Animator _animator;
    
    [Header("Debug")]
    // Booléen indiquant si le joueur est en train de sauter
    [SerializeField] private bool _isJumping = false;
    // Indice de la ligne sur laquelle se trouve le joueur
    [SerializeField] private int _currentLaneIndex = 1;
    // Booléen indiquant si le joueur est en train de glisser
    [SerializeField] private bool _isSliding = false;
    // Booléen indiquant si le joueur est en train de se baisser
    [SerializeField] private bool _isSlidingDown = false;
    // Booléen indiquant que les mouvements du joueur sont vérouillés
    [SerializeField] private bool _locked;
    
    // Coroutine exécutant la glissade (déplacement latéral) du joueur
    private Coroutine _slideCoroutine;


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois lorsque l'objet est détruit.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void OnDestroy()
    {
        // Désabonnement de l'évènement déclenché lors de la mise à jour du nombre de vies du joueur
        EventSystem.OnPlayerLifeUpdated -= HandlePlayerLifeUpdated;
        // Désabonnement de l'évènement déclenché lors du changement d'état du jeu
        EventSystem.OnStateChanged -= HandleStateChanged;
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Traitement exécuté au réveil de l'objet. Cette méthode est appelée en tout premier.
    /// </summary>
    // -------------------------------------------------------------------------------
    public void Awake()
    {
        // Activation de l'évènement à false indiquant qu'aucun mouvement de flexion n'est en cours lors du réveil de
        // l'objet
        EventSystem.OnPlayerSlideDown?.Invoke(false);
        Debug.Log("Player awake");
        // Abonnement à l'évènement déclenché lors du changement d'état du jeu
        EventSystem.OnStateChanged += HandleStateChanged;
        // Au démarrage du jeu, les mouvements du joueur sont verouillés
        _locked = true;
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Traitement à exécuter à chaque frame
    /// </summary>
    // -------------------------------------------------------------------------------
    public void Update()
    {
        // Cas où les mouvements du joueur sont verouillés
        if (_locked)
        {
            // On quitte la méthode car aucune action n'est désormais possible
            return;
        }
        // Traitement de l'appui sur la flèche du haut → Saut
        HandleJump();
        // Traitement de l'appui sur la flèche de gauche ou la flèche de droite → Glissade
        HandleSlide();
        // Traitement de l'appui sur la flèche du bas → Flexion
        HandleSlideDown();
    }

    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Traitement de l'appui sur la flèche du haut → Exécution du saut
    /// </summary>
    // -------------------------------------------------------------------------------
    private void HandleJump()
    {
        // Cas de l'appui sur la flèche du haut → Saut
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            // Cas où le joueur est déjà en train de sauter ou de se baisser
            if (_isJumping || _isSlidingDown)
            {
                // On quitte la méthode
                return;
            }
            // Démarrage de la coroutine relatif au saut
            StartCoroutine(JumpCoroutine());
        }
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Traitement de l'appui sur la flèche de gauche ou de droite → Exécution de la glissade
    /// </summary>
    // -------------------------------------------------------------------------------
    private void HandleSlide()
    {
        // Cas de l'appui sur la flèche de gauche → Glissade sur la gauche
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            // Cas où le joueur est déjà en train de glisser
            if (_isSliding)
            {
                // Afin de pouvoir exécuter un nouveau déplacement même si une glissade du joueur est déjà
                // en cours, on arrête la précédente coroutine pour en redémarrer une nouvelle.
                // Vérification qu'une coroutine est en cours d'exécution
                // Eviter autant que possible les null check
                if (_slideCoroutine != null)
                {
                    // Arrêt de la coroutine
                    StopCoroutine(_slideCoroutine);
                    // Mise à jour du booléen indiquant que le déplacement est terminé
                    _isSliding = false;
                }
            }
            // Cas où l'on se trouve déjà sur la ligne de gauche
            if (_currentLaneIndex == 0)
            {
                // On quitte la méthode
                return;
            }
            // Mise à jour de la ligne sur laquelle se trouve le joueur
            _currentLaneIndex--;
            // Glissade (slide) vers la gauche de la ligne actuelle
            _slideCoroutine = StartCoroutine(SlideCoroutine(_slideTargets[_currentLaneIndex]));
        }
        
        // Cas de l'appui sur la flèche de droite → Glissade sur la droite
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            // Cas où le joueur est déjà en train de glisser
            if (_isSliding)
            {
                // Afin de pouvoir exécuter un nouveau déplacement même si une glissade du joueur est déjà
                // en cours, on arrête la précédente coroutine pour en redémarrer une nouvelle.
                // Vérification qu'une coroutine est en cours d'exécution
                // Eviter autant que possible les null check
                if (_slideCoroutine != null)
                {
                    // Arrêt de la coroutine
                    StopCoroutine(_slideCoroutine);
                    // Mise à jour du booléen indiquant que le déplacement est terminé
                    _isSliding = false;
                }
            }
            // Cas où l'on se trouve déjà sur la ligne de droite
            if (_currentLaneIndex == _slideTargets.Length - 1)
            {
                // On quitte la méthode
                return;
            }
            // Mise à jour de la ligne sur laquelle se trouve le joueur
            _currentLaneIndex++;
            // Glissade (slide) vers la droite de la ligne actuelle
            _slideCoroutine = StartCoroutine(SlideCoroutine(_slideTargets[_currentLaneIndex]));
        }
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Traitement de l'appui sur la flèche du bas → Exécution de la flexion
    /// </summary>
    // -------------------------------------------------------------------------------
    private void HandleSlideDown()
    {
        // Cas de l'appui sur la flèche du bas → Flexion
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            // Cas où le joueur est déjà en train de se baisser ou en train de sauter
            if (_isSlidingDown || _isJumping)
            {
                // On quitte la méthode
                return;
            }
            // Démarrage de la coroutine relatif au mouvement de flexion du joueur
            StartCoroutine(SlideDownCoroutine());
        }
    }

    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Coroutine permettant de gérer le saut en arrière plan
    /// </summary>
    // -------------------------------------------------------------------------------
    // 2 curves utilisés, mais une seule aurait pu suffire
    private IEnumerator JumpCoroutine()
    {
        // --------------------
        // Jumping
        // --------------------
        // Mise à jour du booléen indiquant que le joueur est en train de sauter
        _isJumping = true;
        // Mise à jour du booléen indiquant que le joueur est en train de sauter au niveau de l'animator
        _animator.SetBool("IsJumping", true);
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
        
        // --------------------
        // Falling
        // --------------------
        // Activation du trigger dans l'animator indiquant que la chute débute
        _animator.SetTrigger("Falling");
        // Réinitialisation du timer relatif à la durée du saut en cours
        jumpTimer = 0f;
        
        // Itération tant que la durée actuelle de la phase de la descente du saut n'a pas atteint la durée totale
        // prévue pour le saut (la descente n'est pas terminée)
        while (jumpTimer <= halfJumpDuration)
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
        // Mise à jour du booléen indiquant que le joueur ne saute plus
        _isJumping = false;
        // Mise à jour du booléen indiquant que le joueur ne saute plus au niveau de l'animator
        _animator.SetBool("IsJumping", false);
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Coroutine permettant de gérer les glissades en arrière plan (du centre vers la gauche et réciproquement, 
    /// ainsi que du centre vers la droite et réciproquement)
    /// </summary>
    // -------------------------------------------------------------------------------
    private IEnumerator SlideCoroutine(Transform target)
    {
        // Mise à jour du booléen indiquant que le joueur est en train de glisser
        _isSliding = true;
        // Durée actuelle de la glissade
        float slideTimer = 0f;

        // Itération tant que la durée actuelle de la glissade n'a pas atteint la durée totale prévue pour la
        // glissade (la glissade n'est pas terminée)
        while (slideTimer <= _slideDuration)
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
        
        // Mise à jour du booléen indiquant que le joueur ne glisse plus
        _isSliding = false;
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Coroutine permettant de gérer la flexion du joueur en arrière plan
    /// </summary>
    // -------------------------------------------------------------------------------
    private IEnumerator SlideDownCoroutine()
    {
        // Mise à jour du booléen indiquant que le joueur est en train de se baisser
        _isSlidingDown = true;
        // Mise à jour du booléen indiquant que le joueur est en train de se baisser au niveau de l'animator
        _animator.SetBool("IsSlidingDown", true);
        // Activation de l'évènement relatif à la flexion du joueur
        EventSystem.OnPlayerSlideDown?.Invoke(true);
        // Durée du mouvement de flexion
        float slideTimer = 0f;
        
        // Itération tant que la durée actuelle de la flexion n'a pas atteint la durée totale prévue pour la
        // flexion (la flexion n'est pas terminée)
        while (slideTimer <= _slideDownDuration)
        {
            // Mise à jour de la durée de la flexion en ajoutant le temps écoulé depuis l'exécution de la frame
            // précédente
            slideTimer += Time.deltaTime;
            // Indique à Unity que le traitement dans cette frame est terminée. La boucle reprendra dans la boucle
            // suivante si elle n'est pas terminée
            yield return null;
        }
        
        // Mise à jour du booléen indiquant que le joueur ne se baisse plus
        _isSlidingDown = false;
        // Mise à jour du booléen indiquant que le joueur ne se baisse plus au niveau de l'animator
        _animator.SetBool("IsSlidingDown", false);
        // Désactivation de l'évènement relatif à la flexion du joueur
        EventSystem.OnPlayerSlideDown?.Invoke(false);
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
            // On déclenche l'animation relative à la prise de dégâts par le joueur
            _animator.SetTrigger("TakeDamage");
            // On quitte la méthode car il n'y a rien à faire
            return;
        }
        // Arrêt des coroutines
        StopAllCoroutines();
        // Appel du trigger relatif à la mort du joueur dans l'animator (l'animation correspondant à la mort du joueur
        // va se jouer)
        _animator.SetTrigger("Dead");
        // Mise à jour du booléen indiquant que le joueur n'a plus de vie et qu'il ne peut donc plus effectuer de
        // mouvements
        _locked = true;
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée lorsque l'état courant du jeu a changé
    /// </summary>
    /// <param name="newState"></param>
    // -------------------------------------------------------------------------------
    private void HandleStateChanged(State newState)
    {
        // Cas où l'état passé en paramètre ne correspond pas à un état relatif à l'exécution du jeu
        if (newState is not GameState)
        {
            // Verrouillage des mouvements du joueur
            _locked = true;
            // Arrêt des coroutines
            StopAllCoroutines();
            // Désabonnement de l'évènement déclenché lors de la mise à jour du nombre de vies du joueur,
            // car l'évènement ne correspond pas à l'exécution du jeu
            EventSystem.OnPlayerLifeUpdated -= HandlePlayerLifeUpdated;
            // On quitte la méthode car il n'y a rien à faire
            return;
        }
        // Changement d'animation du joueur : Il passe de l'état inactif à l'état de course
        _animator.SetTrigger("Running");
        // Abonnement à l'évènement déclenché lors de la mise à jour du nombre de vies du joueur
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLifeUpdated;
        // Déverrouillage des mouvements du joueur
        _locked = false;
    }
}
