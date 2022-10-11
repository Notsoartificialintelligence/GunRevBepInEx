using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class Drillets : PassiveItem
    {
        public static void Register()
        {
            string itemName = "Drillets";

            string resourceName = "GunRev/Resources/drillet";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Drillets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Dig Deep";
            string longDesc = "Bullets pierce twice and always uncover secret walls.\n\nBullets said to have once burrowed to the centre of Gunymede. Not a wise economic investment.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.C;
        }
        public void PostProcessProjectile(Projectile projectile, float f)
        {
            PierceProjModifier pierce = projectile.gameObject.GetOrAddComponent<PierceProjModifier>();
            pierce.penetration += 2;
            projectile.damagesWalls = true;
            pierce.penetratesBreakables = true;
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