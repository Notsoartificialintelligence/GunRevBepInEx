using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class FamiliarPhone : PlayerItem
    {
        public static void Register()
        {
            string itemName = "Familiar Phone";
            string resourceName = "GunRev/Resources/familiarphone";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<FamiliarPhone>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Indestructible";
            string longDesc = "Applies 2 armor on use.\n\nThis device is said to be legend on some nonexistent planet.\n\nHow the hell did it get here though?";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");
            item.quality = PickupObject.ItemQuality.B;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0f);
            item.consumable = true;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

        }
        public override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_NOKIA_RINGTONE__1994_", base.gameObject);
            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, user);
            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, user);
        }
    }
}