using UnityEngine;
using UnityEngine.Animations.Rigging;
using SBPScripts;

namespace UnityEditor.Animations.Rigging
{
    internal static class AnimationRiggingEditorUtils
    {
        internal static class AnimationRiggingContextMenus
        {
            [MenuItem("Window/Cyclist Setup/Setup Selected")]
            static void RigSetup(MenuCommand command)
            {
                GameObject selectedObject = Selection.activeGameObject;
                if (selectedObject == null)
                {
                    Debug.Log("<color=yellow>No Gameobject Selected: </color>Please select your custom character in the hierarchy.");
                }
                
                AnimationRiggingEditorUtils.RigSetup(Selection.activeGameObject.transform);

                GameObject TemplatePrefab = (GameObject) AssetDatabase.LoadAssetAtPath(
                    "Assets/Simple Bicycle Physics/Editor/TemplatePrefab.prefab", typeof(GameObject));
                GameObject originalCyclist = null;
                foreach (Transform child in TemplatePrefab.transform)
                {
                    if (child.name == "EditorCyclist")
                        originalCyclist = child.gameObject;
                }

                //Align Transform
                Vector3 originalPosition = originalCyclist.GetComponent<Transform>().transform.position;
                Quaternion originalRotation = originalCyclist.GetComponent<Transform>().transform.rotation;
                Vector3 originalScale = originalCyclist.GetComponent<Transform>().transform.localScale;
                selectedObject.transform.position = originalPosition;
                selectedObject.transform.rotation = originalRotation;
                selectedObject.transform.localScale = originalScale;

                //Assign Controller
                selectedObject.GetComponent<Animator>().runtimeAnimatorController =
                    originalCyclist.GetComponent<Animator>().runtimeAnimatorController;

                //Add Cyclist Anim Controller
                selectedObject.AddComponent<CyclistAnimController>();

                //Add Procedural IK
                selectedObject.AddComponent<ProceduralIKHandler>();
                System.Reflection.FieldInfo[] fields2 =
                    selectedObject.GetComponent<ProceduralIKHandler>().GetType().GetFields();
                foreach (System.Reflection.FieldInfo field in fields2)
                {
                    field.SetValue(
                        selectedObject.GetComponent(selectedObject.GetComponent<ProceduralIKHandler>().GetType()),
                        field.GetValue(originalCyclist.GetComponent<ProceduralIKHandler>()));
                }

                //Transfer All IK data to Rig 1 (New Rig)
                GameObject originalRig = null;
                foreach (Transform child in originalCyclist.transform)
                {
                    if (child.name == "Procedural Animation Rig")
                        originalRig = child.gameObject;
                }

                GameObject selectedRig = null;
                foreach (Transform child in selectedObject.transform)
                {
                    if (child.name == "Rig 1")
                        selectedRig = child.gameObject;
                }

                for (int i = 0; i < originalRig.transform.childCount; i++)
                {
                    GameObject child = originalRig.transform.GetChild(i).gameObject;
                    GameObject clone = GameObject.Instantiate(child);
                    clone.name = child.name;

                    switch (child.name)
                    {
                        case "HipIK":
                            selectedObject.GetComponent<CyclistAnimController>().hipIK = clone;
                            break;
                        case "ChestIK":
                            selectedObject.GetComponent<CyclistAnimController>().chestIK = clone;
                            break;
                        case "HeadIK":
                            selectedObject.GetComponent<CyclistAnimController>().headIK = clone;
                            break;
                        case "LeftFootIK":
                            selectedObject.GetComponent<CyclistAnimController>().leftFootIK = clone;
                            break;
                        case "LeftFootIdleIK":
                            selectedObject.GetComponent<CyclistAnimController>().leftFootIdleIK = clone;
                            break;
                    }
                        
                    clone.transform.SetParent(selectedRig.transform, true);
                }

                Debug.Log("<color=green>Setup Success! </color>" + selectedObject.name +
                          " has been set up successfully.");
            }

            [MenuItem("Window/Cyclist Setup/Setup IK Target Transforms")]
            static void SetupIKTransforms()
            {
                GameObject TemplatePrefab =
                    (GameObject) AssetDatabase.LoadAssetAtPath(
                        "Assets/Simple Bicycle Physics/Editor/TemplatePrefab.prefab", typeof(GameObject));
                GameObject selectedObject = Selection.activeGameObject;
                // Setup IK Targets
                
                Transform[] allChildren = TemplatePrefab.GetComponentsInChildren<Transform>();
                foreach (Transform eachChild in allChildren)
                {
                    GameObject.Instantiate(eachChild, selectedObject.transform, true);
                }
            }
        }

        private static void RigSetup(Transform transform)
        {
            var rigBuilder = transform.GetComponent<RigBuilder>();

            if (rigBuilder == null)
                rigBuilder = Undo.AddComponent<RigBuilder>(transform.gameObject);
            else
                Undo.RecordObject(rigBuilder, "Rig Builder Component Added.");

            var name = "Rig";
            var cnt = 1;
            while (rigBuilder.transform.Find(string.Format("{0} {1}", name, cnt)) != null)
            {
                cnt++;
            }
            name = string.Format("{0} {1}", name, cnt);
            var rigGameObject = new GameObject(name);
            Undo.RegisterCreatedObjectUndo(rigGameObject, name);
            rigGameObject.transform.SetParent(rigBuilder.transform);

            var rig = Undo.AddComponent<Rig>(rigGameObject);
            rigBuilder.layers.Add(new RigLayer(rig));

            if (PrefabUtility.IsPartOfPrefabInstance(rigBuilder))
                EditorUtility.SetDirty(rigBuilder);
        }
    }
}