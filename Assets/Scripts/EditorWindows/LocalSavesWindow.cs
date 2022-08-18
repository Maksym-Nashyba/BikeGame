using System;
using System.Diagnostics;
using System.IO;
using SaveSystem.Front;
using SaveSystem.PersistencyAndSerialization;
using UnityEditor;
using UnityEngine;

namespace EditorWindows
{
    public class LocalSavesWindow : EditorWindow
    {
        private static readonly string FilePath = Application.persistentDataPath + "/Saves/SaveFile.ngr";
        private Saves _saves;

        [MenuItem("Window/LocalSaves")]
        private static void ShowWindow()
        {
            LocalSavesWindow window = GetWindow<LocalSavesWindow>();
            window.titleContent = new GUIContent("Saves");
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Open Folder"))
            {   
                OpenFolderInExplorer();
            }
            
            if (GUILayout.Button("Clear"))
            {
                DeleteSaveFile();
            }
            
            if (GUILayout.Button("Pull"))
            {
                _saves = GetSaves();
            }
            
            if(_saves is null) return;
            
            GUILayout.Label($"{_saves.Bikes.GetAllUnlockedBikes()[0].GUID}");
        }

        private Saves GetSaves()
        {
            if (EditorApplication.isPlaying)
            {
                Saves saves = FindObjectOfType<Saves>();
                if (saves is null) throw new Exception("Couldn't find saves object");
                return saves;
            }
            else
            {
                ISaveDataSerializer serializer = new BinarySaveDataSerializer();
                IPersistencyProvider<ISaveDataSerializer> persistencyProvider = new LocalFilePersistency<ISaveDataSerializer>(serializer);
                Saves saves = new GameObject().AddComponent<Saves>();
                saves.Initialize(persistencyProvider);
                
                if (saves is null) throw new Exception("Couldn't initialize saves object");
                return saves;
            }
        }

        private void DeleteSaveFile()
        {
            if (!File.Exists(FilePath)) return;
            File.Delete(FilePath);
        }

        private void OpenFolderInExplorer()
        {
            string path = Application.persistentDataPath + @"\Saves";
            path = path.Replace("/", @"\");
            Process.Start(path);
        }

        private void OnDisable()
        {
            if(_saves is not null) DestroyImmediate(_saves.gameObject);
        }
    }
}