using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class InertiaBullets : PassiveItem
    {
        public static void Register()
        {
            string itemName = "Inertia Bullets";

            string resourceName = "GunRev/Resources/inertia_bullets";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<InertiaBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Can't Stop, Won't Stop";
            string longDesc = "The faster bullets are, the more damage they deal. Bullets penetrate enemies if they travel fast enough.\n\nPossibly the best investment the R&G Department ever made. That's not saying much.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.B;
        }
        public void PostProcessProjectile(Projectile projectile, float f)
        {
            if (projectile.baseData.speed > 16)
            {
                projectile.baseData.damage += 4;
            }
            else
            {
                projectile.baseData.damage += projectile.baseData.speed % 4;
            }
            PierceProjModifier pierce = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            pierce.penetration += (int)projectile.baseData.speed % 12;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            
            player.PostProcessProjectile += this.PostProcessProjectile;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return base.Drop(player);

        }
    }
}