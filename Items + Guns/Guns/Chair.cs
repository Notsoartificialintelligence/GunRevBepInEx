using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class Chair : GunBehaviour
    {
        public float ExtraDamage = 0;
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Chair", "chair");
            Game.Items.Rename("outdated_gun_mods:chair", "ai:chair");
            gun.gameObject.AddComponent<Chair>();
            gun.SetShortDescription("Take A Seat");
            gun.SetLongDescription("Recieves a damage boost when a table is flipped. Damage boosts stack.\n\nAn essential part of any seating arrangement, except for couches.");
            gun.SetupSprite(null, "chair_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            Gun other = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.2f;
            gun.DefaultModule.cooldownTime = 0.3f;
            gun.DefaultModule.numberOfShotsInClip = 7;
            gun.SetBaseMaxAmmo(350);
            gun.quality = PickupObject.ItemQuality.A;
            gun.gunClass = GunClass.SILLY;
            gun.encounterTrackable.EncounterGuid = "table flip funny";
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.SetProjectileSpriteRight("chair_projectile", 10, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 6, 5);
            projectile.shouldRotate = true;
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 4f;
            projectile.baseData.speed = 15f;
            projectile.baseData.range = 70f;
            projectile.transform.parent = gun.barrelOffset;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Chair Bullets", "GunRev/Resources/Clips/chair_uiclip", "GunRev/Resources/Clips/chair_uiclip_empty");
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

        public override void PostProcessProjectile(Projectile projectile)
        {
            projectile.baseData.damage += ExtraDamage;
            base.PostProcessProjectile(projectile);
        }

        private void HandleFlip(FlippableCover table)
        {
            ExtraDamage += 1;
        }

        public override void OnPlayerPickup(PlayerController playerOwner)
        {
            base.OnPlayerPickup(playerOwner);
            playerOwner.OnTableFlipped += HandleFlip;
        }

        public override void OnDroppedByPlayer(PlayerController player)
        {
            player.OnTableFlipped -= HandleFlip;
            base.OnDroppedByPlayer(player);
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_magnum_reload_01", base.gameObject);
            }
        }
    }
}