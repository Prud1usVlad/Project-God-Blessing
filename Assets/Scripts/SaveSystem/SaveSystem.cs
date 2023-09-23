using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.SaveSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SaveSystem", fileName = "SaveSystem")]
    public class SaveSystem : ScriptableObject
    {
        public bool SaveData<T>(T file, string fileName = "save.json")
                where T : class
        {
            string dirPath = Application.persistentDataPath + "/Saves";
            string path = dirPath + "/" + fileName;
            string data = JsonUtility.ToJson(file);

            string reserveCopy = "";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            if (File.Exists(path))
            {
                reserveCopy = File.ReadAllText(path);
            }

            try
            {
                File.WriteAllText(path, data);

                return true;
            }
            catch (Exception ex)
            {
                if (File.Exists(path))
                {
                    File.WriteAllText(path, reserveCopy);
                }

                Debug.Log(ex.Message);

                return false;
            }
        }

        public T LoadData<T>(string fileName)
            where T : class
        {
            string path = Application.persistentDataPath + "/Saves/" + fileName;
            T res = null;

            if (File.Exists(path))
            {
                string data = File.ReadAllText(path);
                res = JsonUtility.FromJson<T>(data);
            }
            else
            {
                Debug.Log("File does not exist");
            }

            return res;
        }

        public bool LoadData<T>(string fileName, T toOverwrite)
            where T : class
        {
            string path = Application.persistentDataPath + "/Saves/" + fileName;

            if (File.Exists(path))
            {
                string data = File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(data, toOverwrite);

                return true;
            }
            else
            {
                Debug.Log("File does not exist");

                return false;
            }
        }

        public T LoadLast<T>()
            where T : class
        {
            string dirPath = Application.persistentDataPath + "/Saves/";
            var directory = new DirectoryInfo(dirPath);

            if (!Directory.Exists(dirPath))
                return null;

            var file = directory.GetFiles()
                .OrderByDescending(f => f.LastWriteTime)
                .First();

            if (file == null)
                return null;

            return LoadData<T>(file.Name); 
        }

        public void DeleteFile(string fileName)
        {
            string dirPath = Application.persistentDataPath + "/Saves";
            string path = dirPath + "/" + fileName;

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                Debug.Log($"No file with name {fileName} found");
            }
        }

        public void DeleteOldFiles(string prefix = null, int filesCap = 10)
        {
            string dirPath = Application.persistentDataPath + "/Saves/";

            if (Directory.Exists(dirPath))
            {
                var directory = new DirectoryInfo(dirPath);
                IEnumerable<FileInfo> files = directory.GetFiles();

                if (prefix is not null)
                    files = files.Where(f => f.Name.StartsWith(prefix));

                files.OrderByDescending(f => f.LastWriteTime);

                for (int i = files.Count(); i > filesCap; i--)
                {
                    files.ElementAt(i).Delete();
                }
            }
        }
    }
}
