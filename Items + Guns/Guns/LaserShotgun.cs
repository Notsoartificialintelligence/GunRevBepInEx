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
    public class LaserShotgun : AdvancedGunBehavior
    {
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Laser Shotgun", "lasershotgun");
            Game.Items.Rename("outdated_gun_mods:laser_shotgun", "ai:laser_shotgun");
            var behav = gun.gameObject.AddComponent<LaserShotgun>();
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("Combat Enters The Future");
            gun.SetLongDescription("Not one, not two, but THREE powerful lasers combined into a convenient package based on the trusty shotgun.\n\nFun with cats, too. Just don't let them get close.");
            gun.SetupSprite(null, "lasershotgun_idle_001", 8);
            gun.barrelOffset.transform.localPosition = new Vector3(0.875f, 0.4375f, 0f);
            gun.SetBaseMaxAmmo(1000);
            gun.ammo = 1000;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.SetAnimationFPS(gun.shootAnimation, 9);
            gun.isAudioLoop = true;
            gun.barrelOffset.transform.localPosition = new Vector3(1.4375f, 0.25f, 0f);
            gun.carryPixelOffset = new IntVector2(2, -1);
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 2;
            int iterator = 0;
            for (int i = 0; i < 3; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            }
            List<string> BeamAnimPaths = new List<string>()
            {
                "GunRev/Resources/lasershotgun_middle_001",
            };
            List<string> BeamEndPaths = new List<string>()
            {
                "GunRev/Resources/lasershotgun_impact_001",
            };
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "GunRev/Resources/lasershotgun_middle_001",
                new Vector2(8, 1),
                new Vector2(0, 1),
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
                new Vector2(7, 1),
                new Vector2(0, 1),
                //Beginning
                null,
                -1,
                null,
                null
                );
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.doesScreenShake = false;
            foreach (ProjectileModule projmodule in gun.Volley.projectiles)
            {
                if (iterator == 1)
                {
                    projmodule.angleFromAim = 20;
                }
                if (iterator == 2)
                {
                    projmodule.angleFromAim = -20;
                }
                bool flag = projmodule != gun.DefaultModule;
                if (flag)
                {
                    projmodule.ammoCost = 0;
                }
                beamComp.interpolateStretchedBones = false;
                projmodule.numberOfShotsInClip = 999999;
                projmodule.ammoCost = 5;
                projmodule.angleVariance = 0;
                projmodule.shootStyle = ProjectileModule.ShootStyle.Beam;
                projmodule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
                projmodule.cooldownTime = 0.001f;
                projmodule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
                projmodule.customAmmoType = "red_beam";
                projectile.baseData.damage = 10f;
                projectile.baseData.force *= 0.1f;
                projectile.baseData.speed *= 0.7f;
                projectile.baseData.range = 7f;
                beamComp.penetration = 30;
                beamComp.boneType = BasicBeamController.BeamBoneType.Straight;
                beamComp.endAudioEvent = "Stop_WPN_All";
                beamComp.startAudioEvent = "Play_WPN_moonscraperLaser_shot_01";

                projmodule.projectiles[0] = projectile;
                iterator++;
            }

            gun.quality = PickupObject.ItemQuality.B;
            gun.gunClass = GunClass.BEAM;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
    }
}