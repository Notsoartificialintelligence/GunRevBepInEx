using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class BarrelBullets : PassiveItem
    {
        public static void Register()
        { 
            string itemName = "Barrel Bullets";

            string resourceName = "GunRev/Resources/barrelbullets";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<BarrelBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Poorly Sealed";
            string longDesc = "Gives bullets a chance to trail oil or explode on impact.\n\nWhoever made these \"Explosive Bullets\" did a REALLY bad job of sealing them.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.A;


            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            string text = "assets/data/goops/oil goop.asset";
            GoopDefinition goopDefinition;
            try
            {
                GameObject gameObject = assetBundle.LoadAsset(text) as GameObject;
                goopDefinition = gameObject.GetComponent<GoopDefinition>();
            }
            catch
            {
                goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
            }
            goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
            BarrelBullets.DefaultOilGoop = goopDefinition;
        }
        public static GoopDefinition DefaultOilGoop;

        private void AddGoop(Projectile proj)
        {
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(BarrelBullets.DefaultOilGoop).TimedAddGoopCircle(proj.specRigidbody.UnitCenter, 1f, 0.5f, false);
        }
        public void PostProcessProjectile(Projectile projectile, float f)
        {
            int FUCK = UnityEngine.Random.Range(0,100);
            if (FUCK <= 3)
            {
                ExplosiveModifier explosiveModifier = projectile.gameObject.AddComponent<ExplosiveModifier>();
                explosiveModifier.doExplosion = true;
                explosiveModifier.explosionData = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
            }
            else if (FUCK > 3 || FUCK < 50)
            {
                projectile.OnPostUpdate += this.AddGoop;
            }
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