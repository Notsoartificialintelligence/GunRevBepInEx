using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class OilJar : PassiveItem
    {
        public static void Register()
        {
            string itemName = "Oil Jar";

            string resourceName = "GunRev/Resources/oiljar";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<OilJar>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Greasy";
            string longDesc = "Increases fire rate and reload speed.\n\nThis oil can be used to grease the barrels of guns, or you could burn it or something, I don't know.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.C;

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, -0.4f, StatModifier.ModifyMethod.ADDITIVE);
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