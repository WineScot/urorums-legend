using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class SaveSystem
{
    public static void SavePlayer (int saveNumber, HeroManager hero)    // quick save, szybkie zapisywanie stanu gry przy użyciu hot key "s" oraz tymczasowy save przy otwieraniu sceny "Save Game" (scena nr 5)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/UrorumsSave" + saveNumber + ".binsave";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        SavePlayerData data = new SavePlayerData(hero);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Game progress saved successfully!");
    }

    public static void SavePlayer(int saveNumber)   // Odczytuje tymczasowy zapis o indeksie 0 i zapisuje w slocie o wskazanym indeksie. Na końcu wywołuje LoadPlayer() i wczytuje ponownie grę.
    {
        string path = Application.persistentDataPath + "/UrorumsSave0.binsave";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SavePlayerData heroData = formatter.Deserialize(stream) as SavePlayerData;
            stream.Close();

            path = Application.persistentDataPath + "/UrorumsSave" + saveNumber + ".binsave";
            stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, heroData);
            stream.Close();

            LoadPlayer(saveNumber);
        }
        else
        {
            Debug.LogError("Temporary save number 0 not found in " + path);
            return;
        }
    }

    public static SavePlayerData LoadSaveData(int saveNumber)   // odczytywanie save'ów w scenie odczytu zapisu gry (SaveLoadButtons). Zwraca odczytany save.
    {
        string path = Application.persistentDataPath + "/UrorumsSave" + saveNumber + ".binsave";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SavePlayerData data = formatter.Deserialize(stream) as SavePlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogWarning("Save number " + saveNumber + " not found in " + path);
            return null;
        }
    }

    public static void LoadPlayer(int saveNumber)   // odczytywanie save'ów i załadowanie sceny w której ostatnim razem znajdował się gracz
    {
        string path = Application.persistentDataPath + "/UrorumsSave" + saveNumber + ".binsave";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SavePlayerData data = formatter.Deserialize(stream) as SavePlayerData;
            stream.Close();

            if (PlayerPrefs.GetInt("CurrentSave") == 0)         // CurrentSave może być równe 0 tylko wtedy, gdy wybrana została opcja New Game i następuje pierwszy zapis z wyborem slota.
                PlayerPrefs.SetInt("CurrentSave", saveNumber);  // Nie zmieniamy wtedy CurrentSave, bo saveNumber w tymczasowym savie wynosi 0.
            PlayerPrefs.SetFloat("HeroHealth", data.health);
            PlayerPrefs.SetFloat("HeroPositionX", data.position[0]);
            PlayerPrefs.SetFloat("HeroPositionY", data.position[1]);
            PlayerPrefs.SetFloat("HeroPositionZ", data.position[2]);

            PlayerPrefs.SetString("SaveDate", data.saveDate);
            PlayerPrefs.SetInt("CurrentScene", data.currentScene);

            SceneManager.LoadScene(data.currentScene);
            Debug.Log("hero health: " + PlayerPrefs.GetFloat("HeroHealth") + "\nPos.x: " + PlayerPrefs.GetFloat("HeroPositionX") + "\nPos.y: " + PlayerPrefs.GetFloat("HeroPositionY") + "\nPos.z: " + PlayerPrefs.GetFloat("HeroPositionZ"));
            Debug.Log("Current scene: " + data.currentScene);
            return;
        }
        else
        {
            Debug.LogError("Save number " + saveNumber + " not found in " + path);
            return;
        }
    }


    public static void DeleteSave (int saveNumber)
    {
        string path = Application.persistentDataPath + "/UrorumsSave" + saveNumber + ".binsave";
        File.Delete(path);
    }
}

