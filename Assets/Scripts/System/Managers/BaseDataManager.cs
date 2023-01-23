using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BaseDataManager : MonoBehaviour
{
    public static BaseDataManager Instance;
    private void Awake()
    {
         if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        StatisticsManager.Instance.LoadPlayerData();
    }

    public bool Load(string savePath, MonoBehaviour saveObject)
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), saveObject);
            file.Close();

            Debug.Log("load " + saveObject.name + saveObject.name + " " + Application.persistentDataPath);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Load(string savePath, ScriptableObject saveObject)
    {

        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), saveObject);
            file.Close();

            Debug.Log("load " + saveObject.name + " " + Application.persistentDataPath);
            return true;
        }
        else
        {
            StatisticsManager.Instance.isNewPlayer = true;
            return false;
        }
    }

    public void Save(string savePath, MonoBehaviour saveObject)
    {
        string saveData = JsonUtility.ToJson(saveObject, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
        Debug.Log("save " + saveObject.name + saveObject.name + " " + Application.persistentDataPath);
    }

    public void Save(string savePath, ScriptableObject saveObject)
    {
        string saveData = JsonUtility.ToJson(saveObject, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
        Debug.Log("save " + saveObject.name + " " + Application.persistentDataPath);
    }
}
