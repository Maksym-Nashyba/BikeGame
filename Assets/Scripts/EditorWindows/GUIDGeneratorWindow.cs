using IGUIDResources;
using UnityEditor;
using UnityEngine;

namespace EditorWindows
{
    public class GUIDGeneratorWindow : EditorWindow
    {
        private string _lastGenerated;
        
        [MenuItem("Window/GUIDGenerator")]
        private static void ShowWindow()
        {
            Rect windowPosition = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 70, 400, 140);
            GUIDGeneratorWindow window = GetWindowWithRect<GUIDGeneratorWindow>(windowPosition);
            window.titleContent = new GUIContent("GUIDs");
            window.GenerateNewGUID();
            window.Show();
        }

        private void OnGUI()
        {
            _lastGenerated = GUILayout.TextField(_lastGenerated);
            if (GUILayout.Button("GENERATE"))
            {
                GenerateNewGUID();
            }
            if (GUILayout.Button("COPY"))
            {
                CopyCurrentGUIDToClipboard();
            }
            GUILayout.Space(50);
            if (GUILayout.Button("CLOSE"))
            {
                Close();
            }
        }

        private void GenerateNewGUID()
        {
            _lastGenerated = GUIDs.GetNew();
        }

        private void CopyCurrentGUIDToClipboard()
        {
            GUIUtility.systemCopyBuffer = _lastGenerated;
        }
    }
}