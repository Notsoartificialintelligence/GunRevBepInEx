using System;
using System.Collections.Generic;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using AnimationType = Alexandria.EnemyAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using Alexandria.DungeonAPI;
using SpriteBuilder = Alexandria.ItemAPI.SpriteBuilder;
using static DirectionalAnimation;
using EnemyAPI;

namespace GunRev
{
	class Omegear : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "omegear";
		public static GameObject shootpoint;

		private static tk2dSpriteCollectionData OmegearCollection;


		public static List<int> spriteIds2 = new List<int>();

		private static Texture2D BossCardTexture = Alexandria.ItemAPI.ResourceExtractor.GetTextureFromResource("GunRev/Resources/omegear/omegear_bosscard.png");

		public static void Init()
		{
			Omegear.BuildPrefab();
		}
		public static void BuildPrefab()
		{
			if (!(prefab != null || EnemyAPI.EnemyBuilder.Dictionary.ContainsKey(guid)))
			{
				prefab = BossBuilder.BuildPrefab("Omegear", guid, "GunRev/Resources/omegear/gearboss_idle_001.png", new IntVector2(0, 0), new IntVector2(32, 32), false, true);
				var enemy = prefab.AddComponent<EnemyBehavior>();
				OmegearController pain = prefab.AddComponent<OmegearController>();

				AIAnimator aiAnimator = enemy.aiAnimator;

				enemy.aiActor.knockbackDoer.weight = 20f;
				enemy.aiActor.MovementSpeed = 1f;
				enemy.aiActor.healthHaver.PreventAllDamage = false;
				enemy.aiActor.CollisionDamage = 1f;
				enemy.aiActor.HasShadow = false;
				enemy.aiActor.IgnoreForRoomClear = false;
				enemy.aiActor.specRigidbody.CollideWithOthers = true;
				enemy.aiActor.specRigidbody.CollideWithTileMap = true;
				enemy.aiActor.PreventFallingInPitsEver = true;
				enemy.aiActor.healthHaver.ForceSetCurrentHealth(1500f);
				enemy.aiActor.CollisionKnockbackStrength = 10f;
				enemy.aiActor.CanTargetPlayers = true;
				enemy.aiActor.healthHaver.SetHealthMaximum(1500f, null, false);
				AIBeamShooter gaming = enemy.gameObject.AddComponent<AIBeamShooter>();
				enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 54,
					ManualHeight = 58,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,
					Position = enemy.specRigidbody.UnitCenter.ToIntVector2()
				});
				enemy.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{

					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyHitBox,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 50,
					ManualHeight = 54,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,
					Position = enemy.specRigidbody.UnitCenter.ToIntVector2()
				});

				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "idle",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};

				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.FourWay,
					Prefix = "run",
					AnimNames = new string[]
                    {
						"run_left",
						"run_right",
                    },
					Flipped = new DirectionalAnimation.FlipType[1]
				};

				if (OmegearCollection == null)
				{
					OmegearCollection = SpriteBuilder.ConstructCollection(prefab, "GearCollection");
					UnityEngine.Object.DontDestroyOnLoad(OmegearCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], OmegearCollection);
					}
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, OmegearCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8,
					9,
					10,
					11

					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 6f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, OmegearCollection, new List<int>
					{

					12,
					13,
					14,
					15,
					16,
					17,
					18,
					19,
					20,
					21,
					22,
					23,
					24,
					25,
					26,
					27,
					28,
					29,
					30,
					31,
					32,
					33,
					34,
					35,
					36,
					37,
					38,
					39,
					40,
					41,
					42,
					43,
					44,
					45,
					46,
					47,
					48,
					49,
					50,
					51,
					52,
					53,
					54,
					55,
					56,
					57,
					58,
					59,
					60,
					61,
					62,
					63,
					64,
					65,
					66,
					67,
					68,
					69,
					70,
					71,
					72,
					73,
					74,
					75,
					76,
					0,
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8,
					9,
					10,
					11,
					0,
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8,
					9,
					10,
					11

					}, "intro", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 12f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, OmegearCollection, new List<int>
					{

					12

					}, "preintro", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 6f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, OmegearCollection, new List<int>
					{

					77,
					78,
					79,
					80,
					81,
					82,
					83,
					84,
					85,
					86,
					87,
					88

					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 6f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, OmegearCollection, new List<int>
					{

					89,
					90,
					91,
					92,
					93,
					94,
					95,
					96,
					97,
					98,
					99,
					100

					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 6f;
				}

				enemy.aiActor.PreventBlackPhantom = false;

				shootpoint = new GameObject("OmegearCentre");
				shootpoint.transform.parent = enemy.transform;
				shootpoint.transform.position = enemy.sprite.WorldCenter;

				var bs = prefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;

				AIActor actor = EnemyDatabase.GetOrLoadByGuid("4b992de5b4274168a8878ef9bf7ea36b");

				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				bs.TargetBehaviors = new List<TargetBehaviorBase>
				{
					new TargetPlayerBehavior
					{
						Radius = 45f,
						LineOfSight = true,
						ObjectPermanence = true,
						SearchInterval = 0.25f,
						PauseOnTargetSwitch = false,
						PauseTime = 0.25f
					},

				};

				bs.AttackBehaviorGroup.AttackBehaviors = new List<AttackBehaviorGroup.AttackGroupItem>()
				{

					new AttackBehaviorGroup.AttackGroupItem()
						{
						Probability = 1.1f,
						Behavior = new ShootBehavior()
						{
							StopDuring = ShootBehavior.StopType.Attack,
							BulletScript = new CustomBulletScriptSelector(typeof(GearSurround)),
							LeadAmount = 0,

							AttackCooldown = 2f,
							Cooldown = 4f,

							RequiresLineOfSight = true,
							ShootPoint = shootpoint,
							CooldownVariance = 0f,
							GlobalCooldown = 0,
							InitialCooldown = 1,
							InitialCooldownVariance = 0,
							GroupName = null,
							MinRange = 0,
							Range = 1000,
							MinWallDistance = 0,
							MaxEnemiesInRoom = -1,
							MinHealthThreshold = 0,
							MaxHealthThreshold = 1,
							HealthThresholds = new float[0],
							AccumulateHealthThresholds = true,
							targetAreaStyle = null,
							IsBlackPhantom = false,
							resetCooldownOnDamage = null,
							MaxUsages = 0,
							ImmobileDuringStop = true,
							FireAnimation = "idle",

						},
						NickName = "the gear"
					},
					new AttackBehaviorGroup.AttackGroupItem()
						{
						Probability = 0.7f,
						Behavior = new ShootBehavior()
						{
							StopDuring = ShootBehavior.StopType.Attack,
							BulletScript = new CustomBulletScriptSelector(typeof(GearSpin)),
							LeadAmount = 0,

							AttackCooldown = 4f,
							Cooldown = 6f,

							RequiresLineOfSight = true,
							ShootPoint = shootpoint,
							CooldownVariance = 0f,
							GlobalCooldown = 0,
							InitialCooldown = 1,
							InitialCooldownVariance = 0,
							GroupName = null,
							MinRange = 0,
							Range = 1000,
							MinWallDistance = 0,
							MaxEnemiesInRoom = -1,
							MinHealthThreshold = 0,
							MaxHealthThreshold = 1,
							HealthThresholds = new float[0],
							AccumulateHealthThresholds = true,
							targetAreaStyle = null,
							IsBlackPhantom = false,
							resetCooldownOnDamage = null,
							MaxUsages = 0,
							ImmobileDuringStop = true,
							FireAnimation = "idle",
						},
						NickName = "the spin"
					},
					new AttackBehaviorGroup.AttackGroupItem()
						{
						Probability = 0.5f,
						Behavior = new CustomBeholsterLaserBehavior()
						{
							BulletScript = new CustomBulletScriptSelector(typeof(GoToTheMiddleYouLazyPieceOfShitGear)),
							InitialCooldown = 8f,
							firingTime = 10f,
							ShootPoint = enemy.specRigidbody.transform,
                        }
                    },
					/*new AttackBehaviorGroup.AttackGroupItem()
						{
						Probability = 0.3f,
						Behavior = new ShootBehavior()
						{
							StopDuring = ShootBehavior.StopType.Attack,
							BulletScript = new CustomBulletScriptSelector(typeof(BarSpin)),
							LeadAmount = 0,

							AttackCooldown = 8f,
							Cooldown = 10f,

							RequiresLineOfSight = true,
							ShootPoint = shootpoint,
							CooldownVariance = 0f,
							GlobalCooldown = 0,
							InitialCooldown = 1,
							InitialCooldownVariance = 0,
							GroupName = null,
							MinRange = 0,
							Range = 1000,
							MinWallDistance = 0,
							MaxEnemiesInRoom = -1,
							MinHealthThreshold = 0,
							MaxHealthThreshold = 1,
							HealthThresholds = new float[0],
							AccumulateHealthThresholds = true,
							targetAreaStyle = null,
							IsBlackPhantom = false,
							resetCooldownOnDamage = null,
							MaxUsages = 0,
							ImmobileDuringStop = true,
							FireAnimation = "idle",
						},
						NickName = "the bar"
					},*/
					new AttackBehaviorGroup.AttackGroupItem()
						{
						Probability = 1f,
						Behavior = new SummonEnemyBehavior()
						{
							DefineSpawnRadius = true,
							MaxSpawnRadius = 4,
							GroupName = null,
							GlobalCooldown = 0,
							InitialCooldown = 1,
							InitialCooldownVariance = 0,
							EnemeyGuids = new List<string> { "b37bbc3f456b465a8822fc024b74ee73" },
							MaxEnemiesInRoom = 8,
							Cooldown = 6f,
							RequiresLineOfSight = true,
							AttackCooldown = 6f,
							IsBlackPhantom = false,
							MinHealthThreshold = 0,
							MaxHealthThreshold = 1,
							SummonAnim = "idle",
							AccumulateHealthThresholds = true,
							HealthThresholds = new float[0],
							Range = 1000,
							MinRange = 0,
							NumToSpawn = 4,
							MaxUsages = 0,
							SummonVfx = "Global VFX/VFX_SpawnEnemy_Reticle"
						},
						NickName = "the summon"
					}
				};
				bs.MovementBehaviors = new List<MovementBehaviorBase>() {
				new SeekTargetBehavior() {
					StopWhenInRange = false,
					CustomRange = 6,
					LineOfSight = true,
					ReturnToSpawn = true,
					SpawnTetherDistance = 0,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = -0.25f,
					MaxActiveRange = 0
				}
				};
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;

				Game.Enemies.Add("ai:omegear", enemy.aiActor);
				if (enemy.GetComponent<EncounterTrackable>() != null)
				{
					UnityEngine.Object.Destroy(enemy.GetComponent<EncounterTrackable>());
				}
				GenericIntroDoer miniBossIntroDoer = prefab.AddComponent<GenericIntroDoer>();
				prefab.AddComponent<OmegearIntroController>();

				miniBossIntroDoer.triggerType = GenericIntroDoer.TriggerType.PlayerEnteredRoom;
				miniBossIntroDoer.initialDelay = 0.15f;
				miniBossIntroDoer.cameraMoveSpeed = 14;
				miniBossIntroDoer.specifyIntroAiAnimator = null;
				miniBossIntroDoer.BossMusicEvent = "Play_MUS_Ending_Marine_01";
				miniBossIntroDoer.PreventBossMusic = false;
				miniBossIntroDoer.InvisibleBeforeIntroAnim = false;
				miniBossIntroDoer.preIntroAnim = "preintro";
				miniBossIntroDoer.preIntroDirectionalAnim = string.Empty;
				miniBossIntroDoer.introAnim = "intro";
				miniBossIntroDoer.introDirectionalAnim = string.Empty;
				miniBossIntroDoer.continueAnimDuringOutro = false;
				miniBossIntroDoer.cameraFocus = null;
				miniBossIntroDoer.roomPositionCameraFocus = Vector2.zero;
				miniBossIntroDoer.restrictPlayerMotionToRoom = false;
				miniBossIntroDoer.fusebombLock = false;
				miniBossIntroDoer.AdditionalHeightOffset = 0;
				GunRev.Module.Strings.Enemies.Set("#OMEGEAR_NAME", "OMEGEAR");
				GunRev.Module.Strings.Enemies.Set("#OMEGEAR_NAME_SMALL", "Omegear");

				GunRev.Module.Strings.Enemies.Set("#BRASS_KICKER", "BRASS KICKER");
				GunRev.Module.Strings.Enemies.Set("#QUOTE", "");
				enemy.aiActor.OverrideDisplayName = "#OMEGEAR_NAME_SMALL";

				miniBossIntroDoer.portraitSlideSettings = new PortraitSlideSettings()
				{
					bossNameString = "#OMEGEAR_NAME",
					bossSubtitleString = "#BRASS_KICKER",
					bossQuoteString = "#QUOTE",
					bossSpritePxOffset = IntVector2.Zero,
					topLeftTextPxOffset = IntVector2.Zero,
					bottomRightTextPxOffset = IntVector2.Zero,
					bgColor = Color.blue
				};
				if (BossCardTexture)
				{
					miniBossIntroDoer.portraitSlideSettings.bossArtSprite = BossCardTexture;
					miniBossIntroDoer.SkipBossCard = false;
					enemy.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.MainBar;
				}
				else
				{
					miniBossIntroDoer.SkipBossCard = true;
					enemy.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.MainBar;
				}

				SpriteBuilder.AddSpriteToCollection("GunRev/Resources/omegear/omegearicon", AmmonomiconController.ForceInstance.EncounterIconCollection);
				if (enemy.GetComponent<EncounterTrackable>() != null)
				{
					UnityEngine.Object.Destroy(enemy.GetComponent<EncounterTrackable>());
				}
				enemy.encounterTrackable = enemy.gameObject.AddComponent<EncounterTrackable>();
				enemy.encounterTrackable.journalData = new JournalEntry();
				enemy.encounterTrackable.EncounterGuid = "ai:omegear";
				enemy.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
				enemy.encounterTrackable.journalData.SuppressKnownState = false;
				enemy.encounterTrackable.journalData.IsEnemy = true;
				enemy.encounterTrackable.journalData.SuppressInAmmonomicon = false;
				enemy.encounterTrackable.ProxyEncounterGuid = "";
				enemy.encounterTrackable.journalData.AmmonomiconSprite = "GunRev/Resources/omegear/omegearicon";
				enemy.encounterTrackable.journalData.enemyPortraitSprite = Alexandria.ItemAPI.ResourceExtractor.GetTextureFromResource("GunRev\\Resources\\holokinammonomicon.png");
				GunRev.Module.Strings.Enemies.Set("#OMEGEARAMMONOMICON", "Omegear");
				GunRev.Module.Strings.Enemies.Set("#OMEGEARAMMONOMICONSHORT", "Brass Kicker");
				GunRev.Module.Strings.Enemies.Set("#OMEGEARAMMONOMICONLONG", "The result of a lengthy corporate deal between the Flaktory's parent corporation and OmegaCorp, with the intent of creating the most malicious lump of brass ever made. OmegaCorp was initially reluctant to sign the contract, but the promise of \"exposure\" from the Flaktory got them on board.\n\nOne of the most advanced brass marvels of engineering ever created, except for perhaps a pendulum clock.");
				enemy.encounterTrackable.journalData.PrimaryDisplayName = "#OMEGEARAMMONOMICON";
				enemy.encounterTrackable.journalData.NotificationPanelDescription = "#OMEGEARAMMONOMICONSHORT";
				enemy.encounterTrackable.journalData.AmmonomiconFullEntry = "#OMEGEARAMMONOMICONLONG";
				EnemyAPI.EnemyBuilder.AddEnemyToDatabase(enemy.gameObject, "ai:omegear");
				EnemyDatabase.GetEntry("ai:omegear").isInBossTab = true;
				EnemyDatabase.GetEntry("ai:omegear").isNormalEnemy = true;

				miniBossIntroDoer.SkipFinalizeAnimation = true;
				miniBossIntroDoer.RegenerateCache();

				//==================
				//Important for not breaking basegame stuff!
				StaticReferenceManager.AllHealthHavers.Remove(enemy.aiActor.healthHaver);
				//==================

			}
		}


		private static string[] spritePaths = new string[]
		{
			//Idle
			"GunRev/Resources/omegear/gearboss_idle_001.png",
			"GunRev/Resources/omegear/gearboss_idle_002.png",
			"GunRev/Resources/omegear/gearboss_idle_003.png",
			"GunRev/Resources/omegear/gearboss_idle_004.png",
			"GunRev/Resources/omegear/gearboss_idle_005.png",
			"GunRev/Resources/omegear/gearboss_idle_006.png",
			"GunRev/Resources/omegear/gearboss_idle_007.png",
			"GunRev/Resources/omegear/gearboss_idle_008.png",
			"GunRev/Resources/omegear/gearboss_idle_009.png",
			"GunRev/Resources/omegear/gearboss_idle_010.png",
			"GunRev/Resources/omegear/gearboss_idle_011.png",
			"GunRev/Resources/omegear/gearboss_idle_012.png",
			//Intro (long!!!)
			"GunRev/Resources/omegear/omegear_intro_001.png",
			"GunRev/Resources/omegear/omegear_intro_002.png",
			"GunRev/Resources/omegear/omegear_intro_003.png",
			"GunRev/Resources/omegear/omegear_intro_004.png",
			"GunRev/Resources/omegear/omegear_intro_005.png",
			"GunRev/Resources/omegear/omegear_intro_006.png",
			"GunRev/Resources/omegear/omegear_intro_007.png",
			"GunRev/Resources/omegear/omegear_intro_008.png",
			"GunRev/Resources/omegear/omegear_intro_009.png",
			"GunRev/Resources/omegear/omegear_intro_010.png",
			"GunRev/Resources/omegear/omegear_intro_011.png",
			"GunRev/Resources/omegear/omegear_intro_012.png",
			"GunRev/Resources/omegear/omegear_intro_013.png",
			"GunRev/Resources/omegear/omegear_intro_014.png",
			"GunRev/Resources/omegear/omegear_intro_015.png",
			"GunRev/Resources/omegear/omegear_intro_016.png",
			"GunRev/Resources/omegear/omegear_intro_017.png",
			"GunRev/Resources/omegear/omegear_intro_018.png",
			"GunRev/Resources/omegear/omegear_intro_019.png",
			"GunRev/Resources/omegear/omegear_intro_020.png",
			"GunRev/Resources/omegear/omegear_intro_021.png",
			"GunRev/Resources/omegear/omegear_intro_022.png",
			"GunRev/Resources/omegear/omegear_intro_023.png",
			"GunRev/Resources/omegear/omegear_intro_024.png",
			"GunRev/Resources/omegear/omegear_intro_025.png",
			"GunRev/Resources/omegear/omegear_intro_026.png",
			"GunRev/Resources/omegear/omegear_intro_027.png",
			"GunRev/Resources/omegear/omegear_intro_028.png",
			"GunRev/Resources/omegear/omegear_intro_029.png",
			"GunRev/Resources/omegear/omegear_intro_030.png",
			"GunRev/Resources/omegear/omegear_intro_031.png",
			"GunRev/Resources/omegear/omegear_intro_032.png",
			"GunRev/Resources/omegear/omegear_intro_033.png",
			"GunRev/Resources/omegear/omegear_intro_034.png",
			"GunRev/Resources/omegear/omegear_intro_035.png",
			"GunRev/Resources/omegear/omegear_intro_036.png",
			"GunRev/Resources/omegear/omegear_intro_037.png",
			"GunRev/Resources/omegear/omegear_intro_038.png",
			"GunRev/Resources/omegear/omegear_intro_039.png",
			"GunRev/Resources/omegear/omegear_intro_040.png",
			"GunRev/Resources/omegear/omegear_intro_041.png",
			"GunRev/Resources/omegear/omegear_intro_042.png",
			"GunRev/Resources/omegear/omegear_intro_043.png",
			"GunRev/Resources/omegear/omegear_intro_044.png",
			"GunRev/Resources/omegear/omegear_intro_045.png",
			"GunRev/Resources/omegear/omegear_intro_046.png",
			"GunRev/Resources/omegear/omegear_intro_047.png",
			"GunRev/Resources/omegear/omegear_intro_048.png",
			"GunRev/Resources/omegear/omegear_intro_049.png",
			"GunRev/Resources/omegear/omegear_intro_050.png",
			"GunRev/Resources/omegear/omegear_intro_051.png",
			"GunRev/Resources/omegear/omegear_intro_052.png",
			"GunRev/Resources/omegear/omegear_intro_053.png",
			"GunRev/Resources/omegear/omegear_intro_054.png",
			"GunRev/Resources/omegear/omegear_intro_055.png",
			"GunRev/Resources/omegear/omegear_intro_056.png",
			"GunRev/Resources/omegear/omegear_intro_057.png",
			"GunRev/Resources/omegear/omegear_intro_058.png",
			"GunRev/Resources/omegear/omegear_intro_059.png",
			"GunRev/Resources/omegear/omegear_intro_060.png",
			"GunRev/Resources/omegear/omegear_intro_061.png",
			"GunRev/Resources/omegear/omegear_intro_062.png",
			"GunRev/Resources/omegear/omegear_intro_063.png",
			"GunRev/Resources/omegear/omegear_intro_064.png",
			"GunRev/Resources/omegear/omegear_intro_065.png",
			"GunRev/Resources/omegear/omegear_intro_066.png",
			//whew
			"GunRev/Resources/omegear/gearboss_idle_right_001.png",
			"GunRev/Resources/omegear/gearboss_idle_right_002.png",
			"GunRev/Resources/omegear/gearboss_idle_right_003.png",
			"GunRev/Resources/omegear/gearboss_idle_right_004.png",
			"GunRev/Resources/omegear/gearboss_idle_right_005.png",
			"GunRev/Resources/omegear/gearboss_idle_right_006.png",
			"GunRev/Resources/omegear/gearboss_idle_right_007.png",
			"GunRev/Resources/omegear/gearboss_idle_right_008.png",
			"GunRev/Resources/omegear/gearboss_idle_right_009.png",
			"GunRev/Resources/omegear/gearboss_idle_right_010.png",
			"GunRev/Resources/omegear/gearboss_idle_right_011.png",
			"GunRev/Resources/omegear/gearboss_idle_right_012.png",

			"GunRev/Resources/omegear/gearboss_idle_left_001.png",
			"GunRev/Resources/omegear/gearboss_idle_left_002.png",
			"GunRev/Resources/omegear/gearboss_idle_left_003.png",
			"GunRev/Resources/omegear/gearboss_idle_left_004.png",
			"GunRev/Resources/omegear/gearboss_idle_left_005.png",
			"GunRev/Resources/omegear/gearboss_idle_left_006.png",
			"GunRev/Resources/omegear/gearboss_idle_left_007.png",
			"GunRev/Resources/omegear/gearboss_idle_left_008.png",
			"GunRev/Resources/omegear/gearboss_idle_left_009.png",
			"GunRev/Resources/omegear/gearboss_idle_left_010.png",
			"GunRev/Resources/omegear/gearboss_idle_left_011.png",
			"GunRev/Resources/omegear/gearboss_idle_left_012.png"
		};
		public class EnemyBehavior : BraveBehaviour
		{
			private RoomHandler m_StartRoom;
			public void Update()
			{
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				if (!base.aiActor.HasBeenEngaged)
				{
					CheckPlayerRoom();
				}
			}
			private void CheckPlayerRoom()
			{
				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom && GameManager.Instance.PrimaryPlayer.IsInCombat == true)
				{

					GameManager.Instance.StartCoroutine(LateEngage());
				}
				else
				{
					base.aiActor.HasBeenEngaged = false;
				}
			}
			private IEnumerator LateEngage()
			{
				yield return new WaitForSeconds(0.5f);
				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom)
				{
					base.aiActor.HasBeenEngaged = true;
				}
				yield break;
			}

			public void Start()
			{
				this.aiActor.knockbackDoer.SetImmobile(false, "omegear");
				base.aiActor.HasBeenEngaged = false;
				//Important for not breaking basegame stuff!
				StaticReferenceManager.AllHealthHavers.Remove(base.aiActor.healthHaver);
			}

		}
	}

	public class GoToTheMiddleYouLazyPieceOfShitGear : Script
    {
        public override IEnumerator Top()
        {
			this.BulletBank.aiActor.PathfindToPosition(this.BulletBank.aiActor.GetAbsoluteParentRoom().GetCenterCell().ToCenterVector2());
			this.BulletBank.aiActor.SimpleMoveToPosition(this.BulletBank.aiActor.GetAbsoluteParentRoom().GetCenterCell().ToCenterVector2());
			yield return this.Wait(360);
		}
    }
	/*public class BarSpin : Script
    {
        protected override IEnumerator Top()
        {
			this.BulletBank.aiActor.PathfindToPosition(this.BulletBank.aiActor.GetAbsoluteParentRoom().GetCenterCell().ToCenterVector2());
			this.BulletBank.aiActor.SimpleMoveToPosition(this.BulletBank.aiActor.GetAbsoluteParentRoom().GetCenterCell().ToCenterVector2());
			yield return this.Wait(32);
			int rand1 = UnityEngine.Random.Range(120, 300);
			int rand2 = 600 - rand1;
			for (int i = 0; i < 4; i++)
            {
				float num = 6 * (11 * 2 * (60 / 90) / 25);
				float num2 = 11 * 2 * (60 / 90) / 25;
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("cd4a4b7f612a4ba9a720b9f97c52f38c").bulletBank.GetBullet("default"));
				for (int l = 0; l < 15; l++)
				{
					base.Fire(new Offset(l + 1, 0, i * 90), new Direction(i * 90), new Speed(num + num2 * i), new BarSpin.BarBullet(this, "default", 6 + i, rand1, rand2));
					yield return this.Wait(2);
				}
			}
			yield return this.Wait(630);

		}
		public class BarBullet : Bullet
        {
			public BarBullet(BarSpin parent, string bulletName, int spawnTime, int rand1, int rand2) : base(bulletName, true, false, true)
            {
				this.m_parent = parent;
				this.m_spawntime = spawnTime;
				this.m_rand1 = rand1;
				this.m_rand2 = rand2;
            }
            protected override IEnumerator Top()
            {
				this.ChangeSpeed(new Speed(0), 90);
				yield return this.Wait(90);
				this.ChangeDirection(new Direction(90, Brave.BulletScript.DirectionType.Relative));
				yield return this.Wait(1);
				this.ChangeSpeed(new Speed(this.m_spawntime / 25 * (11 * 2) * (Mathf.PI / 4) * (60 / 120), SpeedType.Relative));
				//90 / 120, Brave.BulletScript.DirectionType.Sequence
				this.ChangeDirection(new Direction(12), this.m_rand1);
				yield return this.Wait(this.m_rand1);
				int thing = (int)this.Speed;
				for (int i = thing; i > 0; i--)
                {
					this.ChangeSpeed(new Speed(i, SpeedType.Relative));
					yield return this.Wait(3);
				}
				this.ChangeDirection(new Direction((90 / 120) * -1, Brave.BulletScript.DirectionType.Sequence), this.m_rand2);
				for (int i = 0; i < thing; i++)
				{
					this.ChangeSpeed(new Speed(i, SpeedType.Relative));
					yield return this.Wait(3);
				}
				yield return this.Wait(this.m_rand2);
				this.Vanish(false);
            }
            private BarSpin m_parent;
			private int m_spawntime;
			private int m_rand1;
			private int m_rand2;
        }
    }*/
	public class GearSpin : Script
    {
        public override IEnumerator Top()
        {
			hasSpawnedAllProjectiles = false;
			for (int i = 0; i < 8; i++)
			{
				for (int l = 0; l < 3; l++)
                {
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").bulletBank.GetBullet("default"));
					base.Fire(new Offset(l, 0, i * 45f), new Direction(i * 45f), new Speed(0f, SpeedType.Absolute), new GearSpin.Pain(this, "default"));
					yield return this.Wait(4);
				}
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("cd4a4b7f612a4ba9a720b9f97c52f38c").bulletBank.GetBullet("default"));
				base.Fire(new Offset(4, 0, i * 45f), new Direction(i * 45f), new Speed(0f, SpeedType.Absolute), new GearSpin.Spike(this, "default"));
				yield return this.Wait(4);
			}
			hasSpawnedAllProjectiles = true;
			yield break;
		}
		private bool hasSpawnedAllProjectiles;

		public class Pain : Bullet
        {
			public Pain(GearSpin parent, string bulletName) : base(bulletName, true, false, false)
            {
				this.m_parent = parent;
            }
            public override IEnumerator Top()
            {
                while (true)
                {
					if (m_parent.hasSpawnedAllProjectiles)
                    {
						this.ChangeSpeed(new Speed(12f, SpeedType.Absolute));
						yield break;
                    }
					else
                    {
						yield return this.Wait(1);
                    }
                }
            }
            private GearSpin m_parent;
        }
		public class Spike : Bullet
        {
			public Spike(GearSpin parent, string bulletName) : base(bulletName, true, false, true)
            {
				this.m_parent = parent;
			}
			public override IEnumerator Top()
			{
				while (true)
				{
					if (m_parent.hasSpawnedAllProjectiles)
					{
						this.ChangeSpeed(new Speed(12f, SpeedType.Absolute));
						for (int i = 0; i < 60; i++)
						{
							float aim = this.GetAimDirection(1f, 12f);
							float delta = BraveMathCollege.ClampAngle180(aim - this.Direction);
							if (Mathf.Abs(delta) > 100f)
							{
								yield break;
							}
							this.Direction += Mathf.MoveTowards(0f, delta, 3f);
							yield return this.Wait(1);
						}
						yield break;
					}
					else
					{
						yield return this.Wait(1);
					}
				}
			}
			private GearSpin m_parent;
		}
    }
	public class GearSurround : Script
	{
		public override IEnumerator Top()
		{
			for (int l = 0; l < 3; l++)
            {
				if (l % 2 == 0)
                {
					additionthingy = 11.25f;
                }
				else
                {
					additionthingy = 0f;
                }
				hasSpawnedAllProjectiles = false;
				for (int i = 0; i < 16; i++)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").bulletBank.GetBullet("default"));
					base.Fire(new Offset(3, 0, i * 22.5f + additionthingy), new Direction(i * 22.5f + additionthingy), new Speed(0f, SpeedType.Absolute), new GearSurround.Pain(this, "default"));
					if (i % 2 == 0)
					{
						base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("cd4a4b7f612a4ba9a720b9f97c52f38c").bulletBank.GetBullet("default"));
						base.Fire(new Offset(4, 0, i * 22.5f + additionthingy), new Direction(i * 22.5f + additionthingy), new Speed(0f, SpeedType.Absolute), new GearSurround.SpikeBullet(this, "default"));
					}
					yield return this.Wait(4);
				}
				hasSpawnedAllProjectiles = true;
				yield return this.Wait(8);
			}
			yield break;
		}
		private float additionthingy;
		private bool hasSpawnedAllProjectiles;
		public class Pain : Bullet
		{
			public Pain(GearSurround parent, string bulletName) : base(bulletName, true, false, false)
			{
				this.m_parent = parent;
			}
			public override IEnumerator Top()
			{
				while (true)
				{
					if (m_parent.hasSpawnedAllProjectiles)
					{
						this.ChangeSpeed(new Speed(1f, SpeedType.Absolute));
						while (this.Speed < 12f)
						{
							yield return new WaitForSeconds(0.5f);
							this.ChangeSpeed(new Speed(Speed + 1f, SpeedType.Absolute));
							this.ChangeDirection(new Direction(Direction + 2f));
						}
						m_parent = null;
						yield break;
					}
                    else
                    {
						yield return this.Wait(1);
                    }
				}
			}
			private GearSurround m_parent;
		}
		public class SpikeBullet : Bullet
		{
			public SpikeBullet(GearSurround parent, string bulletName) : base(bulletName, true, false, false)
			{
				this.m_parent = parent;
			}
			public override IEnumerator Top()
			{
				while (true)
				{
					if (m_parent.hasSpawnedAllProjectiles)
					{
						this.ChangeSpeed(new Speed(1f, SpeedType.Absolute));
						while (this.Speed < 12f)
						{
							yield return new WaitForSeconds(0.5f);
							this.ChangeSpeed(new Speed(Speed + 1f, SpeedType.Absolute));
							this.ChangeDirection(new Direction(Direction + 2f));
						}
						m_parent = null;
						yield break;
					}
					else
					{
						yield return this.Wait(1);
					}
				}
			}
			private GearSurround m_parent;
		}
	}
}