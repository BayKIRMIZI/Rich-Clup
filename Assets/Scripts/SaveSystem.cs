using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveLevel(GameLevel gameLevel)
    {
        string path = Application.persistentDataPath + "/level.bin";

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Open);
        stream.SetLength(0);
        stream.Close();

        stream = new FileStream(path, FileMode.Append);
        PlayerData data = new PlayerData(gameLevel);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadLevel(GameLevel gameLevel)
    {
        string path = Application.persistentDataPath + "/level.bin";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream;
        PlayerData playerData;

        if (File.Exists(path))
        {
            fileStream = new FileStream(path, FileMode.Open);
            playerData = (PlayerData) formatter.Deserialize(fileStream);
            fileStream.Close();

            Debug.Log("Dosya Okundu");
            return playerData;
        }
        else
        {
            fileStream = new FileStream(path, FileMode.Append);
            playerData = new PlayerData(gameLevel);

            formatter.Serialize(fileStream, playerData);
            fileStream.Close();

            Debug.Log("Dosya Yok. Yeni oluşturuldu");
            return playerData;
        }
    }
}
