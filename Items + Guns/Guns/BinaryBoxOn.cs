using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

    public class BinaryBoxOn : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Binary Box On", "binaryon");
            Game.Items.Rename("outdated_gun_mods:binary_box_on", "ai:binary_box_on");
            gun.gameObject.AddComponent<BinaryBoxOn>();
            gun.SetShortDescription("11111111");
            gun.SetLongDescription("Shots have a tendency to move upwards.\n\n01010011 01101000 01101111 01110100 01110011 00100000 01101000 01100001 01110110 01100101 00100000 01100001 00100000 01110100 01100101 01101110 01100100 01100101 01101110 01100011 01111001 00100000 01110100 01101111 00100000 01101101 01101111 01110110 01100101 00100000 01110101 01110000 01110111 01100001 01110010 01100100 01110011 00101110");
            gun.SetupSprite(null, "binaryon_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 13);
            Gun other = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.1f;
            gun.DefaultModule.cooldownTime = 0.3f;
            gun.DefaultModule.numberOfShotsInClip = 11;
            gun.SetBaseMaxAmmo(111);
            gun.quality = PickupObject.ItemQuality.A;
            gun.gunClass = GunClass.PISTOL;
            gun.encounterTrackable.EncounterGuid = "up";
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.SetProjectileSpriteRight("binarybox_projectile_on", 6, 6, false, tk2dBaseSprite.Anchor.MiddleCenter, 4, 4);
            projectile.shouldRotate = true;
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 4f;
            projectile.baseData.speed = 11f;
            projectile.baseData.range = 111f;
            projectile.transform.parent = gun.barrelOffset;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = CustomClipAmmoTypeToolbox.AddCustomAmmoType("Transistor", "GunRev/Resources/Clips/binaryclip", "GunRev/Resources/Clips/binaryclipempty");
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_zapper_fire_01", gameObject);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
        public void PostProcessProjectile(Projectile projectile, float f)
        {
            projectile.SendInDirection(projectile.Direction.Rotate(1), false);
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
                AkSoundEngine.PostEvent("Play_WPN_zapper_reload_01", base.gameObject);
            }
        }
    }
}