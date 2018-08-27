using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Serialization;

public struct SaveGameData
{
    public int heighestLevelID;
    public Vector3 playerPos;
}

public static class SaveGame
{
    private static string filename = "savegame.sav";
    public static bool doSave;
    private static SaveGameData data;

    private static void CompressDataToSave(int levelId, Vector3 playerPosition)
    {
        data.heighestLevelID = levelId;
        data.playerPos = playerPosition;
    }

    private static void LoadData(SaveGameData loadData)
    {
        LevelManager.highestUnlocked = loadData.heighestLevelID;

        //LevelManager.player.pos = loadData.playerPos;
    }

    public static void Save()
    {
        if (doSave)
        {
            CompressDataToSave(LevelManager.highestUnlocked, LevelManager.player.pos);

            IAsyncResult result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            StorageDevice device = StorageDevice.EndShowSelector(result);
            result = device.BeginOpenContainer("StorageDemo", null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = device.EndOpenContainer(result);
            if (container.FileExists(filename))
                container.DeleteFile(filename);
            Stream stream = container.CreateFile(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            serializer.Serialize(stream, data);
            stream.Close();
            container.Dispose();
            doSave = false;
        }
    }
    public static void Load()
    {
        IAsyncResult result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
        StorageDevice device = StorageDevice.EndShowSelector(result);
        result = device.BeginOpenContainer("StorageDemo", null, null);
        // Wait for the WaitHandle to become signaled.
        result.AsyncWaitHandle.WaitOne();

        StorageContainer container = device.EndOpenContainer(result);

        // Close the wait handle.
        result.AsyncWaitHandle.Close();
        // Check to see whether the save exists.
        if (!container.FileExists(filename))
        {
            // If not, dispose of the container and return.
            container.Dispose();
            return;
        }
        // Open the file.
        Stream stream = container.OpenFile(filename, FileMode.Open);
        XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
        SaveGameData data = (SaveGameData)serializer.Deserialize(stream);
        // Close the file.
        stream.Close();
        // Dispose the container.
        container.Dispose();
        LoadData(data);
        
    }
}
