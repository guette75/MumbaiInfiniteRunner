using System;
using TMPro;
using UnityEngine;

public class UIMainMenuController : MonoBehaviour
{
    // Nombre d'exécutions du jeu
    [SerializeField] private TMP_Text _runCountText;
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
        SaveData saveData = SaveService.Load();
        // Cas où une donnée a été chargée
        if (saveData == null)
        {
            // On instancie la variable saveData
            _saveData = new SaveData();
        }
        else
        {
            // On affecte la donnée chargée depuis l'appareil dans la variable contenant les données utilisateur
            _saveData = saveData;
            Debug.Log($"Save data Run Counts : {saveData.runCount}");
        }
        // Ecriture réduite de ce if else
        // _saveData = saveData ?? new SaveData();
        // Ecriture du nombre d'exécutions du jeu
        _runCountText.text = $"Runs: {_saveData.runCount}";
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
        _saveData.runCount++;
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
