using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

    public class Gunbatus : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Gunbatus", "gunbatus");
            Game.Items.Rename("outdated_gun_mods:gunbatus", "ai:gunbatus");
            gun.gameObject.AddComponent<Gunbatus>();
            gun.SetShortDescription("Solar Sailer");
            gun.SetLongDescription("Shoots controllable drones.\n\nA strange machine from a distant galaxy. It is capable of creating smaller machines and deploying them near-infinitely.");
            gun.SetupSprite(null, "gunbatus_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            Gun other = PickupObjectDatabase.GetById(36) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 3f;
            gun.DefaultModule.cooldownTime = 0.6f;
            gun.DefaultModule.numberOfShotsInClip = 4;
            gun.SetBaseMaxAmmo(32);
            gun.quality = PickupObject.ItemQuality.A;
            gun.gunClass = GunClass.SILLY;
            gun.encounterTrackable.EncounterGuid = "nyoom funni spaceship";
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.shouldRotate = true;
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 5f;
            projectile.baseData.speed = 40f;
            projectile.baseData.range = 1000f;
            ExplosiveModifier explosiveModifier = projectile.gameObject.AddComponent<ExplosiveModifier>();
            explosiveModifier.doExplosion = true;
            explosiveModifier.explosionData = DungeonDatabase.GetOrLoadByName("base_castle").sharedSettingsPrefab.DefaultSmallExplosionData;
            RemoteBulletsProjectileBehaviour remotebulletbehaviour = projectile.gameObject.AddComponent<RemoteBulletsProjectileBehaviour>();
            remotebulletbehaviour.trackingTime = 1000f;
            remotebulletbehaviour.trackingSpeed = 100f;
            projectile.transform.parent = gun.barrelOffset;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            projectile.SetProjectileSpriteRight("gunbatusprojectile_001", 8, 6, false, tk2dBaseSprite.Anchor.MiddleCenter, 6, 4);
            gun.DefaultModule.customAmmoType = "exotic";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_yarirocketlauncher_shot_01", gameObject);
        }
        private bool HasReloaded;
        public override void Update()
        {
            if (gun.CurrentOwner)
            {

                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_yarirocketlauncher_reload_01", base.gameObject);
            }
        }
    }
}