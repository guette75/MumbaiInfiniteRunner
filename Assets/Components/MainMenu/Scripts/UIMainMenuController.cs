using System;
using TMPro;
using UnityEngine;

public class UIMainMenuController : MonoBehaviour
{
    // Nombre d'exécutions du jeu
    [SerializeField] private TMP_Text _runCountText;
    // Meilleur temps du jeu
    [SerializeField] private TMP_Text _bestTimeText;
    // Variables contenant les données utilisateur à afficher, sauvegarder et recharger
    private SaveData _saveData;
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelée une seule fois juste avant la première exécution de la méthode update (exécutée à chaque
    /// nouvelle frame), juste après la création de cette instance.
    /// </summary>
    // -------------------------------------------------------------------------------
    private void Start()
    {
        // Chargement de la donnée "SaveData" depuis l'appareil
        _saveData = SaveService.Load();
        // Ecriture du nombre d'exécutions du jeu
        _runCountText.text = $"Attempts: {_saveData.RunCount}";
        // Cas où aucun meilleur temps n'existe
        if (_saveData.BestTime == 0)
        {
            _bestTimeText.text = "No Best Time";
        }
        // Cas où un meilleur temps existe
        else
        {
            // Objet contenant le mailleur temps au format date/heure afin de pouvoir l'afficher
            TimeSpan timeSpan = new TimeSpan(0, 0, Mathf.RoundToInt(_saveData.BestTime));
            // Affichage du meilleur temps
            _bestTimeText.text =
                "Best time: " + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
        }
    }

    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Démarre le jeu.
    /// Lance la scène de démarrage.
    /// </summary>
    // -------------------------------------------------------------------------------
    public void StartGame()
    {
        // Incrémentation du nombre d'exécutions du jeu
        _saveData.RunCount++;
        // Sauvegarde de cette donnée sur l'apparail du joueur
        SaveService.Save(_saveData);
        // Démarre le jeu
        SceneLoaderService.loadGame();
    }
    
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode appelant la fermeture de l'application
    /// </summary>
    // -------------------------------------------------------------------------------
    public void QuitGame()
    {
        // Cas où le jeu ne s'exécute pas dans l'éditeur
        #if !UNITY_EDITOR
            // Quitte le jeu dans le mode normal (hors éditeur)
            Application.Quit();
        // Cas où le jeu s'exécute dans l'éditeur
        #else
            // quitte le jeu dans le mode éditeur
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
