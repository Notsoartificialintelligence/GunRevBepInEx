using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{

    public class Keyboard : GunBehaviour
    {

        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Keyboard", "keyboard");
            Game.Items.Rename("outdated_gun_mods:keyboard", "ai:keyboard");
            gun.gameObject.AddComponent<Keyboard>();
            gun.SetShortDescription("Prone To Smashing");
            gun.SetLongDescription("Shoots the 4 letters of the gamer alphabet.\n\nA testament to the time spent making this mod, and to those who helped along the way.\n\nThank you.");
            gun.SetupSprite(null, "keyboard_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            Gun other = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Ordered;
            gun.reloadTime = 1.6f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = 8;
            gun.SetBaseMaxAmmo(256);
            gun.quality = PickupObject.ItemQuality.A;
            gun.gunClass = GunClass.SILLY;
            gun.encounterTrackable.EncounterGuid = "WASD";

            Projectile W = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            W.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(W.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(W);
            W.SetProjectileSpriteRight("keyboard_projectile_001", 8, 8, false, tk2dBaseSprite.Anchor.MiddleCenter, 8, 8);
            W.shouldRotate = true;
            W.baseData.damage = 16f;
            W.baseData.speed = 16f;
            W.baseData.range = 16f;
            W.transform.parent = gun.barrelOffset;

            Projectile A = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            A.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(A.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(A);
            A.SetProjectileSpriteRight("keyboard_projectile_002", 8, 8, false, tk2dBaseSprite.Anchor.MiddleCenter, 8, 8);
            A.shouldRotate = true;
            A.baseData.damage = 16f;
            A.baseData.speed = 16f;
            A.baseData.range = 16f;
            A.transform.parent = gun.barrelOffset;

            Projectile S = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            S.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(S.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(S);
            S.SetProjectileSpriteRight("keyboard_projectile_003", 8, 8, false, tk2dBaseSprite.Anchor.MiddleCenter, 8, 8);
            S.shouldRotate = true;
            S.baseData.damage = 16f;
            S.baseData.speed = 16f;
            S.baseData.range = 16f;
            S.transform.parent = gun.barrelOffset;

            Projectile D = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            D.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(D.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(D);
            D.SetProjectileSpriteRight("keyboard_projectile_004", 8, 8, false, tk2dBaseSprite.Anchor.MiddleCenter, 8, 8);
            D.shouldRotate = true;
            D.baseData.damage = 16f;
            D.baseData.speed = 16f;
            D.baseData.range = 16f;
            D.transform.parent = gun.barrelOffset;

            gun.DefaultModule.projectiles[0] = W;
            gun.DefaultModule.projectiles.Add(A);
            gun.DefaultModule.projectiles.Add(S);
            gun.DefaultModule.projectiles.Add(D);
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "green_small";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_zapper_shot_01", gameObject);
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