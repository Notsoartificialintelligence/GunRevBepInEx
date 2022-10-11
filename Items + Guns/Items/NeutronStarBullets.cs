using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alexandria.ItemAPI;
using SaveAPI;

namespace GunRev
{
    public class NeutronStarBullets : PassiveItem
    {
        public static void Register()
        {
            string itemName = "Neutron Star Bullets";

            string resourceName = "GunRev/Resources/neutron_star_bullets";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<NeutronStarBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "What If...";
            string longDesc = "Bullets with such a high density, they have their own gravity. The gravity they create can sometimes cause miniature supernovae on collision.\n\nThe most expensive project the R&G Department has authorised to date. It costs around 5 decillion casings to create, as well as the costs incurred when half of the facility caved in and burned to a crisp.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.S;
        }
        public void PostProcessProjectile(Projectile projectile, float f)
        {
            BlackHoleDoer blackHole = projectile.gameObject.GetOrAddComponent<BlackHoleDoer>();
            blackHole.affectsPlayer = false;
            blackHole.affectsEnemies = true;
            blackHole.affectsDebris = true;
            blackHole.affectsBullets = false;
            blackHole.destroysBullets = false;
            blackHole.destroysDebris = true;
            blackHole.SpawningPlayer = Owner;
            blackHole.introStyle = BlackHoleDoer.BlackHoleIntroStyle.Instant;
            blackHole.outroStyle = BlackHoleDoer.BlackHoleOutroStyle.Nova;
            blackHole.distortStrength = -0.2f;
            blackHole.damageRadius = 0;
            blackHole.damageToEnemiesPerSecond = 0;
            blackHole.damageToPlayerPerSecond = 0;
            blackHole.gravitationalForce = 10f;
            blackHole.gravitationalForceActors = 50f;
            blackHole.radius = 10f;
            blackHole.coreDuration = 5f;
            int aaaaa = UnityEngine.Random.Range(0, 15);
            if (aaaaa < 3)
            {
                projectile.OnHitEnemy += OnEnemyHit;
            }
        }

        public void OnEnemyHit(Projectile projectile, SpeculativeRigidbody hitRigidbody, bool fatal)
        {
            if (hitRigidbody.aiActor.healthHaver.IsAlive)
            {
                Exploder.Explode(projectile.specRigidbody.UnitCenter, GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultExplosionData, projectile.specRigidbody.UnitCenter);
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