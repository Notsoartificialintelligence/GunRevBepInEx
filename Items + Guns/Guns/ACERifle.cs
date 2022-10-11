using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

    public class ACERifle : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("ACE Rifle", "ace");
            Game.Items.Rename("outdated_gun_mods:ace_rifle", "ai:ace_rifle");
            gun.gameObject.AddComponent<ACERifle>();
            gun.SetShortDescription("High Ranking");
            gun.SetLongDescription("On kill, cloaks the player. Recieves a damage boost from firing while cloaked. Damage boosts can stack.\n\nWonder what those colours symbolise?");
            gun.SetupSprite(null, "ace_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            Gun other = PickupObjectDatabase.GetById(49) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 2f;
            gun.DefaultModule.cooldownTime = 1f;
            gun.DefaultModule.numberOfShotsInClip = 4;
            gun.SetBaseMaxAmmo(80);
            gun.quality = PickupObject.ItemQuality.A;
            gun.gunClass = GunClass.RIFLE;
            gun.encounterTrackable.EncounterGuid = "asexual gun epic";
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 20f;
            projectile.baseData.speed = 40f;
            projectile.baseData.range = 999999999999f;
            projectile.transform.parent = gun.barrelOffset;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "Rifle";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_sniperrifle_shot_01", gameObject);
        }
        public static void OnHitEnemy(Projectile projectile, SpeculativeRigidbody hitRigidbody, bool fatal)
        {
            PlayerController user = (PlayerController)projectile.Owner;
            if (hitRigidbody.aiActor != null)
            {
                if (fatal == true)
                {
                    user.SetIsStealthed(true, "ACERifle");
                }
            }
        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            PlayerController user = (PlayerController)projectile.Owner;
            float damagemultiplier = 1.5f;
            projectile.OnHitEnemy += OnHitEnemy;
            if (user.IsStealthed)
            {
                projectile.baseData.damage *= damagemultiplier;
                user.SetIsStealthed(false, "ACERifle");
            }
            else
            {
                projectile.baseData.damage = 20f;
            }
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
                player.SetIsStealthed(false, "ACERifle");
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("SND_WPN_m1rifle_reload_01", base.gameObject);
            }
        }
    }
}
