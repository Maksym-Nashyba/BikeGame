using LevelObjectives.Objectives;
using UnityEditor;
using UnityEngine;

namespace EditorWindows
{
    [CustomPropertyDrawer(typeof(CheckpointObjective),  true)]
    public class CheckpointObjectiveProperty : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            EditorGUI.PropertyField(new Rect(position.x, position.y + 20f, position.width, 20f), property.FindPropertyRelative("_checkpoint"));

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 50f;
        }
    }
}