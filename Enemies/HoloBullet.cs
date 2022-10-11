using System;
using System.Collections.Generic;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = EnemyAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using Alexandria.DungeonAPI;
using static DirectionalAnimation;
using EnemyAPI;

namespace GunRev
{
	public class HoloKinBehaviour : MonoBehaviour
	{
		private void Start()
		{
			self = base.GetComponent<AIActor>();
			SpeculativeRigidbody specRigidbody = self.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision += this.PreRigidbodyCollision;
			Shader shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
			self.sprite.renderer.material.shader = shader;
		}
		private void PreRigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
		{
			Projectile projectile = otherRigidbody.projectile;
			if (projectile)
			{
				PhysicsEngine.SkipCollision = true;
			}
		}
		private AIActor self;
	}
	public class HoloBullet
	{
		public static GameObject prefab;
		public static readonly string guid = "79711df6b2ba4dfaa49b5502dadefcac";
		private static tk2dSpriteCollectionData HolobulletCollection;
		public static void Init()
		{
			HoloBullet.BuildPrefab();
		}
		public static void BuildPrefab()
		{

			if (prefab == null || !EnemyAPI.EnemyBuilder.Dictionary.ContainsKey(guid))
			{
				prefab = EnemyAPI.EnemyBuilder.BuildPrefab("Holo-Kin", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(0, 0), true);
				var enemy = prefab.AddComponent<EnemyBehavior>();
				enemy.aiActor.SetIsFlying(false, "Not A Flying Enemy", false, false);
				enemy.aiActor.name = "Holo-Kin";
				enemy.aiActor.MovementSpeed = 2f;
				enemy.aiActor.knockbackDoer.weight = 100;
				enemy.aiActor.IgnoreForRoomClear = true;
				enemy.gameObject.AddComponent<KillOnRoomClear>();
				enemy.aiActor.CollisionDamage = 0f;
				enemy.aiActor.healthHaver.ForceSetCurrentHealth(30f);
				enemy.aiActor.healthHaver.SetHealthMaximum(45f, null, false);
				enemy.aiActor.specRigidbody.CollideWithOthers = false;
				enemy.aiActor.specRigidbody.CollideWithTileMap = true;
				enemy.aiActor.CollisionKnockbackStrength = 0f;
				enemy.aiActor.PreventBlackPhantom = true;
				enemy.aiActor.ImmuneToAllEffects = true;
				enemy.aiActor.healthHaver.PreventAllDamage = true;
				AIAnimator aiAnimator = enemy.aiAnimator;
				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.FourWay,
					Flipped = new DirectionalAnimation.FlipType[4],
					AnimNames = new string[]
					{
						"run_back_right",
						"run_front_right",
						"run_front_left",
						"run_back_left",

					}
				};
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle_right",
						"idle_left",
					}
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
					name = "die",
					anim = new DirectionalAnimation
						{
							Type = DirectionalAnimation.DirectionType.Single,
							Flipped = new DirectionalAnimation.FlipType[1],
							AnimNames = new string[]
							{

								"die",

							}

						}
					}
				};
				bool flag = HolobulletCollection == null;
				if (flag)
				{
					HolobulletCollection = SpriteBuilder.ConstructCollection(prefab, "Holobullet_Collection");
					UnityEngine.Object.DontDestroyOnLoad(HolobulletCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], HolobulletCollection);
					}
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, HolobulletCollection, new List<int>
					{
						0,
						1,
					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 4f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, HolobulletCollection, new List<int>
					{
						2,
						3,
					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 4f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, HolobulletCollection, new List<int>
					{
						4,
						5,
						6,
						7,
						8,
						9,
					}, "run_front_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 12f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, HolobulletCollection, new List<int>
					{
						10,
						11,
						12,
						13,
						14,
						15,
					}, "run_front_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 12f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, HolobulletCollection, new List<int>
					{
						16,
						17,
						18,
						19,
						20,
						21,
					}, "run_back_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 12f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, HolobulletCollection, new List<int>
					{
						22,
						23,
						24,
						25,
						26,
						27,
					}, "run_back_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 12f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, HolobulletCollection, new List<int>
					{
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
					}, "die", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8;
				}
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				GameObject position = enemy.transform.Find("GunAttachPoint").gameObject;
				position.transform.position = new Vector2(-0.3125f, 0.3125f);
				AIActor SourceEnemy = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
				EnemyAPI.EnemyBuilder.DuplicateAIShooterAndAIBulletBank(prefab, SourceEnemy.aiShooter, SourceEnemy.GetComponent<AIBulletBank>(), 38, position.transform);
				bs.TargetBehaviors = new List<TargetBehaviorBase>
			{
				new TargetPlayerBehavior
				{
					Radius = 35f,
					LineOfSight = true,
					ObjectPermanence = true,
					SearchInterval = 0.25f,
					PauseOnTargetSwitch = false,
					PauseTime = 0.25f,
				}
			};
				bs.AttackBehaviors = new List<AttackBehaviorBase>() {
				new ShootGunBehavior() {
					GroupCooldownVariance = 0.2f,
					LineOfSight = false,
					WeaponType = WeaponType.BulletScript,
					OverrideBulletName = null,
					BulletScript = new CustomBulletScriptSelector(typeof(HolokinScript)),
					FixTargetDuringAttack = true,
					StopDuringAttack = true,
					LeadAmount = 0,
					LeadChance = 1,
					RespectReload = true,
					MagazineCapacity = 6,
					ReloadSpeed = 2f,
					EmptiesClip = true,
					SuppressReloadAnim = false,
					TimeBetweenShots = -1,
					PreventTargetSwitching = true,
					OverrideAnimation = null,
					OverrideDirectionalAnimation = null,
					HideGun = false,
					UseLaserSight = false,
					UseGreenLaser = false,
					PreFireLaserTime = -1,
					AimAtFacingDirectionWhenSafe = false,
					Cooldown = 0.7f,
					CooldownVariance = 0,
					AttackCooldown = 0,
					GlobalCooldown = 0,
					InitialCooldown = 0,
					InitialCooldownVariance = 0,
					GroupName = null,
					GroupCooldown = 0,
					MinRange = 0,
					Range = 16,
					MinWallDistance = 0,
					MaxEnemiesInRoom = 0,
					MinHealthThreshold = 0,
					MaxHealthThreshold = 1,
					HealthThresholds = new float[0],
					AccumulateHealthThresholds = true,
					targetAreaStyle = null,
					IsBlackPhantom = false,
					resetCooldownOnDamage = null,
					RequiresLineOfSight = true,
					MaxUsages = 0,
				}
			};
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new SeekTargetBehavior
				{
					StopWhenInRange = true,
					CustomRange = 15f,
					LineOfSight = true,  
					ReturnToSpawn = false,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0f,
					MaxActiveRange = 0f
				}
			};
				Game.Enemies.Add("ai:holokin", enemy.aiActor);
				SpriteBuilder.AddSpriteToCollection("GunRev/Resources/holobullet/holobullet_idle_left_001", AmmonomiconController.ForceInstance.EncounterIconCollection);
				if (enemy.GetComponent<EncounterTrackable>() != null)
				{
					UnityEngine.Object.Destroy(enemy.GetComponent<EncounterTrackable>());
				}
				enemy.encounterTrackable = enemy.gameObject.AddComponent<EncounterTrackable>();
				enemy.encounterTrackable.journalData = new JournalEntry();
				enemy.encounterTrackable.EncounterGuid = "ed4759bb5ccd4566a0ba8709782df1d5";
				enemy.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
				enemy.encounterTrackable.journalData.SuppressKnownState = false;
				enemy.encounterTrackable.journalData.IsEnemy = true;
				enemy.encounterTrackable.journalData.SuppressInAmmonomicon = false;
				enemy.encounterTrackable.ProxyEncounterGuid = string.Empty;
				enemy.encounterTrackable.journalData.AmmonomiconSprite = "GunRev/Resources/holobullet/holobullet_idle_left_001";
				enemy.encounterTrackable.journalData.enemyPortraitSprite = Alexandria.ItemAPI.ResourceExtractor.GetTextureFromResource("GunRev\\Resources\\holokinammonomicon.png");
				Module.Strings.Enemies.Set("#HOLOKIN", "Holo-Kin");
				Module.Strings.Enemies.Set("#HOLOKIN_SHORTDESC", "Hard Light Bullet");
				Module.Strings.Enemies.Set("#HOLOKIN_LONGDESC", "Previously Hegemony training units, these strange entities have recently succumbed to the dark forces of the Gungeon.\n\nNo matter how hard you try, you're not going to hit one of these.");
				enemy.encounterTrackable.journalData.PrimaryDisplayName = "#HOLOKIN";
				enemy.encounterTrackable.journalData.NotificationPanelDescription = "#HOLOKIN_SHORTDESC";
				enemy.encounterTrackable.journalData.AmmonomiconFullEntry = "#HOLOKIN_LONGDESC";
				EnemyAPI.EnemyBuilder.AddEnemyToDatabase(enemy.gameObject, enemy.aiActor.EnemyGuid);
				EnemyDatabase.GetEntry(enemy.aiActor.EnemyGuid).ForcedPositionInAmmonomicon = 11;
				EnemyDatabase.GetEntry(enemy.aiActor.EnemyGuid).isInBossTab = false;
				EnemyDatabase.GetEntry(enemy.aiActor.EnemyGuid).isNormalEnemy = true;
			}
		}

		private static string[] spritePaths = new string[]
		{
			//idle anims
			"GunRev/Resources/holobullet/holobullet_idle_left_001",
			"GunRev/Resources/holobullet/holobullet_idle_left_002",
			"GunRev/Resources/holobullet/holobullet_idle_right_001",
			"GunRev/Resources/holobullet/holobullet_idle_right_002",
			//run while you still can
			"GunRev/Resources/holobullet/holobullet_run_left_001",
			"GunRev/Resources/holobullet/holobullet_run_left_002",
			"GunRev/Resources/holobullet/holobullet_run_left_003",
			"GunRev/Resources/holobullet/holobullet_run_left_004",
			"GunRev/Resources/holobullet/holobullet_run_left_005",
			"GunRev/Resources/holobullet/holobullet_run_left_006",
			"GunRev/Resources/holobullet/holobullet_run_right_001",
			"GunRev/Resources/holobullet/holobullet_run_right_002",
			"GunRev/Resources/holobullet/holobullet_run_right_003",
			"GunRev/Resources/holobullet/holobullet_run_right_004",
			"GunRev/Resources/holobullet/holobullet_run_right_005",
			"GunRev/Resources/holobullet/holobullet_run_right_006",
			"GunRev/Resources/holobullet/holobullet_run_left_back_001",
			"GunRev/Resources/holobullet/holobullet_run_left_back_002",
			"GunRev/Resources/holobullet/holobullet_run_left_back_003",
			"GunRev/Resources/holobullet/holobullet_run_left_back_004",
			"GunRev/Resources/holobullet/holobullet_run_left_back_005",
			"GunRev/Resources/holobullet/holobullet_run_left_back_006",
			"GunRev/Resources/holobullet/holobullet_run_right_back_001",
			"GunRev/Resources/holobullet/holobullet_run_right_back_002",
			"GunRev/Resources/holobullet/holobullet_run_right_back_003",
			"GunRev/Resources/holobullet/holobullet_run_right_back_004",
			"GunRev/Resources/holobullet/holobullet_run_right_back_005",
			"GunRev/Resources/holobullet/holobullet_run_right_back_006",
			//aaaaaaaaaaaaaaaaaaaaaa
			"GunRev/Resources/holobullet/holobullet_die_001",
			"GunRev/Resources/holobullet/holobullet_die_002",
			"GunRev/Resources/holobullet/holobullet_die_003",
			"GunRev/Resources/holobullet/holobullet_die_004",
			"GunRev/Resources/holobullet/holobullet_die_005",
			"GunRev/Resources/holobullet/holobullet_die_006",
			"GunRev/Resources/holobullet/holobullet_die_007",
			"GunRev/Resources/holobullet/holobullet_die_008",
			"GunRev/Resources/holobullet/holobullet_die_009",
			"GunRev/Resources/holobullet/holobullet_die_010",
			"GunRev/Resources/holobullet/holobullet_die_011",
			"GunRev/Resources/holobullet/holobullet_die_012",
		};
		public class HolokinScript : Script
		{
			//Here we are going to create a basic bullet script.
			public override IEnumerator Top()
			{

				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").bulletBank.GetBullet("default"));
				}
				base.Fire(new Direction(0, Brave.BulletScript.DirectionType.Aim, -1f), new Speed(6f, SpeedType.Absolute), new BasicBullet()); // Shoot a bullet -20 degrees from the enemy aim angle, with a bullet speed of 9.
				base.PostWwiseEvent("Play_WPN_magnum_shot_01", null);
				yield return this.Wait(6);//Here the script will wait 6 frames before firing another shot.
				yield break;
			}
		}
		public class EnemyBehavior : BraveBehaviour
		{
			private RoomHandler m_StartRoom;
			private void Update()
			{
				if (!base.aiActor.HasBeenEngaged) { CheckPlayerRoom(); }
			}
			private void CheckPlayerRoom()
			{
				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom) { base.aiActor.HasBeenEngaged = true; }
			}
			private void Start()
			{
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				HoloKinBehaviour holoKinBehaviour = aiActor.gameObject.AddComponent<HoloKinBehaviour>() as HoloKinBehaviour;
			}
		}
		public class BasicBullet : Bullet
		{
			public BasicBullet() : base("default", false, false, false)
			{
			}

		}
	}
}