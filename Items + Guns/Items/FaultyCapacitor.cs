using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class FaultyCapacitor : PassiveItem
    {
        public static void Register()
        {
            string itemName = "Faulty Capacitor";

            string resourceName = "GunRev/Resources/faultycapacitor";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<FaultyCapacitor>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Shocking Behaviour";
            string longDesc = "When hit, release a discharge of electricity.\n\nThis capacitor is sourced from a BLENDMATE K-50 Hyper Blender. Well, it was a blender before the capacitor was removed. Now it's just trash.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.C;
        }

        public void OnPlayerHurt(PlayerController player)
        {
            AkSoundEngine.PostEvent("Play_WPN_zapper_shot_01", player.specRigidbody.gameObject);
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(38) as Gun).DefaultModule.projectiles[0]);
            projectile.baseData.range = 16;
            projectile.baseData.damage = 12;
            LightningProjectileComp lightning = projectile.gameObject.GetOrAddComponent<LightningProjectileComp>();
            BounceProjModifier bounce = projectile.gameObject.GetOrAddComponent<BounceProjModifier>();
            bounce.numberOfBounces = 1;
            for (int i = 0; i < 6; i++)
            {
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, player.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, i * 60));
                Projectile thing = gameObject.GetComponent<Projectile>();
                thing.Owner = player;
                thing.Shooter = player.specRigidbody;
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            
            player.OnReceivedDamage += this.OnPlayerHurt;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            
            player.OnReceivedDamage -= this.OnPlayerHurt;
            return base.Drop(player);
        }
    }
}