using Assets.Scripts.QuestSystem.Stages;
using UnityEditor;
using System.Collections.Generic;
using Assets.Scripts.QuestSystem;
using UnityEngine;

namespace Assets.CustomDrawers
{
    [CustomEditor(typeof(QuestData))]
    public class QuestStageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            QuestData data = (QuestData)target;

            EditorGUILayout.LabelField("Add quest stage:");

            if (GUILayout.Button("Add Collect stage"))
            {
                data.stages.Add(new CollectStage());
            }

            if (GUILayout.Button("Add Kill stage"))
            {
                data.stages.Add(new KillStage());
            }

            if (GUILayout.Button("Add Interact stage"))
            {
                data.stages.Add(new InteractStage());
            }

            if (GUILayout.Button("Add Travel stage"))
            {
                data.stages.Add(new TravelStage());
            }

            DrawDefaultInspector();
        }
    }
}
