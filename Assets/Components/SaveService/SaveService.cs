using System;
using System.IO;
using UnityEngine;

public static class SaveService
{
    // Pour une sérialisation plus poussée, utiliser NewtonSoft
    
    // Nom du fichier de sauvegarde
    private const string FILE_NAME = "InfiniteDiscountSave.json";
    // Chemin vers le fichier de sauvegarde
    private static string filePath => Path.Combine(Application.persistentDataPath, FILE_NAME);
    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode statique permettant la sauvegarde des données du jeu sur l'appareil du joueur.
    /// Le format de fichier utilisé est le JSON.
    /// /// <param name="data"></param>
    /// </summary>
    // -------------------------------------------------------------------------------
    public static void Save(SaveData data)
    {
        // Convertit les champs public de l'objet passé en paramètre au format JSON
        string json = JsonUtility.ToJson(data);
        // Ecriture du JSON dans le fichier
        File.WriteAllText(filePath, json);
        Debug.Log($"Data successfully saved at {filePath}");
    }

    
    // -------------------------------------------------------------------------------
    /// <summary>
    /// Méthode statique permettant le chargement des données du jeu depuis l'appareil du joueur.
    /// Le format de fichier utilisé est le JSON.
    /// </summary>
    /// /// <returns>La donnée contenue dans le fichier</returns>
    // -------------------------------------------------------------------------------
    public static SaveData Load()
    {
        try
        {
            // Lecture du JSON contenu dans le fichier
            string json = File.ReadAllText(filePath);
            // Conversion du JSON en un objet SaveData
            return JsonUtility.FromJson<SaveData>(json);
        }
        catch (Exception exception)
        {
            Debug.LogWarning($"No data found, creating a new one... Details: {exception}");
            // Aucune donnée n'a pu être chargée suite au déclenchement de l'exception, on crée donc une nouvelle
            // instance
            return new SaveData();
        }
    }
}
