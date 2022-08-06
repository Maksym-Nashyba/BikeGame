using SaveSystem.Front;
using UnityEditor;
using UnityEngine;

namespace SaveSystem.Editor
{
    public class SavesWindow : EditorWindow
    {
        private Saves saves;
        
        [MenuItem("Window/Saves")]
        private static void ShowWindow()
        {
            var window = GetWindow<SavesWindow>();
            window.titleContent = new GUIContent("Saves Editor");
            window.Show();
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void OnGUI()
        {
            
        }
    }
}