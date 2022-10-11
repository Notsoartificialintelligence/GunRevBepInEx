using Alexandria.ItemAPI;
using UnityEngine;

namespace GunRev
{
    public class Robullets : PassiveItem
    {
        public static void Register()
        {
            string itemName = "Robullets";

            string resourceName = "GunRev/Resources/robullets";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Robullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Shooting Is The Future";
            string longDesc = "Bullets have a chance to spawn a sentry on enemy kill.\n\nWhile Gundead are organic beings, their outer shells have a similar chemical makeup to that of many common metals and alloys used in military devices. Actually, that makes sense when you read it out.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ai");

            item.quality = PickupObject.ItemQuality.A;
        }
        public void OnHitEnemy(Projectile projectile, SpeculativeRigidbody hitRigidbody, bool fatal)
        {
            if (hitRigidbody.aiActor != null)
            {
                if (fatal == true)
                {
                    int shart = UnityEngine.Random.Range(0, 20);
                    if (Owner.PlayerHasActiveSynergy("Sentry Mode Activated") && shart < 4)
                    {
                        SpawnTurret(hitRigidbody.aiActor);
                    }
                    else if (shart == 1)
                    {
                        SpawnTurret(hitRigidbody.aiActor);
                    }
                }
            }
        }        
        public void PostProcessProjectile(Projectile projectile, float f)
        {
            projectile.OnHitEnemy += OnHitEnemy;
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

        private void SpawnTurret(AIActor hitenemy)
        {
            var Turret = EnemyDatabase.GetOrLoadByGuid("998807b57e454f00a63d67883fcf90d6");
            IntVector2? spawnPos = hitenemy.specRigidbody.UnitCenter.ToIntVector2();
            if (spawnPos.HasValue)
            {
                AIActor TargetActor = AIActor.Spawn(Turret.aiActor, spawnPos.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(spawnPos.Value), true, AIActor.AwakenAnimationType.Default, true);
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(TargetActor.specRigidbody, null, false);
                TargetActor.IsHarmlessEnemy = true;
                TargetActor.IgnoreForRoomClear = true;
                AkSoundEngine.PostEvent("Play_OBJ_turret_set_01", gameObject);
            }
        }
    }
}