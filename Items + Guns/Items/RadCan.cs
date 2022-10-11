using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SaveAPI;

namespace GunRev
{
    public class RadCan : PlayerItem
    {
        public static void Register()
        {
            string itemName = "Rad Can";
            string resourceName = "GunRev/Resources/radcan";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<RadCan>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Totally Gnarly";
            string longDesc = "Damages the player, but gives a speed, damage, and fire rate increase.\n\nRad Cans were briefly sold as energy drinks outside of the Gungeon.\n\nContains sextuple your recommended daily intake of ionising radiation.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");
            item.quality = PickupObject.ItemQuality.B;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 0f);
            item.consumable = true;
            item.SetupUnlockOnFlag(GungeonFlags.BOSSKILLED_BLOBULORD, true);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_WPN_Bubbler_Drink_01", base.gameObject);
            user.healthHaver.ApplyDamage(2f, Vector2.zero, null, CoreDamageTypes.None);
            StatModifier.Create(PlayerStats.StatType.RateOfFire, StatModifier.ModifyMethod.ADDITIVE, 1f);
            StatModifier.Create(PlayerStats.StatType.Damage, StatModifier.ModifyMethod.ADDITIVE, 1f);
            StatModifier.Create(PlayerStats.StatType.MovementSpeed, StatModifier.ModifyMethod.ADDITIVE, 1f);
        }
    }
}
