using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption;
    private readonly string encryptionCodeWord = "SlimesRCute";

    public FileDataHandler(string datadirpath, string datafilename, bool shouldEncrypt)
    {
        this.dataDirPath = datadirpath;
        this.dataFileName = datafilename;
        useEncryption = shouldEncrypt;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                //Load serialized data from file
                string datatoLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        datatoLoad = reader.ReadToEnd();
                    }
                }

                // decrypt the data if option is enabled
                if (useEncryption)
                {
                    datatoLoad = EncryptDecrypt(datatoLoad);
                }

                // deserialize data back into game data
                loadedData = JsonUtility.FromJson<GameData>(datatoLoad);

            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            // Create directory if it does not already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the game data object into Json format
            string dataToStore = JsonUtility.ToJson(data, !useEncryption);

            // Encrypt the data if option is enabled
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // Write file to sile system
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    // Encryption to make files not as easily changeable
    // But congrats if your doing it anyways.
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
