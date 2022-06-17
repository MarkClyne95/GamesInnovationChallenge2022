using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SC_SaveSystem : MonoBehaviour
{
    public static SC_SaveSystem instance;
    private string filepath;

    private void Awake()
    {
        filepath = Application.persistentDataPath + "/Save.Data";

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame(SC_GameData data)
    {
        FileStream fs = new FileStream(filepath, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(fs, data);

        fs.Close();
    }

    public SC_GameData LoadGame()
    {
        if (File.Exists(filepath))
        {
            //File Does Exist
            FileStream fs = new FileStream(filepath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            SC_GameData SaveData = (SC_GameData)bf.Deserialize(fs);
            fs.Close();

            return SaveData;
        }
        else
        {
            Debug.Log("No File Found " + filepath);
            return null;
        }
    }
}
