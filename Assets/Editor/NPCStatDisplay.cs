using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne.AI;
using UnityEditor;

[CustomEditor(typeof(NPCBehaviour))]
public class NPCStatDisplay : Editor
{
    private void OnSceneGUI()
    {
        NPCBehaviour npcBehaviour = (NPCBehaviour)target;
        if (npcBehaviour == null)
            return;


        BehaviourSkill skill = npcBehaviour.NPCState();
        Action action = npcBehaviour.NPCCurrentAction();

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        Handles.Label(npcBehaviour.transform.position + new Vector3(0, 2.5f, 0), ("skill: " + skill.ToString() + "\n action: " + action.ToString()), style);


    }
}
