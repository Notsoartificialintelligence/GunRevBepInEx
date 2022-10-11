using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace GunRev
{
    public class NonFungiBullets : PassiveItem
    {
        public PickupObject item;
        public string shortDesc;
        public string longDesc;
        public GameObject obj;
        public string resourceName;
        public static void Register()
        {
            string itemName = "Non-Fungibullet #" + UnityEngine.Random.Range(0,99999).ToString();

            string resourceName = "GunRev/Resources/nonfungibullets/non-fungibullets";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<NonFungiBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "PLACEHOLDER TEXT";
            string longDesc = "Every bullet has a unique colour and added damage value.\n\nWhile these images may be innocent and somewhat comical, the energy required to encrypt them has devastated many ecosystems and planets.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            string[] spritePaths = new string[]
{
                "GunRev/Resources/nonfungibullets/non-fungibullets.png",
                "GunRev/Resources/nonfungibullets/non-fungibullets_002.png",
                "GunRev/Resources/nonfungibullets/non-fungibullets_003.png",
                "GunRev/Resources/nonfungibullets/non-fungibullets_004.png",
                "GunRev/Resources/nonfungibullets/non-fungibullets_005.png",
                "GunRev/Resources/nonfungibullets/non-fungibullets_006.png",
                "GunRev/Resources/nonfungibullets/non-fungibullets_007.png",
                "GunRev/Resources/nonfungibullets/non-fungibullets_008.png",
                "GunRev/Resources/nonfungibullets/non-fungibullets_009.png",
                "GunRev/Resources/nonfungibullets/non-fungibullets_010.png",
                "GunRev/Resources/nonfungibullets/non-fungibullets_011.png",
                "GunRev/Resources/nonfungibullets/non-fungibullets_012.png"
};

            var randomSprite = obj.AddComponent<RandomiseSpriteAndNameOfItem>();
            var collection = obj.GetComponent<tk2dSprite>().Collection;

            foreach (var path in spritePaths)
            {
                randomSprite.spriteIds.Add(SpriteBuilder.AddSpriteToCollection(path, collection));
            }

            item.quality = PickupObject.ItemQuality.B;

            item.SetupUnlockOnStat(TrackedStats.TOTAL_MONEY_COLLECTED, 100000, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
        }
        public void PostProcessProjectile(Projectile projectile, float f)
        {
            projectile.baseData.damage += UnityEngine.Random.Range(0, 5);
            projectile.AdjustPlayerProjectileTint(new Color32(Convert.ToByte(UnityEngine.Random.Range(0, 255)), Convert.ToByte(UnityEngine.Random.Range(0, 255)), Convert.ToByte(UnityEngine.Random.Range(0, 255)), Convert.ToByte(255)), 3);
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