using System;
using System.Diagnostics;
using System.IO;
using SaveSystem.Front;
using SaveSystem.Models;
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

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += _ => Close();
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

            if (GUILayout.Button("Display current SaveData"))
            {
                _saves = GetSaves();
            }

            if (_saves is null) return;
            DisplayCurrentSave();
        }

        private void DisplayCurrentSave()
        {
            GUILayout.Space(10f);
            DisplayCurrencies();
            GUILayout.Label("_____________________________________________________________________");
            DisplayCareer();
            GUILayout.Label("_____________________________________________________________________");
            DisplayBikes();
        }

        private void DisplayCurrencies()
        {
            SavedCurrencies currencies = _saves.Currencies;
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Dollans: {currencies.GetDollans()}");
            GUILayout.Label($"Pedals: {currencies.GetPedals()}");
            GUILayout.EndHorizontal();
        }

        private void DisplayCareer()
        {
            GUILayout.Label("Levels: ", EditorStyles.boldLabel);
            PersistentLevel[] levels = _saves.Career.GetAllCompletedLevels();
            if(levels is null) return;
            foreach (PersistentLevel level in levels)
            {
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    GUILayout.Label($"GUID: {level.GUID}");
                    GUILayout.Label($"Best time: {level.BestTime}");
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Pedal: ", GUILayout.Width(50f));
                    GUILayout.Toggle(level.PedalCollected, "");
                    GUILayout.EndHorizontal();
                }
            }
        }

        private void DisplayBikes()
        {
            GUILayout.Label("Bikes: ", EditorStyles.boldLabel);
            PersistentBike[] bikes = _saves.Bikes.GetAllUnlockedBikes();

            foreach (PersistentBike bike in bikes)
            {
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    GUILayout.Label($"GUID: {bike.GUID}");
                    GUILayout.Label($"Selected skin: {bike.SelectedSkinGUID}");
                    GUILayout.Label("Bought skins:");
                    using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        foreach (string skinGUID in bike.UnlockedSkins)
                        {
                            GUILayout.Label(skinGUID);
                        }
                    }
                }
            }
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
                IPersistencyProvider<ISaveDataSerializer> persistencyProvider =
                    new LocalFilePersistency<ISaveDataSerializer>(serializer);
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
            if (_saves is not null) DestroyImmediate(_saves.gameObject);
        }
    }
}