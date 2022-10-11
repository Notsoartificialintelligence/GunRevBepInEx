using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GunRev
{
    public class RefreshKey : PlayerItem
    {
        public static void Register()
        {
            string itemName = "Refresh Key";
            string resourceName = "GunRev/Resources/refreshkey";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<RefreshKey>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Tab Out";
            string longDesc = "If the player is on fire or under the effects of poison, triggers a blank effect.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");
            item.quality = PickupObject.ItemQuality.C;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 15f);
            item.consumable = false;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

        }
        public override bool CanBeUsed(PlayerController user)
        {
            return user.CurrentPoisonMeterValue > 0 | user.CurrentFireMeterValue > 0;
        }
        public override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_WPN_zapper_reload_01", base.gameObject);
            user.ForceBlank();
        }
    }
}
