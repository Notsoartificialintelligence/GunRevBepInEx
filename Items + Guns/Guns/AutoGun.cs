using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

    public class AutoGun : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("AWC-19", "awc-19");
            Game.Items.Rename("outdated_gun_mods:awc-19", "ai:awc-19");
            gun.gameObject.AddComponent<AutoGun>();
            gun.SetShortDescription("Self-Aware");
            gun.SetLongDescription("An advanced experimental rifle created by the Automated Weaponry Company. It was potentially the most dangerous gun built by the company, and thus was pulled from shelves within mere hours of the technical oversight surfacing.\n\nThey are now seen as collector's items, and are kept in reinforced glass cases for good measure.");
            gun.SetupSprite(null, "autogun_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 13);
            Gun other = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.3f;
            gun.DefaultModule.cooldownTime = 0.6f;
            gun.DefaultModule.numberOfShotsInClip = 4;
            gun.SetBaseMaxAmmo(60);
            gun.quality = PickupObject.ItemQuality.A;
            gun.gunClass = GunClass.RIFLE;
            gun.encounterTrackable.EncounterGuid = "mfw the gun fires itself :skull:";
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.SetProjectileSpriteRight("trigunometry_projectile_001", 10, 10, false, tk2dBaseSprite.Anchor.MiddleCenter, 8, 8);
            projectile.shouldRotate = true;
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.AppliesPoison = true;
            projectile.PoisonApplyChance = 0.8f;
            projectile.healthEffect = PickupObjectDatabase.GetById(204).GetComponent<BulletStatusEffectItem>().HealthModifierEffect;
            projectile.baseData.damage = 8f;
            projectile.baseData.speed = 18f;
            projectile.baseData.range = 30f;
            projectile.transform.parent = gun.barrelOffset;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MEDIUM_BLASTER;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", gameObject);
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
                AkSoundEngine.PostEvent("Play_WPN_m1911_reload_01", base.gameObject);
            }
        }
    }
}