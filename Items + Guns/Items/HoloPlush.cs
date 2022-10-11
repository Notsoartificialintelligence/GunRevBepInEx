using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class HoloPlush : PassiveItem
    {
        public static void Register()
        {
            string itemName = "Holo-Kin Plush";

            string resourceName = "GunRev/Resources/holoplush";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<HoloPlush>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Illusion Of Comfort";
            string longDesc = "What a lovely plushie.\n\nSuch a shame it cannot be interacted with in any way...\n\nDoes absolutely nothing.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.D;
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