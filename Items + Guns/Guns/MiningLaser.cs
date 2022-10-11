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
    public class MiningLaser : AdvancedGunBehavior
    {
        public static int kills = 0;

        public static Projectile yellowbeam;
        public static Projectile orangebeam;
        public static Projectile redbeam;

        private bool IsOrangeBeam = false;
        private bool IsRedBeam = false;
        public static void Add()
        {

            Gun gun = ETGMod.Databases.Items.NewGun("Duplicant Mining Laser", "mininglaser");
            Game.Items.Rename("outdated_gun_mods:duplicant_mining_laser", "ai:duplicant_mining_laser");
            var behav = gun.gameObject.AddComponent<MiningLaser>();
            behav.preventNormalFireAudio = true;
            gun.SetShortDescription("Skills Not Included");
            gun.SetLongDescription("Damage and knockback increases with the number of enemies defeated by this weapon.\n\nA standard-issue colony matter manipulator, famously given to all Gravitas Company personnel. Its controls seem to be jammed on the 'excavation' setting.");

            gun.SetupSprite(null, "mininglaser_idle_001", 8);

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
            gun.DefaultModule.numberOfShotsInClip = 1300;
            gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;
            gun.DefaultModule.customAmmoType = "yellow_beam";
            gun.barrelOffset.transform.localPosition = new Vector3(1.90625f, 0.59375f, 0f);
            gun.SetBaseMaxAmmo(1300);
            gun.ammo = 1300;

            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).loopStart = 3;

            List<string> YellowBeamAnimPaths = new List<string>()
            {
                "GunRev/Resources/mininglaser_yellow_mid_001",
            };
            List<string> YellowBeamEndPaths = new List<string>()
            {
                "GunRev/Resources/mininglaser_yellow_end_001",
            };
            List<string> YellowBeamStartPaths = new List<string>()
            {
                "GunRev/Resources/mininglaser_yellow_start_001"
            };

            List<string> OrangeBeamAnimPaths = new List<string>()
            {
                "GunRev/Resources/mininglaser_orange_mid_001",
            };
            List<string> OrangeBeamEndPaths = new List<string>()
            {
                "GunRev/Resources/mininglaser_orange_end_001",
            };
            List<string> OrangeBeamStartPaths = new List<string>()
            {
                "GunRev/Resources/mininglaser_orange_start_001"
            };

            List<string> RedBeamAnimPaths = new List<string>()
            {
                "GunRev/Resources/mininglaser_red_mid_001",
            };
            List<string> RedBeamEndPaths = new List<string>()
            {
                "GunRev/Resources/mininglaser_red_end_001",
            };
            List<string> RedBeamStartPaths = new List<string>()
            {
                "GunRev/Resources/mininglaser_red_start_001"
            };

            //BULLET STATS
            yellowbeam = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);

            BasicBeamController yelbeamComp = yellowbeam.GenerateBeamPrefab(
                "GunRev/Resources/mininglaser_yellow_mid_001",
                new Vector2(10, 4),
                new Vector2(0, 0),
                YellowBeamAnimPaths,
                9,
                //Impact
                null,
                -1,
                null,
                null,
                //End
                YellowBeamEndPaths,
                9,
                new Vector2(9, 4),
                new Vector2(0, 0),
                //Beginning
                YellowBeamStartPaths,
                -1,
                new Vector2(4, 4),
                new Vector2(0, 0)
                );

            yellowbeam.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(yellowbeam.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(yellowbeam);
            yellowbeam.baseData.damage = 14f;
            yellowbeam.baseData.force = 0.5f;
            yellowbeam.baseData.speed = 18f;
            yelbeamComp.penetration = 0;
            yelbeamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
            yelbeamComp.interpolateStretchedBones = false;
            yelbeamComp.endAudioEvent = "Stop_WPN_All";
            yelbeamComp.startAudioEvent = "Play_WPN_moonscraperLaser_shot_01";

            orangebeam = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);

            BasicBeamController orabeamComp = orangebeam.GenerateBeamPrefab(
                "GunRev/Resources/mininglaser_orange_mid_001",
                new Vector2(10, 4),
                new Vector2(0, 0),
                OrangeBeamAnimPaths,
                9,
                //Impact
                null,
                -1,
                null,
                null,
                //End
                OrangeBeamEndPaths,
                9,
                new Vector2(9, 4),
                new Vector2(0, 0),
                //Beginning
                OrangeBeamStartPaths,
                -1,
                new Vector2(4, 4),
                new Vector2(0, 0)
                );

            orangebeam.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(orangebeam.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(orangebeam);
            orangebeam.baseData.damage = 17f;
            orangebeam.baseData.force = 0.85f;
            orangebeam.baseData.speed = 18f;
            orabeamComp.penetration = 0;
            orabeamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
            orabeamComp.interpolateStretchedBones = false;
            orabeamComp.endAudioEvent = "Stop_WPN_All";
            orabeamComp.startAudioEvent = "Play_WPN_moonscraperLaser_shot_01";

            redbeam = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(86) as Gun).DefaultModule.projectiles[0]);

            BasicBeamController redbeamComp = redbeam.GenerateBeamPrefab(
                "GunRev/Resources/mininglaser_red_mid_001",
                new Vector2(10, 4),
                new Vector2(0, 0),
                RedBeamAnimPaths,
                9,
                //Impact
                null,
                -1,
                null,
                null,
                //End
                RedBeamEndPaths,
                9,
                new Vector2(9, 4),
                new Vector2(0, 0),
                //Beginning
                RedBeamStartPaths,
                -1,
                new Vector2(4, 4),
                new Vector2(0, 0)
                );

            redbeam.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(redbeam.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(redbeam);
            redbeam.baseData.damage = 24f;
            redbeam.baseData.force = 1.25f;
            redbeam.baseData.speed = 18f;
            redbeamComp.penetration = 0;
            redbeamComp.boneType = BasicBeamController.BeamBoneType.Projectile;
            redbeamComp.interpolateStretchedBones = false;
            redbeamComp.endAudioEvent = "Stop_WPN_All";
            redbeamComp.startAudioEvent = "Play_WPN_moonscraperLaser_shot_01";

            gun.DefaultModule.projectiles[0] = yellowbeam;

            gun.quality = PickupObject.ItemQuality.B;
            gun.gunClass = GunClass.BEAM;
            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }
        public static void OnHitEnemy(Projectile projectile, SpeculativeRigidbody hitRigidbody, bool fatal)
        {
            if (hitRigidbody.aiActor != null)
            {
                if (fatal == true)
                {
                    Gun gun = projectile.Owner.CurrentGun;
                    if (gun.gunName == "Duplicant Mining Laser")
                    {
                        if (hitRigidbody.aiActor.healthHaver.IsBoss)
                        {
                            kills += 5;
                        }
                        else
                        {
                            kills++;
                        }
                    }
                }
            }
        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            projectile.OnHitEnemy += OnHitEnemy;
        }
        protected override void Update()
        {
            if (gun.CurrentOwner)
            {
                if (kills > 30 && kills <= 60 && !IsOrangeBeam)
                {
                    gun.DefaultModule.projectiles.Add(orangebeam);
                    gun.DefaultModule.projectiles.Remove(yellowbeam);
                    gun.DefaultModule.projectiles.Remove(redbeam);
                    IsOrangeBeam = true;
                    IsRedBeam = false;
                }
                if (kills > 60 && !IsRedBeam)
                {
                    gun.DefaultModule.projectiles.Add(redbeam);
                    gun.DefaultModule.projectiles.Remove(yellowbeam);
                    gun.DefaultModule.projectiles.Remove(orangebeam);
                    IsOrangeBeam = false;
                    IsRedBeam = true;
                }
                if (!gun.PreventNormalFireAudio)
                {
                    this.gun.PreventNormalFireAudio = true;
                }
            }
        }
    }
}