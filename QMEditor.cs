using UnityEngine;
using UnityEditor;

namespace Tahsin.Quests
{
    [CustomEditor(typeof(QuestManager))]
    public class QMEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            QuestManager questManager = (QuestManager)target;
            for (int i = 0; i < questManager.quests.Count; i++)
            {
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button($"Remove Quest#{i}"))
                {
                    if (questManager.following.Contains(questManager.quests[i])) 
                    {
                        questManager.following.RemoveAt(i);
                    }
                    questManager.quests.RemoveAt(i);
                    i--;
                }
                GUI.backgroundColor = Color.cyan;
                if (GUILayout.Button("Follow Quest")) 
                {
                    questManager._FollowQuest(questManager.quests[i]);
                }
                questManager.quests[i].OnGUI();
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            GUI.backgroundColor = Color.grey;
            EditorGUILayout.Space(50);
            if (GUILayout.Button("Add"))
            {
                Quest quest = new Quest();
                questManager.quests.Add(quest);
            }
            if (GUILayout.Button("Clear"))
            {
                questManager.quests.Clear();
            }
            for (int i = 0; i < questManager.following.Count; i++) 
            {
                if (GUILayout.Button("Unfollow")) 
                {
                    questManager.following.RemoveAt(i);
                    i--;
                }
                questManager.following[i].OnGUI();
            }
        }
    }
}
