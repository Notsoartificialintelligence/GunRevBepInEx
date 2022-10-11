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
    public class DiscouragementBeam : AdvancedGunBehavior
    {
        public static int ApertureScienceID;
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Aperture Science Thermal Discouragement Beam", "discouragementbeam");
            Game.Items.Rename("outdated_gun_mods:aperture_science_thermal_discouragement_beam", "ai:thermal_discouragement_beam");
            var behav = gun.gameObject.AddComponent<DiscouragementBeam>();
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("It Burns!");
            gun.SetLongDescription("Has a chance to set enemies on fire.\n\nThe Aperture Science Thermal Discouragement Beam may be utilised as a weapon, although it contradicts its original scientific nature and may result in a mandatory excursion to Android Hell.");

            gun.SetupSprite(null, "discouragementbeam_idle_001", 8);

            gun.carryPixelOffset = new IntVector2(0, 5);
            gun.SetAnimationFPS(gun.shootAnimation, 32);
            gun.isAudioLoop = true;
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(86) as Gun, true, false);
            gun.doesScreenShake = false;
            gun.DefaultModule.ammoCost = 15;
            gun.DefaultModule.angleVariance = 0;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Beam;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.muzzleFlashEffects.type = VFXPoolType.None;
            gun.DefaultModule.cooldownTime = 0.001f;
            gun.DefaultModule.numberOfShotsInClip = 3000;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "red_beam";
            gun.barrelOffset.transform.localPosition = new Vector3(0.875f, 0.4375f, 0f);
            gun.SetBaseMaxAmmo(2000);
            gun.ammo = 2000;
            ApertureScienceID = gun.PickupObjectId;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 3;

            List<string> BeamAnimPaths = new List<string>()
            {
                "GunRev/Resources/discouragementbeam_mid_001",
            };
            List<string> BeamEndPaths = new List<string>()
            {
                "GunRev/Resources/discouragementbeam_impact_001",
            };

            //BULLET STATS
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);

            BasicBeamController beamComp = projectile.GenerateBeamPrefab(
                "GunRev/Resources/discouragementbeam_mid_001",
                new Vector2(5, 3),
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
                new Vector2(5, 7),
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
            projectile.baseData.force *= 0.6f;
            projectile.baseData.speed *= 10f;
            projectile.AppliesFire = true;
            projectile.FireApplyChance = 0.1f;
            beamComp.PenetratesCover = false;
            beamComp.penetration = 9999999;
            beamComp.boneType = BasicBeamController.BeamBoneType.Straight;
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