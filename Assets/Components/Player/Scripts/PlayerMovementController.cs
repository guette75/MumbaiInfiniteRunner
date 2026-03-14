using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
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
    

    public void Update()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            // Démarrage de la coroutine relatif au saut
            StartCoroutine(JumpCoroutine());
        }
    }

    // Coroutine permettant de gérer le saut en arrière plan
    // 2 curves utilisés, mais une seule aurait pu suffire
    private IEnumerator JumpCoroutine()
    {
        // Durée actuelle du saut
        float jumpTimer = 0f;
        // Durée de la moitié du saut (une moitié pour la nomtée, une moitié pour la descente)
        float halfJumpDuration = _jumpDuration / 2f;

        // Itération tant que la durée actuelle de la phase de la montée du saut n'a pas atteint la durée totale
        // prévue pour le saut
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
            // suivante
            yield return null;
        }
        
        // Réinitialisation du timer reltif à la durée du saut en cours
        jumpTimer = 0f;
        
        // Itération tant que la durée actuelle de la phase de la descente du saut n'a pas atteint la durée totale
        // prévue pour le saut
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
            // suivante
            yield return null;
        }
        //Debug.Log("Coroutine finished");
    }
}
