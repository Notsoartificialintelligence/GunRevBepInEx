using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using Alexandria.ItemAPI;

namespace GunRev
{
    public class WhileGunShoot : AdvancedGunBehavior
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("While Gun Shoot", "whilegunshoot");
            Game.Items.Rename("outdated_gun_mods:while_gun_shoot", "ai:while_gun_shoot");
            var behav = gun.gameObject.AddComponent<WhileGunShoot>();
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("Machine Learning");
            gun.SetLongDescription("Shoots a node that latches onto enemies.\n\nAn ancient device, it appears to have no three-dimensional qualities.");

            gun.SetupSprite(null, "whilegunshoot_idle_001", 8);

            gun.SetAnimationFPS(gun.shootAnimation, 8);
            gun.isAudioLoop = true;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.doesScreenShake = false;
            gun.DefaultModule.ammoCost = 10;
            gun.DefaultModule.angleVariance = 0;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.cooldownTime = 0.001f;
            gun.DefaultModule.numberOfShotsInClip = 3000;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "green_beam";
            gun.barrelOffset.transform.localPosition = new Vector3(0.875f, 0.4375f, 0f);
            gun.SetBaseMaxAmmo(1500);
            gun.ammo = 1500;

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 1;

            List<string> BeamAnimPaths = new List<string>()
            {
                "GunRev/Resources/whilegunshoot_middle_001",
            };
            List<string> BeamEndPaths = new List<string>()
            {
                "GunRev/Resources/whilegunshoot_impact_001",
            };

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);

            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "GunRev/Resources/whilegunshoot_middle_001",
                new Vector2(8, 2),
                new Vector2(0, 0),
                BeamAnimPaths,
                9,
                //Impact
                null,
                -1,
                null,
                null,
                //End
                BeamEndPaths,
                9,
                new Vector2(6, 6),
                new Vector2(0, 0),
                //Beginning
                null,
                -1,
                null,
                null
                );

            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage = 10f;
            projectile.baseData.force *= 0.1f;
            projectile.baseData.speed *= 1f;
            beamComp.homingAngularVelocity = 360f;
            beamComp.homingRadius = 360f;
            beamComp.penetration = 0;
            beamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
            beamComp.interpolateStretchedBones = false;
            beamComp.endAudioEvent = "Stop_WPN_All";
            beamComp.startAudioEvent = "Play_WPN_moonscraperLaser_shot_01";

            gun.DefaultModule.projectiles[0] = projectile;

            gun.quality = PickupObject.ItemQuality.B;
            gun.gunClass = GunClass.BEAM;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
    }
}