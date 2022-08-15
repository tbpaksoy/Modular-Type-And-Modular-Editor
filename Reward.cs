using UnityEngine;
using UnityEditor;
using Tahsin.Quests;
using Tahsin.Zones;
namespace Tahsin
{
    [System.Serializable]
    public abstract class Reward
    {
        public abstract int value { get; }
        public abstract Sprite GetRewardSprite();
        public abstract string GetRewardInfo();
        public abstract string GetRewardInfo(int quantity);
        public abstract void GetReward(int quantity);
        public abstract void OnGUI();
    }
    public class ItemReward : Reward, ISerializationCallbackReceiver
    {
        public Item item;
        [SerializeReference]
        private Item _item;
        public override int value => item.value;

        public override void GetReward(int quantity)
        {
            //TODO;
        }

        public override string GetRewardInfo() => item.GetRewardInfo();

        public override string GetRewardInfo(int quantity) => item.GetRewardInfo(quantity);

        public override Sprite GetRewardSprite() => item.icon;

        public void OnAfterDeserialize()
        {
            item = _item;
        }

        public void OnBeforeSerialize()
        {
            _item = item;
        }

        public override void OnGUI()
        {
            item = EditorGUILayout.ObjectField("Item", item, typeof(Item), false) as Item;
        }
    }
    public class MoneyReward : Reward
    {
        public override int value => 1;

        public override void GetReward(int quantity)
        {
            //TODO
        }

        public override string GetRewardInfo() => null;

        public override string GetRewardInfo(int quantity) => $"Money : {quantity}";

        public override Sprite GetRewardSprite() => Resources.Load("dollar_sign", typeof(Sprite)) as Sprite;

        public override void OnGUI()
        {
        }
    }
    public class ExperienceReward : Reward
    {
        public override int value => 0;

        public override void GetReward(int quantity)
        {
            //TODO
        }

        public override string GetRewardInfo() => null;

        public override string GetRewardInfo(int quantity) => $"Experience : {quantity}";

        public override Sprite GetRewardSprite() => Resources.Load("XP", typeof(Sprite)) as Sprite;

        public override void OnGUI()
        {
        }
    }
    public class QuestReward : Reward,ISerializationCallbackReceiver
    {
        public Quest quest;
        [SerializeReference]
        private Quest _quest;
        public override int value => 0;

        public override void GetReward(int quantity) => quest.GetReward(quantity);

        public override string GetRewardInfo() => quest.GetRewardInfo();

        public override string GetRewardInfo(int quantity) => quest.GetRewardInfo(quantity);

        public override Sprite GetRewardSprite() => quest.GetRewardSprite();

        public void OnAfterDeserialize()
        {
            _quest = quest;
        }

        public void OnBeforeSerialize()
        {
            quest = _quest;
        }

        public override void OnGUI()
        {
        }
    }
    public class ZoneReward : Reward, ISerializationCallbackReceiver
    {
        public Zone zone;
        [SerializeReference]
        private Zone _zone;
        public override int value => 0;
        public Vector3 position => zone.obj == null ? new Vector3() : zone.obj.transform.position ;
        public override void GetReward(int quantity) => zone.UnlockZone();


        public override string GetRewardInfo() => $"Unlocks Zone : {zone.name}";

        public override string GetRewardInfo(int quantity) => GetRewardInfo();

        public override Sprite GetRewardSprite() => zone.icon;

        public void OnAfterDeserialize()
        {
            zone = _zone;
        }

        public void OnBeforeSerialize()
        {
            _zone = zone;
        }

        public override void OnGUI()
        {
            zone.OnGUI();
        }
    }
}

