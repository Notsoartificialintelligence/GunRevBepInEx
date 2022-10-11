using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;

namespace GunRev
{
    public class GildedInfusion : PassiveItem
    {
        public GildedInfusion item;
        public static void Register()
        {
            string itemName = "Gilded Infusion";

            string resourceName = "GunRev/Resources/gildedheart";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<GildedInfusion>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "A Healthy Balance";
            string longDesc = "Increases the amount of money the player collects from enemies based on the number of heart containers the player has.\n\nLiquid gold is a popular novelty among billionaires, partly because it's gold, and partly due to the costs needed to keep it a liquid. But mostly because it's gold.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.B;

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            if (player.characterIdentity == PlayableCharacters.Robot)
            {
                player.RemovePassiveItem(this.PickupObjectId);
            }
            
            ETGMod.AIActor.OnPreStart = (Action<AIActor>)Delegate.Combine(ETGMod.AIActor.OnPreStart, new Action<AIActor>(this.Moners));
        }
        public void Moners(AIActor monerman)
        {
            monerman.AssignedCurrencyToDrop += (int)Owner.healthHaver.GetMaxHealth() / 2;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            
            ETGMod.AIActor.OnPreStart = (Action<AIActor>)Delegate.Remove(ETGMod.AIActor.OnPreStart, new Action<AIActor>(this.Moners));
            return base.Drop(player);
        }
    }
}