using UnityEngine;
using SBPScripts;
using UnityEditor;

public class SaveBicycleReplay : MonoBehaviour
{
    static string json, encodedString;
    static WayPointSystem wayPointSystem;

    [MenuItem("Window/Save Bicycle Replay")]
    static void SaveReplay()
    {
        if (Selection.activeGameObject != null)
        {
            wayPointSystem = Selection.activeGameObject.GetComponent<BicycleController>().WayPointSystem;
            //JSON Implementation
            //json = JsonUtility.ToJson(wayPointSystem);

            //Custom String Implementation - Encoding
            for (int i = 0; i < wayPointSystem.bicyclePositionTransform.Count; i++)
            {
                encodedString += Mathf.Round(wayPointSystem.bicyclePositionTransform[i].x * 1000f) * 0.001f + "," + Mathf.Round(wayPointSystem.bicyclePositionTransform[i].y * 1000f) * 0.001f + "," + Mathf.Round(wayPointSystem.bicyclePositionTransform[i].z * 1000f) * 0.001f + "," + Mathf.Round(wayPointSystem.bicycleRotationTransform[i].x * 1000f) * 0.001f + "," + Mathf.Round(wayPointSystem.bicycleRotationTransform[i].y * 1000f) * 0.001f + "," + Mathf.Round(wayPointSystem.bicycleRotationTransform[i].z * 1000f) * 0.001f + "," + Mathf.Round(wayPointSystem.bicycleRotationTransform[i].w * 1000f) * 0.001f + "," + wayPointSystem.movementInstructionSet[i].x + "," + wayPointSystem.movementInstructionSet[i].y + "," + wayPointSystem.sprintInstructionSet[i].ToString() + "," + wayPointSystem.bHopInstructionSet[i] + ",";
            }

            Debug.Log("<color=green>Gameplay Saved! </color>" + " This run has been saved successfully. Please select the Bicycle Controller and click on " + "<color=blue>Load Bicycle Replay</color>" + " to load replay data");
            wayPointSystem.recordingState = WayPointSystem.RecordingState.DoNothing;
        }
        else
            Debug.Log("<color=yellow>Please select the Bicycle Controller Object to save it's gameplay </color>");
    }
    [MenuItem("Window/Load Bicycle Replay")]
    static void LoadReplay()
    {
        GameObject wPS;
        if (Selection.activeGameObject != null)
        {
            wPS = Selection.activeGameObject;

            //JSON Implementation
            //JsonUtility.FromJsonOverwrite(json,wPS.GetComponent<BicycleController>().wayPointSystem);

            //Custom String Implementation - Decoding
            wPS.GetComponent<BicycleController>().WayPointSystem.bicyclePositionTransform.Clear();
            wPS.GetComponent<BicycleController>().WayPointSystem.bicycleRotationTransform.Clear();
            wPS.GetComponent<BicycleController>().WayPointSystem.movementInstructionSet.Clear();
            wPS.GetComponent<BicycleController>().WayPointSystem.sprintInstructionSet.Clear();
            wPS.GetComponent<BicycleController>().WayPointSystem.bHopInstructionSet.Clear();
            string[] encodedStringArray = encodedString.Split(',');
            for (int i = 0; i < wayPointSystem.bicyclePositionTransform.Count; i++)
            {
                wPS.GetComponent<BicycleController>().WayPointSystem.bicyclePositionTransform.Add(new Vector3(float.Parse(encodedStringArray[i * 11 + 0]), float.Parse(encodedStringArray[i * 11 + 1]), float.Parse(encodedStringArray[i * 11 + 2])));
                wPS.GetComponent<BicycleController>().WayPointSystem.bicycleRotationTransform.Add(new Quaternion(float.Parse(encodedStringArray[i * 11 + 3]), float.Parse(encodedStringArray[i * 11 + 4]), float.Parse(encodedStringArray[i * 11 + 5]), float.Parse(encodedStringArray[i * 11 + 6])));
                wPS.GetComponent<BicycleController>().WayPointSystem.movementInstructionSet.Add(new Vector2Int(int.Parse(encodedStringArray[i * 11 + 7]), int.Parse(encodedStringArray[i * 11 + 8])));
                wPS.GetComponent<BicycleController>().WayPointSystem.sprintInstructionSet.Add(bool.Parse(encodedStringArray[i * 11 + 9]));
                wPS.GetComponent<BicycleController>().WayPointSystem.bHopInstructionSet.Add(int.Parse(encodedStringArray[i * 11 + 10]));
            }
            Debug.Log("<color=green>Data Loaded! </color>" + " Please switch over to " + "<color=blue>PlayBack Mode</color>" + " to review replay");
        }
        else
            Debug.Log("<color=yellow>Please select the Bicycle Controller Object to load data </color>");
    }

}
