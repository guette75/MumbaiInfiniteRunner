using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [Header("Parameters")]
    // Vitesse de déplacement des plans contenant les objets et les obstacles
    [SerializeField, Tooltip("Transaction speed of chunks in m/s")] private float _translationSpeed = 1f;
    // Nombre de plan Unity pré-construits actifs à afficher
    [SerializeField] private int _activeChunksCount = 5;
    // Nombre de plan Unity pré-construits se trouvant derrière le joueur en attente de destruction
    [SerializeField] private int _behindChunksCount = 1;
    // Durée de l'arrêt du défilement des plans Unity pré-construits lorsque le joueur subit des dégâts
    [SerializeField] private float _stopDelayOnDamage = 0.2f;
    
    [Header("Components")]
    // Pool des plans Unity pré-construits disponibles pour l'affichage infini
    [SerializeField] private ChunkController[] _chunksPool;
    
    // Liste de plan Unity pré-construits instantiés
    private readonly List<ChunkController> _instanceChunks = new();
    // Sauvegarde de la vitesse de translation des plans Unity affectée à la configuration du jeu
    private float _baseTranslationSpeed;
    // Chronomètre relatif à l'arrêt du défilement des plans Unity pré-construits
    private float _stopDelayTimer;
    // Booléen indiquant si le chronomètre relatif à l'arrêt du défilement des plans Unity pré-construits est en route
    private bool _stoppedTimer;


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois juste avant la première exécution de la méthode update (exécutée à chaque
    /// nouvelle frame), juste après la création de cette instance.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void Start()
    {
        // Sauvegarde de la vitesse de déroulement des plans Unity configurée pour le jeu
        _baseTranslationSpeed = _translationSpeed;
        // Abonnement à l'évènement déclenché lors de la mise à jour du nombre de vies du joueur
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLifeUpdated;
        // Création des plans Unity pré-construits initiaux
        AddBaseChunk();
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois lorsque l'objet est détruit.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void OnDestroy()
    {
        // Désabonnement de l'évènement déclenché lors de la mise à jour du nombre de vies du joueur
        EventSystem.OnPlayerLifeUpdated -= HandlePlayerLifeUpdated;
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée à chaque exécution d'un frame
    /// Permet le déplacement des plans Unity à chaque exécution d'une nouvelle frame
    /// </summary>
    /// // -------------------------------------------------------------------------------
    private void Update()
    {
        // Gestion du chronomètre relatif à l'arrêt du défilement des plans Unity suite à la collision du joueur
        // avec un obstacle (ne fera rien si aucune collision n'a eu lieu)
        resetMovementAfterDelay();
        // Itération sur l'ensemble des plans Unity pré-construits
        foreach (ChunkController chunk in _instanceChunks)
        {
            // Translation du plan Unity sélectionné en fonction de la vitesse de déplacement et du temps passé entre
            // 2 affichages de frame
            chunk.transform.Translate(Vector3.back * (Time.deltaTime * _translationSpeed));
        }
        // Mise à jour de la liste des plans Unity pré-construits
        UpdateChunks();
    }

    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Mise à jour de la liste des plans Unity pré-construits
    /// Suppression des plans déjà utilisés, se trouvant derrière le personnage
    /// Création de nouveaux plans, afin qu'il y en ait toujours de disponibles pour l'affichage et l'utilisation
    /// de ceux-ci dans le jeu
    /// </summary>
    // -------------------------------------------------------------------------------
    private void UpdateChunks()
    {
        // Liste des plans Unity pré-construits se trouvant derrière le personnage
        List<ChunkController> behindChunks = new();
        // Itération sur l'ensemble des plans Unity pré-construits
        foreach (ChunkController chunk in _instanceChunks)
        {
            // Vérification que ce plan Unity ne se trouve pas derrière le personnage
            if (chunk.IsBehindPlayer())
            {
                // Ajout de ce plan Unity dans la liste des plans Unity dépassés par le personnage
                behindChunks.Add(chunk);
            }
        }
        // ----------
        // Potentielle suppression des plans Unity déjà utilisés
        // ----------
        // Cas où le nombre de plans Unity derrière le joueur est supérieur au nombre de plans Unity en attente de
        // suppressions
        if (behindChunks.Count > _behindChunksCount)
        {
            // Calcul du nombre de plans Unity à supprimer
            int chunksToDelete = behindChunks.Count - _behindChunksCount;
            // Itération sur le nombre de plans Unity à supprimer
            for (int i=0; i<chunksToDelete; i++)
            {
                // Récupération d'un plan Unity à supprimer
                ChunkController chunkToDelete = behindChunks[i];
                // Retrait du plan Unity des plans Unity pré-construits
                _instanceChunks.Remove(chunkToDelete);
                // Suppression du plan Unity
                Destroy(chunkToDelete.gameObject);
            }
        }
        // ----------
        // Potentiel ajout de nouveaux plans Unity
        // ----------
        // Nombre de plans Unity à créer
        int missingChunksCount = _activeChunksCount - _instanceChunks.Count;
        // Itération sur le nombre de plans Unity à créer
        for (int i = 0; i < missingChunksCount; i++)
        {
            // Ajout du nouveau plan Unity dans la liste des plans pré-construits actifs avec comme position de départ
            // La position de l'ancre situé à la fin du plan précédent
            ChunkController chunk = AddChunk(LastActiveChunk().EndAnchor);
            // Ajout du nouveau plan Unity dans la liste des plans pré-construits
            _instanceChunks.Add(chunk);
        }
    }


    // -------------------------------------------------------------------------------
    /// <summary>
    /// Renvoi un certain nombre de plans Unity pré-construits à afficher en fonction du nombre de plans Unity
    /// actifs souhaités.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void AddBaseChunk()
    {
        // Itération sur le nombre de plan Unity actifs souhaités
        for (int i = 0; i < _activeChunksCount; i++)
        {
            // Cas du premier plan Unity à afficher
            if (i == 0)
            {
                // Appel de la méthode d'affichage d'un plan Unity avec la position de départ du gameObject contenant
                // l'objet ObstacleController
                ChunkController baseChunk = AddChunk(transform.position);
                // Ajout du plan Unity affiché dans la liste des plans Unity actifs
                _instanceChunks.Add(baseChunk);
                // Passage à la prochaine itération de la boucle
                continue;
            }
            // Appel de la méthode d'affichage d'un plan Unity avec la position de fin du plan précédent contenu dans
            // l'ancre de fin rattaché à ce plan
            ChunkController chunk = AddChunk(LastActiveChunk().EndAnchor);
            // Ajout du plan Unity affiché dans la liste des plans Unity actifs
            _instanceChunks.Add(chunk);
        }
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Renvoi d'un plan Unity pré-construit à afficher
    /// </summary>
    // -------------------------------------------------------------------------------
    private ChunkController AddChunk(Vector3 position)
    {
        // Cas où le pool de plans Unity disponibles est vide
        if (_chunksPool.Length == 0)
        {
            Debug.LogError("Chunks pool is empty");
            // Renvoi de null
            return null;
        }
        
        // Sélection aléatoire d'un indice de plan Unity entre 0 et le nombre de plans contenus dans le pool - 1
        int index = Random.Range(0, _chunksPool.Length);
        // Instantiation du plan Unity à afficher avec les coordonnées d'affichage
        ChunkController chunk = Instantiate(_chunksPool[index], position, Quaternion.identity);
        // Renvoi du plan Unity
        return chunk;
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Renvoi du dernier plan Unity pré-construit contenus dans la liste des plans Unity pré-construits actifs
    /// </summary>
    // -------------------------------------------------------------------------------
    private ChunkController LastActiveChunk()
    {
        return _instanceChunks[_instanceChunks.Count - 1];
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
            // Démarrage du chronomètre relatif à l'arrêt du défilement des plans Unity suite à la collision du
            // joueur avec un obstacle
            _stoppedTimer = true;
            // On quitte la méthode car il n'y a rien à faire
            return;
        }
        // Mise à jour de la vitesse de déplacement des plans contenant les objets et les obstacles
        // On met la valeur à 0 car le joueur est mort
        _translationSpeed = 0;
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode gérant le chronomètre relatif à l'arrêt du défilement des plans Unity pré-construits suite à la
    /// collision du joueur avec un obstacle
    /// </summary>
    // -------------------------------------------------------------------------------
    private void resetMovementAfterDelay()
    {
        // Cas où le chronomètre relatif à l'arrêt du défilement des plans Unity n'est pas actif
        if (!_stoppedTimer)
        {
            // Le chronomètre n'est pas actif, il n'y a rien à faire, on sort immédiatement de la méthode
            return;
        }
        // Mise à jour de la durée du chronomètre
        _stopDelayTimer += Time.deltaTime;
        // Cas où la durée de l'arrêt du défilement des plans Unity est terminé
        if (_stopDelayTimer >= _stopDelayOnDamage)
        {
            // Arrêt du chronomètre
            _stoppedTimer = false;
            // Redémarrage du défilement des plans Unity
            _translationSpeed = _baseTranslationSpeed;
            // Réinitialisation du chronomètre
            _stopDelayTimer = 0f;
        }
    }
}
