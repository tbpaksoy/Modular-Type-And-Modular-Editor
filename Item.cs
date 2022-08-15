using UnityEditor;
using UnityEngine;

namespace Tahsin
{
    [CreateAssetMenu(fileName = "Item", menuName = "Create Item")]
    public class Item : ScriptableObject
    {
        public Sprite icon;
        public string itemName;
        public int value;

        public void GetReward(int quantity)
        {
            //Whatever we want.
            //This section changes by type.
            //Inherits from IReward.
        }

        public string GetRewardInfo() => itemName + ":" + value.ToString();

        public string GetRewardInfo(int quantity) => GetRewardInfo() + $" x {quantity} = {value * quantity}";

        public Sprite GetRewardSprite() => icon;
        public void OnGUI()
        {
            icon = (Sprite)EditorGUILayout.ObjectField(icon, typeof(Sprite), false);
            itemName = EditorGUILayout.TextField("Name", itemName);
            value = EditorGUILayout.IntField("Value", value);
        }
    }
}
