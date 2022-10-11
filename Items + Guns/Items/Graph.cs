using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class Graph : PassiveItem
    {
        public static void Register()
        {
            string itemName = "Graph";

            string resourceName = "GunRev/Resources/graph";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Graph>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "A Beautiful Line";
            string longDesc = "Improves accuracy.\n\nThese same tablets are used by Gundead scientists to predict their results. With a little modification, you can predict the accuracy of your bullets, too.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.C;

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, 0.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            
        }

        public override DebrisObject Drop(PlayerController player)
        {
            
            return base.Drop(player);

        }
    }
}