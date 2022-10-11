using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class DisasterRecipe : PassiveItem
    {
        public static void Register()
        {
            string itemName = "Recipe For Disaster";

            string resourceName = "GunRev/Resources/disasterrecipe";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<DisasterRecipe>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            item.CanBeDropped = false;

            string shortDesc = "Hell's Kitchen";
            string longDesc = "All shots explode, but you have 2 less heart containers. This item cannot be dropped.\n\nAn infernally infamous recipe taken from the depths of Bullet Hell. Requires the boiled horn of a demon, a spell from a Gunjurer, and salt, to taste. Feeds 4 to 6 people.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.B;
        }
        public static void PostProcessProjectile(Projectile projectile, float f)
        {
            ExplosiveModifier explosiveModifier = projectile.gameObject.AddComponent<ExplosiveModifier>();
            explosiveModifier.doExplosion = true;
            explosiveModifier.IgnoreQueues = true;
            explosiveModifier.explosionData = DungeonDatabase.GetOrLoadByName("base_castle").sharedSettingsPrefab.DefaultSmallExplosionData;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            
            player.PostProcessProjectile += PostProcessProjectile;
            player.healthHaver.SetHealthMaximum(player.healthHaver.GetMaxHealth() - 2);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            
            player.PostProcessProjectile -= PostProcessProjectile;
            return base.Drop(player);
        }
    }
}