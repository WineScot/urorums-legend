using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayer (int saveNumber, HeroManager hero)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/UrorumsSave" + saveNumber + ".binsave";
        FileStream stream = new FileStream(path, FileMode.Create);

        SavePlayerData data = new SavePlayerData(hero);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Game progress saved successfully!");
    }

    // 
    public static void LoadPlayer(int saveNumber)
    {
        string path = Application.persistentDataPath + "/UrorumsSave" + saveNumber + ".binsave";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SavePlayerData data = formatter.Deserialize(stream) as SavePlayerData;
            stream.Close();

            PlayerPrefs.SetInt("CurrentSave", saveNumber);
            PlayerPrefs.SetFloat("HeroHealth", data.health);
            PlayerPrefs.SetFloat("HeroPositionX", data.position[0]);
            PlayerPrefs.SetFloat("HeroPositionY", data.position[1]);
            PlayerPrefs.SetFloat("HeroPositionZ", data.position[2]);

            PlayerPrefs.SetInt("CurrentScene", data.currentScene);

            UnityEngine.SceneManagement.SceneManager.LoadScene(data.currentScene);
            Debug.Log("hero health: " + PlayerPrefs.GetFloat("HeroHealth") + "\nPos.x: " + PlayerPrefs.GetFloat("HeroPositionX") + "\nPos.y: " + PlayerPrefs.GetFloat("HeroPositionY") + "\nPos.z: " + PlayerPrefs.GetFloat("HeroPositionZ"));
            Debug.Log("Current scene: " + data.currentScene);
            return;   //return data;
        }
        else
        {
            Debug.LogError("Save number " + saveNumber + " not found in " + path);
            return;   //return null;
        }
    }
}

