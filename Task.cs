using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tahsin.Tasks
{
    public enum TaskType 
    {
        KillEnemy, KillDesignatedEnemy, CollectItem, AttackZone, DefendZone
    }
    [System.Serializable]
    public abstract class Task : ISerializationCallbackReceiver
    {
        public abstract bool IsDone();
        public abstract string GetTaskInfo();
        public abstract void OnGUI();

        public abstract void OnBeforeSerialize();

        public abstract void OnAfterDeserialize();
    }
    public class KillEnemyTask : Task
    {
        [SerializeReference]
        public int goal;
        [SerializeReference]
        public int current;
        [SerializeReference]
        private int _goal, _current;
        public int left => goal - current;
        public override bool IsDone()
        {
            return left <= 0;
        }
        public override string GetTaskInfo()
        {
            return "Kill enemy : " + goal + " (" + left + " is left) ";
        }
        public override void OnGUI()
        {
            EditorGUI.indentLevel++;
            goal = EditorGUILayout.IntField("Goal", goal);
            current = EditorGUILayout.IntField("Current", current);
            EditorGUILayout.IntField("Left", left);
            EditorGUI.indentLevel--;
        }

        public override void OnBeforeSerialize()
        {
            _current = current;
            _goal = goal;
        }

        public override void OnAfterDeserialize()
        {
            goal = _goal;
            current = _current;
        }
    }
    public class KillDesignatedEnemyTask : Task
    {
        public Enemy enemy;
        [SerializeReference]
        private Enemy _enemy;
        public override string GetTaskInfo()
        {
            return $"Kill {enemy.name}";
        }

        public override bool IsDone()
        {
            return enemy == null;
        }

        public override void OnAfterDeserialize()
        {
            enemy = _enemy;
        }

        public override void OnBeforeSerialize()
        {
            _enemy = enemy;
        }

        public override void OnGUI()
        {
            EditorGUI.indentLevel++;
            enemy = (Enemy)EditorGUILayout.ObjectField("Enemy ",enemy,typeof(Enemy),true);
            EditorGUI.indentLevel--;
        }
    }
    public class CollectItemTask : Task
    {
        public Item itemType;
        public int amount, collected;
        [SerializeReference]
        public int _amount, _collected;
        public int left => amount - collected;
        public override string GetTaskInfo()
        {
            return $"{itemType.itemName} : Collect {collected}/{amount}({left} is left)";
        }

        public override bool IsDone()
        {
            return  left < 0;
        }

        public override void OnAfterDeserialize()
        {
            collected = _collected;
            amount = _amount;
        }

        public override void OnBeforeSerialize()
        {
            _amount = amount;
            _collected = collected;
        }

        public override void OnGUI()
        {
            EditorGUI.indentLevel++;
            itemType = EditorGUILayout.ObjectField("Item Type", itemType, typeof(Item), true) as Item;
            amount = EditorGUILayout.IntField("Amount ",amount);
            collected = EditorGUILayout.IntField("Collected ", collected);
            EditorGUILayout.IntField("Left", left);
            EditorGUI.indentLevel--;
        }
    }
    public class DefendZoneTask : Task
    {

        public override string GetTaskInfo() => "Defend Zone";

        public override bool IsDone()
        {
            return true;
        }

        public override void OnAfterDeserialize()
        {

        }

        public override void OnBeforeSerialize()
        {

        }

        public override void OnGUI()
        {

        }
    }
    public class AttackZoneTask : Task
    {
        public override string GetTaskInfo() => "Attack Zone";

        public override bool IsDone()
        {
            return true;
        }

        public override void OnAfterDeserialize()
        {
        }

        public override void OnBeforeSerialize()
        {
        }

        public override void OnGUI()
        {
        }
    }
}