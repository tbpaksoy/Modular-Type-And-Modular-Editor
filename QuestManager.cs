using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace Tahsin.Quests
{
    [DisallowMultipleComponent]
    public class QuestManager : MonoBehaviour , ISerializationCallbackReceiver
    {
        public static QuestManager manager;
        [SerializeReference]
        public List<Quest> quests = new List<Quest>();
        [SerializeReference]
        public List<Quest> following = new List<Quest>();
        [SerializeReference]
        private List<Quest> _quests, _following;
        private const int maxFollow = 5;
        public static Quest active;
        private void Awake()
        {
            manager = this;
        }
        public void _FollowQuest(Quest quest) 
        {
            if (following.Contains(quest))
            {
                following.Add(quest);
                if (following.Count > maxFollow)
                {
                    following.RemoveAt(0);
                }
            }
        }
        public static void AddQuest(Quest quest) 
        {
            if (!manager.quests.Contains(quest)) 
            {
                manager.quests.Add(quest);
            }
        }
        public static void RemoveQuest(Quest quest) 
        {
            manager.quests.Remove(quest);
        }

        public void OnBeforeSerialize()
        {
            _quests = quests;
            _following = following;
        }

        public void OnAfterDeserialize()
        {
            quests = _quests;
            following = _following;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (Quest quest in quests) 
            {
                Gizmos.DrawSphere(quest.position, 2);
            }
        }
    }
}
