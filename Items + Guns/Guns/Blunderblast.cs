using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;
using System.Collections.Generic;
using System.Linq;


namespace GunRev
{
    public class Blunderblast : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Blunderblast", "blunderblast");
            Game.Items.Rename("outdated_gun_mods:blunderblast", "ai:blunderblast");
            gun.gameObject.AddComponent<Blunderblast>();
            gun.SetShortDescription("The Cooler One");
            gun.SetLongDescription("An ultra-modern rendition of the famous Blunderbuss. Fires laser projectiles.\n\nComes with WiFi integration, music streaming, a hefty price tag, and multiple workplace safety violations!");
            gun.SetupSprite(null, "blunderblast_idle_001", 8);
            tk2dSpriteAnimationClip chargeClip = gun.sprite.spriteAnimator.GetClipByName("blunderblast_charge");
            chargeClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            chargeClip.loopStart = 2;
            gun.SetAnimationFPS(gun.shootAnimation, 16);

            for (int i = 0; i < 5; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(157) as Gun, true, false);
                gun.gunSwitchGroup = (PickupObjectDatabase.GetById(157) as Gun).gunSwitchGroup;
            }

            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 4f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 5;
            gun.SetBaseMaxAmmo(60);
            gun.quality = PickupObject.ItemQuality.B;
            gun.gunClass = GunClass.CHARGE;
            gun.encounterTrackable.EncounterGuid = "the laser thingymajig";

            foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
            {
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
                projectile.gameObject.SetActive(false);
                projectile.SetProjectileSpriteRight("blunderblast_projectile_001", 11, 8, false, tk2dBaseSprite.Anchor.MiddleCenter, 9, 6);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile);
                gun.DefaultModule.projectiles[0] = projectile;
                projectile.baseData.damage = 3f;
                projectile.baseData.speed = 15f;
                projectile.transform.parent = gun.barrelOffset;
                projectile.shouldRotate = true;

                Projectile projectile2 = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
                projectile2.gameObject.SetActive(false);
                projectile2.SetProjectileSpriteRight("blunderblast_projectile_001", 11, 8, false, tk2dBaseSprite.Anchor.MiddleCenter, 9, 6);
                FakePrefab.MarkAsFakePrefab(projectile2.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile2);
                projectile2.baseData.damage = 8f;
                projectile2.baseData.speed = 20f;
                projectile2.transform.parent = gun.barrelOffset;
                projectile2.shouldRotate = true;

                Projectile projectile3 = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
                projectile3.gameObject.SetActive(false);
                projectile3.SetProjectileSpriteRight("blunderblast_projectile_001", 11, 8, false, tk2dBaseSprite.Anchor.MiddleCenter, 9, 6);
                FakePrefab.MarkAsFakePrefab(projectile3.gameObject);
                UnityEngine.Object.DontDestroyOnLoad(projectile3);
                projectile3.baseData.damage = 14f;
                projectile3.baseData.speed = 30f;
                projectile3.AdditionalScaleMultiplier = 1.5f;
                projectile3.transform.parent = gun.barrelOffset;
                projectile3.shouldRotate = true;

                gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile> {
                new ProjectileModule.ChargeProjectile
                {
                    Projectile = projectile,
                    ChargeTime = 0f
                },
                new ProjectileModule.ChargeProjectile
                {
                    Projectile = projectile2,
                    ChargeTime = 1f
                },
                new ProjectileModule.ChargeProjectile
                {
                    Projectile = projectile3,
                    ChargeTime = 3f
                }
                };
            }
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.SHOTGUN;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            PlayerController player = (PlayerController)gun.CurrentOwner;
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
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
                AkSoundEngine.PostEvent("Play_WPN_shotgun_reload", base.gameObject);
            }
        }
    }
}