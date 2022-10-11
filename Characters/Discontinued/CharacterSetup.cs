using GunRev;//oooo scary its throwing an error, just change it to your mods name space
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BepInEx;
using Alexandria.CharacterAPI;

namespace CustomCharacters
{
    class example : BaseUnityPlugin
    {
        public void Awake()
        {

        }
        public void Start()
        {


			var name = Loader.BuildCharacter("GunRev/Characters/Discontinued", Module.GUID, new Vector3(15.8f, 26.6f, 27.1f), false, new Vector3(15.3f, 24.8f, 25.3f), true, false, false, true, true, new GlowMatDoer(new Color32(224, 169, 0, 255), 4.55f, 55),
                new GlowMatDoer(new Color32(255, 69, 248, 255), 1.55f, 55), 2, false, "").nameInternal;

            Loader.SetupCustomBreachAnimation(name, "sleep_in", 12, tk2dSpriteAnimationClip.WrapMode.Once);
            Loader.SetupCustomBreachAnimation(name, "sleep_hold", 2, tk2dSpriteAnimationClip.WrapMode.Loop);
            Loader.SetupCustomBreachAnimation(name, "sleep_out", 10, tk2dSpriteAnimationClip.WrapMode.Once);
            Loader.SetupCustomBreachAnimation(name, "select_idle", 12, tk2dSpriteAnimationClip.WrapMode.LoopFidget, 1, 4);

            Loader.AddPhase(name, new CharacterSelectIdlePhase
            {
                endVFXSpriteAnimator = null,
                vfxSpriteAnimator = null,
                holdAnimation = "sleep_hold",
                inAnimation = "sleep_in",
                outAnimation = "sleep_out",
                optionalHoldIdleAnimation = "",
                holdMax = 10,
                holdMin = 6,
                optionalHoldChance = 0,
                vfxHoldPeriod = 0,
                vfxTrigger = CharacterSelectIdlePhase.VFXPhaseTrigger.NONE,
            });

            Loader.AddCoopBlankOverride(name, DiscontinuedOverrideBlank);
        }
		//the value returned here is how long of a cool down the "blank" has default is 5 i think 
		public static float DiscontinuedOverrideBlank(PlayerController self)
		{
			if (ghostProj == null)
			{
				ghostProj = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0]);
				ghostProj.gameObject.SetActive(false);

				ghostProj.baseData.speed = 50;
				ghostProj.baseData.damage = 6;
				ghostProj.baseData.UsesCustomAccelerationCurve = true;
				ghostProj.baseData.AccelerationCurve = new AnimationCurve
				{
					postWrapMode = WrapMode.Clamp,
					preWrapMode = WrapMode.Clamp,
					keys = new Keyframe[]
					{
					new Keyframe
					{
						time = 0f,
						value = 0f,
						inTangent = 0f,
						outTangent = 0f
					},
					new Keyframe
					{
						time = 1f,
						value = 1f,
						inTangent = 2f,
						outTangent = 2f
					},
					}
				};
				ghostProj.baseData.CustomAccelerationCurveDuration = 0.3f;
				ghostProj.baseData.IgnoreAccelCurveTime = 0f;
				ghostProj.shouldRotate = true;
				ghostProj.SetProjectileSpriteRight("discontinued_ghost_proj", 9, 8, true, tk2dBaseSprite.Anchor.LowerLeft);
			}
			for (int i = 0; i < 6; i++)
			{
				GameObject gameObject = SpawnManager.SpawnProjectile(ghostProj.gameObject, self.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, i * 60), true);
				gameObject.SetActive(true);
				Projectile component = gameObject.GetComponent<Projectile>();
				component.Owner = self;
				component.Shooter = self.specRigidbody;
				HomingModifier homing = gameObject.GetOrAddComponent<HomingModifier>();
				homing.HomingRadius = 20;
				homing.AngularVelocity = 35;
			}
			return 5f;

		}
		static Projectile ghostProj;
	}
}