using System.Diagnostics;
using SaveSystem.PersistencyAndSerialization;
using UnityEditor;
using UnityEngine;

namespace SaveSystem.Editor
{
    public class SavesFolderOpener : EditorWindow
    {
        [MenuItem("SavesFolder/Open")]
        private static void ShowWindow()
        {
            string path = Application.persistentDataPath + @"\Saves";
            path = path.Replace("/", @"\");
            Process.Start(path);
        }
    }
}