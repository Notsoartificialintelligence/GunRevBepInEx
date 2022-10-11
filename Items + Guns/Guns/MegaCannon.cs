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

    public class MegaCannon : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Mega T Shirt Cannon", "megatshirtcannon");
            Game.Items.Rename("outdated_gun_mods:mega_t_shirt_cannon", "ai:mega_t_shirt_cannon");
            gun.gameObject.AddComponent<MegaCannon>();
            gun.SetShortDescription("Spin Cycle");
            gun.SetLongDescription("The pinnacle of cannon technology.\n\nWho needs miniaturised particle accelerators or hydrogen gas guns when you have a washing machine strapped to a gun?");
            gun.SetupSprite(null, "megatshirtcannon_idle_001", 8);
            tk2dSpriteAnimationClip chargeClip = gun.sprite.spriteAnimator.GetClipByName("megatshirtcannon_charge");
            chargeClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            chargeClip.loopStart = 1;
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            Gun other = PickupObjectDatabase.GetById(150) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 4f;
            gun.DefaultModule.cooldownTime = 2f;
            gun.DefaultModule.numberOfShotsInClip = 2;
            gun.SetBaseMaxAmmo(20);
            gun.quality = PickupObject.ItemQuality.S;
            gun.gunClass = GunClass.SILLY;
            gun.encounterTrackable.EncounterGuid = "Signature look of superiority";
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 300f;
            projectile.baseData.speed = 20f;
            projectile.AdditionalScaleMultiplier = 2f;
            projectile.transform.parent = gun.barrelOffset;
            gun.AddCurrentGunStatModifier(PlayerStats.StatType.MovementSpeed, 0.7f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ProjectileModule.ChargeProjectile chargeProj = new ProjectileModule.ChargeProjectile
            {
                Projectile = projectile,
                ChargeTime = 7f,
            };
            gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile> { chargeProj };
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "t_shirt";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            PlayerController player = (PlayerController)gun.CurrentOwner;
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_serious_cannon_shot_01", gameObject);
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
                AkSoundEngine.PostEvent("Play_WPN_serious_cannon_reload_01", base.gameObject);
            }
        }
    }
}