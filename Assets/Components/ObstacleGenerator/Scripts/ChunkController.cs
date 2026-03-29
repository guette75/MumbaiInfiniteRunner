using UnityEngine;

public class ChunkController : MonoBehaviour
{
    // Ancre délimitant la fin du plan Unity pré-construit
    [SerializeField] private Transform _endAnchor;
    
    // Renvoie l'ancre de fin du plan Unity (Ecriture C# pour le GET)
    public Vector3 EndAnchor => _endAnchor.position;
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Retourne un booléen indiquant si le plan Unity pré-construit se situe dérrière le joueur (z <= 0)
    /// </summary>
    // -------------------------------------------------------------------------------
    public bool IsBehindPlayer()
    {
        return EndAnchor.z <= 0;
    }
}
