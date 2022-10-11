using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class ScienceBook : PassiveItem
    {
        public static void Register()
        {
            string itemName = "Scientist's Guide To The Gungeon";

            string resourceName = "GunRev/Resources/sciencetome";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<ScienceBook>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Volume 1e308";
            string longDesc = "Gain more damage based on your accuracy and fire rate.\n\nMultiple of the pages are badly corroded or scorched. There are initials printed on the spine of the book, \"P.B.\".";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.B;

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, (firerate / acc) * 0.25f, StatModifier.ModifyMethod.ADDITIVE);
        }
        private static float acc;
        private static float firerate;
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            
            acc = Owner.stats.GetStatValue(PlayerStats.StatType.Accuracy);
            firerate = Owner.stats.GetStatValue(PlayerStats.StatType.RateOfFire);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            
            return base.Drop(player);

        }
    }
}