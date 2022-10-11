using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SaveAPI;

namespace GunRev
{
    public class ConstructionController : PlayerItem
    {
        public static void Register()
        {
            string itemName = "Construction Controller";
            string resourceName = "GunRev/Resources/constructioncontroller";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ConstructionController>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Computer Aided";
            string longDesc = "Enables the construction of many different forms of turrets.\n\n";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");
            item.quality = PickupObject.ItemQuality.A;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 600f);
            item.consumable = false;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override void DoEffect(PlayerController user)
        {

        }
    }
}
