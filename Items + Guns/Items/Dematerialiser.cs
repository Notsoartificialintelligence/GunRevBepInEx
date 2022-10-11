using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using System.Collections;

namespace GunRev
{
    public class Dematerialiser : PlayerItem
    {
        public IEnumerator Effect(PlayerController user)
        {
            if (base.LastOwner != null)
            {
                float StatBooster = 5f;
                StatModifier item = new StatModifier
                {
                    statToBoost = PlayerStats.StatType.MovementSpeed,
                    amount = StatBooster,
                    modifyType = StatModifier.ModifyMethod.ADDITIVE
                };
                StatExtra.Add(item);

                base.LastOwner.ownerlessStatModifiers.Add(item);
                base.LastOwner.stats.RecalculateStats(user, true, true);
            }
            Shader shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
            user.sprite.renderer.material.shader = shader;
            for (int i = 0; i < 10; i++)
            {
                user.IsEthereal = true;
                yield return new WaitForSeconds(1);
            };
            user.IsEthereal = false;
            Shader balls = ShaderCache.Acquire("Brave/PlayerShader");
            user.sprite.renderer.material.shader = balls;
            foreach (StatModifier stat in StatExtra)
            {
                user.ownerlessStatModifiers.Remove(stat);
                user.stats.RecalculateStats(user, true, true);
            }

        }
        public static void Register()
        {
            string itemName = "Dematerialiser";
            string resourceName = "GunRev/Resources/dematerialiser";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<Dematerialiser>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Where'd All The Bullets Go?";
            string longDesc = "Temporarily depletes the holder of their physical matter, providing temporary ethereality and a speed boost.\n\n\"A surprisingly simple scientific breakthrough, we simply just stole this thing from an underground laboratory. We have no idea how it works.\"";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");
            item.quality = PickupObject.ItemQuality.A;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1500f);
            item.consumable = false;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

        }
        public override void DoEffect(PlayerController user)
        {
            StartCoroutine(Effect(user));
        }
        private List<StatModifier> StatExtra = new List<StatModifier>();
    }
}