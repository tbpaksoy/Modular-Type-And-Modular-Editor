using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tahsin.Tasks;
using Tahsin.Zones;
namespace Tahsin.Quests
{
    public enum RewardType
    {
        Item,Money,Experience,Zone
    }
    public enum QuestType 
    {
        Main,Side
    }
    [System.Serializable]
    public class Quest : ISerializationCallbackReceiver
    {
        public Zone zone;
        public QuestType questType;
        public RewardType rewardType;
        public string title;
        public string description;
        public int Task;
        [SerializeReference]
        public List<Task> tasks = new();


        public float distanceToPlayer => Player.player ? Vector3.Distance(Player.player.transform.position, position) : -1f;
        public int value => 0;
        public Vector3 position;
        public List<(Reward,int)> rewards = new List<(Reward, int)>();

        public List<(Sprite, string)> GetRewardsInfo() 
        {
            List<(Sprite, string)> result = new List<(Sprite, string)>();
            foreach((Reward,int) reward in rewards) 
            {
                result.Add((reward.Item1.GetRewardSprite(), reward.Item1 != null ? reward.Item1.GetRewardInfo(reward.Item2) : null));
            }
            return result;
        }

        #region For Editor Only
        TaskType taskType;
        [SerializeReference]
        private int[] _quanitites;
        [SerializeReference]
        private Reward[] _rewards;
        bool _show;
        [SerializeReference]
        private Zone _zone;
        [SerializeReference]
        private Task[] _tasks = new Task[1];
        #endregion

        public void AddQuest() 
        {
            QuestManager.AddQuest(this);
        }
        public void DoneQuest() 
        {
            foreach((Reward,int) reward in rewards) 
            {
                reward.Item1.GetReward(reward.Item2);
            }
            QuestManager.RemoveQuest(this);
        }
        public void OnGUI() 
        {
            _show = EditorGUILayout.BeginFoldoutHeaderGroup(_show, "Show");
            if (_show)
            {
                EditorGUILayout.LabelField("Zone");
                zone.OnGUI();
                EditorGUI.indentLevel++;
                questType = (QuestType)EditorGUILayout.EnumPopup("Mission Type", questType);
                title = EditorGUILayout.TextField(title);
                description = EditorGUILayout.TextField(description, GUILayout.Height(100));
                position = EditorGUILayout.Vector3Field("Position", position);
                taskType = (TaskType)EditorGUILayout.EnumPopup("Task Type", taskType);
                EditorGUILayout.LabelField("Task(s)");
                GUI.backgroundColor = Color.green;
                if(GUILayout.Button("Add Task")) 
                {
                    switch (taskType) 
                    {
                        case TaskType.CollectItem:
                            tasks.Add(new CollectItemTask());
                            break;
                        case TaskType.KillDesignatedEnemy:
                            tasks.Add(new KillDesignatedEnemyTask());
                            break;
                        case TaskType.KillEnemy:
                            tasks.Add(new KillEnemyTask());
                            break;
                        case TaskType.AttackZone:
                            tasks.Add(new AttackZoneTask());
                            break;
                        case TaskType.DefendZone:
                            tasks.Add(new DefendZoneTask());
                            break;
                    }
                }
                GUI.backgroundColor = Color.grey;
                EditorGUI.indentLevel++;
                for (int i = 0; i < tasks.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUI.backgroundColor = Color.red;
                    bool remove = GUILayout.Button("Remove Task");
                    GUI.backgroundColor = Color.grey;
                    EditorGUILayout.LabelField("Task #" + i.ToString() + "(" + tasks[i]?.GetType().Name + ")");
                    GUILayout.EndHorizontal();
                    tasks[i]?.OnGUI();
                    if (remove) 
                    {
                        tasks.RemoveAt(i);
                        i--;
                    }
                }
                EditorGUI.indentLevel--;
                rewardType = (RewardType)EditorGUILayout.EnumPopup("Reward Type",rewardType);
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Add Reward"))
                {
                    switch (rewardType) 
                    {
                        case RewardType.Item:
                            rewards.Add((new ItemReward(),0));
                            break;
                        case RewardType.Experience:
                            rewards.Add((new ExperienceReward(), 0));
                            break;
                        case RewardType.Money:
                            rewards.Add((new MoneyReward(), 0));
                            break;
                        case RewardType.Zone:
                            rewards.Add((new ZoneReward(), 0));
                            break;
                    }
                }
                GUI.backgroundColor = Color.grey;
                EditorGUI.indentLevel++;
                for (int i = 0; i < rewards.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUI.backgroundColor = Color.red;
                    bool remove = GUILayout.Button("Remove Reward");
                    GUI.backgroundColor = Color.grey;
                    EditorGUILayout.LabelField($"Reward #{i} ({(rewards[i].Item1 == null ? null : rewards[i].Item1.GetType().Name)})");
                    GUILayout.EndHorizontal();
                    EditorGUI.indentLevel++;
                    int quantity = EditorGUILayout.IntField("Quantity ", rewards[i].Item2);
                    Reward reward = rewards[i].Item1;
                    reward.OnGUI();
                    rewards[i] = (reward, quantity);
                    if (remove)
                    {
                        rewards.RemoveAt(i);
                        i--;
                    }
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
                GUI.backgroundColor = Color.grey;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        public  Sprite GetRewardSprite() => null; // Just for now.
        public  string GetRewardInfo() => $"Mission :{title} ({questType} Mission)";
        public  void GetReward(int quantity) => AddQuest();
        public string GetRewardInfo(int quantity) => GetRewardInfo();

        public void OnBeforeSerialize()
        {
            int count = rewards.Count;
            _quanitites = new int[count];
            _rewards = new Reward[count];
            for (int i = 0;i < count; i++) 
            {
                _quanitites[i] = rewards[i].Item2;
                _rewards[i] = rewards[i].Item1;
            }
            _tasks = tasks.ToArray();
            _zone = zone;
        }

        public void OnAfterDeserialize()
        {
            tasks.Clear();
            rewards.Clear();
            for (int i = 0; i < _quanitites.Length; i++)
            {
                rewards.Add((_rewards[i],_quanitites[i]));
            }
            foreach(Task task in _tasks)
            {
                tasks.Add(task);
            }
            zone = _zone;
        }
    }
}