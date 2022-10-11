using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class Guntry : GunBehaviour
    {
        int SHITFUCK = 1;
        public IEnumerator ReloadThingy()
        {
            yield return new WaitForSeconds(2);
            gun.TransformToTargetGun(Game.Items["ai:rocketgun"] as Gun);
            yield break;
        }
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Guntry", "rocketgun1");
            Game.Items.Rename("outdated_gun_mods:guntry", "ai:rocketgun");
            gun.gameObject.AddComponent<Guntry>();
            gun.SetShortDescription("Humanity's Mistake");
            gun.SetLongDescription("Shoots several stages that have different functions.\n\nSomeone had the hairbrained idea of combining the unreliability of early rocket flight with the power of a rocket launcher. Needless to say, this was not a good idea.");
            gun.SetupSprite(null, "rocketgun1_idle_001", 8);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            Gun other = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom(other, true, false);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 3;
            gun.DefaultModule.cooldownTime = 1.6f;
            gun.DefaultModule.numberOfShotsInClip = 4;
            gun.SetBaseMaxAmmo(32);
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "To the moon!";

            Projectile CommandPod = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(56) as Gun).DefaultModule.projectiles[0]);
            CommandPod.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(CommandPod.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(CommandPod);
            CommandPod.SetProjectileSpriteRight("rocketgun_projectile_001", 5, 3, false, tk2dBaseSprite.Anchor.MiddleCenter, 5, 3);
            CommandPod.shouldRotate = true;
            CommandPod.baseData.damage = 20f;
            CommandPod.baseData.speed = 16f;
            CommandPod.baseData.range = 64f;
            CommandPod.transform.parent = gun.barrelOffset;

            gun.DefaultModule.projectiles[0] = CommandPod;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "rocket";
            ETGMod.Databases.Items.Add(gun, null, "ANY");
        }
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_comm4nd0_shot_01", gameObject);
            gun.TransformToTargetGun(Game.Items["ai:shushyourenotsupposedtoseethis1"] as Gun);
            if (SHITFUCK == 1)
            {
                gun.TransformToTargetGun(Game.Items["ai:shushyourenotsupposedtoseethis1"] as Gun);
            }
            else if (SHITFUCK == 2)
            {
                gun.TransformToTargetGun(Game.Items["ai:shushyourenotsupposedtoseethis2"] as Gun);
            }
            else if (SHITFUCK == 3)
            {
                gun.TransformToTargetGun(Game.Items["ai:shushyourenotsupposedtoseethis3"] as Gun);
            }
            else if (SHITFUCK == 4)
            {
                gun.TransformToTargetGun(Game.Items["ai:shushyourenotsupposedtoseethis4"] as Gun);
                SHITFUCK = 0;
            }
            SHITFUCK += 1;
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
                AkSoundEngine.PostEvent("Play_WPN_comm4nd0_reload_01", base.gameObject);
                StartCoroutine(ReloadThingy());
                SHITFUCK = 1;
            }
        }
    }
}