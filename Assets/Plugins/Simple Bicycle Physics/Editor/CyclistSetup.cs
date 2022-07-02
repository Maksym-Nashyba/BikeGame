using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using SBPScripts;
using UnityEditor;

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
                if(selectedObject!=null)
                {
                AnimationRiggingEditorUtils.RigSetup(Selection.activeGameObject.transform);

                GameObject TemplatePrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Simple Bicycle Physics/Editor/TemplatePrefab.prefab", typeof(GameObject));
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
                selectedObject.GetComponent<Animator>().runtimeAnimatorController = originalCyclist.GetComponent<Animator>().runtimeAnimatorController;

                //Add Cyclist Anim Controller
                selectedObject.AddComponent<CyclistAnimController>();

                //Add Procedural IK
                selectedObject.AddComponent<ProceduralIKHandler>();
                System.Reflection.FieldInfo[] fields2 = selectedObject.GetComponent<ProceduralIKHandler>().GetType().GetFields();
                foreach (System.Reflection.FieldInfo field in fields2)
                {
                    field.SetValue(selectedObject.GetComponent(selectedObject.GetComponent<ProceduralIKHandler>().GetType()), field.GetValue(originalCyclist.GetComponent<ProceduralIKHandler>()));
                }

                //Transfer All IK data to Rig 1 (New Rig)
                GameObject originalRig = null;
                foreach(Transform child in originalCyclist.transform)
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

                    if (child.name == "HipIK")
                        selectedObject.GetComponent<CyclistAnimController>().hipIK = clone;
                    else if (child.name == "ChestIK")
                        selectedObject.GetComponent<CyclistAnimController>().chestIK = clone;
                    else if (child.name == "HeadIK")
                        selectedObject.GetComponent<CyclistAnimController>().headIK = clone;
                    else if (child.name == "LeftFootIK")
                        selectedObject.GetComponent<CyclistAnimController>().leftFootIK = clone;
                    else if (child.name == "LeftFootIdleIK")
                        selectedObject.GetComponent<CyclistAnimController>().leftFootIdleIK = clone;

                    clone.transform.SetParent(selectedRig.transform, true);
                }

                Debug.Log("<color=green>Setup Success! </color>" + selectedObject.name + " has been set up successfully.");
            }
            else
            {
                Debug.Log("<color=yellow>No Gameobject Selected: </color>Please select your custom character in the hierarchy.");
            }
            }

            [MenuItem("Window/Cyclist Setup/Setup IK Target Transforms")]
            static void SetupIKTransforms()
            {
                GameObject TemplatePrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Simple Bicycle Physics/Editor/TemplatePrefab.prefab", typeof(GameObject));
                GameObject selectedObject = Selection.activeGameObject;
                // Setup IK Targets
                GameObject RightHandIKTarget, LeftHandIKTarget, LeftFootIKTarget, RightFootIKTarget, LeftFootIdleIKTarget, HeadIKTarget, ChestIKTarget, HipIKTarget;
                Transform[] allChildren = TemplatePrefab.GetComponentsInChildren<Transform>();
                foreach (Transform eachChild in allChildren)
                {
                    if (eachChild.name == "RightHandIKTarget")
                    {
                        RightHandIKTarget = eachChild.gameObject;
                        var ikTransform = GameObject.Instantiate(RightHandIKTarget);
                        ikTransform.transform.SetParent(selectedObject.transform,true);
                        ikTransform.name = "RightHandIKTarget";
                    }

                    if (eachChild.name == "LeftHandIKTarget")
                    {
                        LeftHandIKTarget = eachChild.gameObject;
                        var ikTransform = GameObject.Instantiate(LeftHandIKTarget);
                        ikTransform.transform.SetParent(selectedObject.transform,true);
                        ikTransform.name = "LeftHandIKTarget";
                    }

                    if (eachChild.name == "LeftFootIKTarget")
                    {
                        LeftFootIKTarget = eachChild.gameObject;
                        var ikTransform = GameObject.Instantiate(LeftFootIKTarget);
                        ikTransform.transform.SetParent(selectedObject.transform,true);
                        ikTransform.name = "LeftFootIKTarget";
                    }

                    if (eachChild.name == "RightFootIKTarget")
                    {
                        RightFootIKTarget = eachChild.gameObject;
                        var ikTransform = GameObject.Instantiate(RightFootIKTarget);
                        ikTransform.transform.SetParent(selectedObject.transform,true);
                        ikTransform.name = "RightFootIKTarget";
                    }

                    if (eachChild.name == "LeftFootIdleIKTarget")
                    {
                        LeftFootIdleIKTarget = eachChild.gameObject;
                        var ikTransform = GameObject.Instantiate(LeftFootIdleIKTarget);
                        ikTransform.transform.SetParent(selectedObject.transform,true);
                        ikTransform.name = "LeftFootIdleIKTarget";
                    }

                    if (eachChild.name == "HeadIKTarget")
                    {
                        HeadIKTarget = eachChild.gameObject;
                        var ikTransform = GameObject.Instantiate(HeadIKTarget);
                        ikTransform.transform.SetParent(selectedObject.transform,true);
                        ikTransform.name = "HeadIKTarget";
                    }

                    if (eachChild.name == "ChestIKTarget")
                    {
                        ChestIKTarget = eachChild.gameObject;
                        var ikTransform = GameObject.Instantiate(ChestIKTarget);
                        ikTransform.transform.SetParent(selectedObject.transform,true);
                        ikTransform.name = "ChestIKTarget";
                    }

                    if (eachChild.name == "HipIKTarget")
                    {
                        HipIKTarget = eachChild.gameObject;
                        var ikTransform = GameObject.Instantiate(HipIKTarget);
                        ikTransform.transform.SetParent(selectedObject.transform,true);
                        ikTransform.name = "HipIKTarget";
                    }
                    
                }
            }


        }


        public static void RigSetup(Transform transform)
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

        public static void BoneRendererSetup(Transform transform)
        {
            var boneRenderer = transform.GetComponent<BoneRenderer>();
            if (boneRenderer == null)
                boneRenderer = Undo.AddComponent<BoneRenderer>(transform.gameObject);
            else
                Undo.RecordObject(boneRenderer, "Bone renderer setup.");

            var animator = transform.GetComponent<Animator>();
            var renderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            var bones = new List<Transform>();
            if (animator != null && renderers != null && renderers.Length > 0)
            {
                for (int i = 0; i < renderers.Length; ++i)
                {
                    var renderer = renderers[i];
                    for (int j = 0; j < renderer.bones.Length; ++j)
                    {
                        var bone = renderer.bones[j];
                        if (!bones.Contains(bone))
                        {
                            bones.Add(bone);

                            for (int k = 0; k < bone.childCount; k++)
                            {
                                if (!bones.Contains(bone.GetChild(k)))
                                    bones.Add(bone.GetChild(k));
                            }
                        }
                    }
                }
            }
            else
            {
                bones.AddRange(transform.GetComponentsInChildren<Transform>());
            }

            boneRenderer.transforms = bones.ToArray();

            if (PrefabUtility.IsPartOfPrefabInstance(boneRenderer))
                EditorUtility.SetDirty(boneRenderer);
        }

        public static void RestoreBindPose(Transform transform)
        {
            var animator = transform.GetComponentInParent<Animator>();
            var root = (animator) ? animator.transform : transform;
            var renderers = root.GetComponentsInChildren<SkinnedMeshRenderer>();

            if (renderers.Length == 0)
            {
                Debug.LogError(
                    string.Format(
                        "Could not restore bind pose because no SkinnedMeshRenderers " +
                        "were found  on {0} or any of its children.", root.name));
                return;
            }

            Undo.RegisterFullObjectHierarchyUndo(root.gameObject, "Restore bind pose");

            var bones = new Dictionary<Transform, Matrix4x4>();
            foreach (var renderer in renderers)
            {
                for (int i = 0; i < renderer.bones.Length; ++i)
                {
                    if (!bones.ContainsKey(renderer.bones[i]))
                        bones.Add(renderer.bones[i], renderer.sharedMesh.bindposes[i]);
                }
            }

            var transforms = transform.GetComponentsInChildren<Transform>();
            var restoredPose = false;
            foreach (var t in transforms)
            {
                if (!bones.ContainsKey(t))
                    continue;

                // The root bone is the only bone in the skeleton
                // hierarchy that does not have a parent bone.
                var isRootBone = !bones.ContainsKey(t.parent);

                var matrix = bones[t];
                var wMatrix = matrix.inverse;

                if (!isRootBone)
                {
                    if (t.parent)
                        matrix *= bones[t.parent].inverse;
                    matrix = matrix.inverse;

                    t.localScale = new Vector3(
                        matrix.GetColumn(0).magnitude,
                        matrix.GetColumn(1).magnitude,
                        matrix.GetColumn(2).magnitude
                        );
                    t.localPosition = matrix.MultiplyPoint(Vector3.zero);
                }
                t.rotation = wMatrix.rotation;

                restoredPose = true;
            }

            if (!restoredPose)
            {
                Debug.LogWarning(
                    string.Format(
                        "No valid bindpose(s) have been found for the selected transform: {0}.",
                        transform.name));
            }
        }
    }
}
