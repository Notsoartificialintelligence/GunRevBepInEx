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
	public class RadioGuy
	{
		public static GameObject prefab;
		public static readonly string guid = "79711df6b2ba4dfaa49b5502dadefcac";
		private static tk2dSpriteCollectionData RadioGuyCollection;
		public static void Init()
		{
			RadioGuy.BuildPrefab();
		}
		public static void BuildPrefab()
		{

			if (prefab == null || !EnemyAPI.EnemyBuilder.Dictionary.ContainsKey(guid))
			{
				prefab = EnemyAPI.EnemyBuilder.BuildPrefab("Walkin Talkin", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(0, 0), true);
				var enemy = prefab.AddComponent<EnemyBehavior>();
				enemy.aiActor.SetIsFlying(false, "Not A Flying Enemy", false, false);
				enemy.aiActor.name = "Walkin Talkin";
				enemy.aiActor.MovementSpeed = 2f;
				enemy.aiActor.knockbackDoer.weight = 20;
				enemy.aiActor.IgnoreForRoomClear = false;
				enemy.aiActor.CollisionDamage = 1f;
				enemy.aiActor.healthHaver.ForceSetCurrentHealth(30f);
				enemy.aiActor.healthHaver.SetHealthMaximum(45f, null, false);
				enemy.aiActor.specRigidbody.CollideWithOthers = true;
				enemy.aiActor.specRigidbody.CollideWithTileMap = true;
				enemy.aiActor.CollisionKnockbackStrength = 3f;
				enemy.aiActor.PreventBlackPhantom = true;
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
				bool flag = RadioGuyCollection == null;
				if (flag)
				{
					RadioGuyCollection = SpriteBuilder.ConstructCollection(prefab, "RadioGuy_Collection");
					UnityEngine.Object.DontDestroyOnLoad(RadioGuyCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], RadioGuyCollection);
					}
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, RadioGuyCollection, new List<int>
					{
						0,
						1,
					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 4f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, RadioGuyCollection, new List<int>
					{
						2,
						3,
					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 4f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, RadioGuyCollection, new List<int>
					{
						4,
						5,
						6,
						7,
						8,
						9,
					}, "run_front_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 12f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, RadioGuyCollection, new List<int>
					{
						10,
						11,
						12,
						13,
						14,
						15,
					}, "run_front_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 12f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, RadioGuyCollection, new List<int>
					{
						16,
						17,
						18,
						19,
						20,
						21,
					}, "run_back_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 12f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, RadioGuyCollection, new List<int>
					{
						22,
						23,
						24,
						25,
						26,
						27,
					}, "run_back_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 12f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, RadioGuyCollection, new List<int>
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
				new ShootBehavior() {

				}
			};
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new FleeTargetBehavior
				{
					TooCloseDistance = 15,
					ForceRun = true,
					DesiredDistance = 25,
					CloseDistance = 20
				}
			};
				Game.Enemies.Add("ai:walkin_talkin", enemy.aiActor);
				SpriteBuilder.AddSpriteToCollection("GunRev/Resources/radioguy/radio_idle_front_left_001", AmmonomiconController.ForceInstance.EncounterIconCollection);
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
				enemy.encounterTrackable.journalData.AmmonomiconSprite = "GunRev/Resources/radioguy/radio_idle_front_left_001";
				enemy.encounterTrackable.journalData.enemyPortraitSprite = Alexandria.ItemAPI.ResourceExtractor.GetTextureFromResource("GunRev\\Resources\\holokinammonomicon.png");
				Module.Strings.Enemies.Set("#WALKIN", "Walkin Talkin");
				Module.Strings.Enemies.Set("#WALKIN_SHORTDESC", "Kith And Kin");
				Module.Strings.Enemies.Set("#WALKIN_LONGDESC", "Living reinforcement radios initially built for cross-floor communication. Their frequencies are now corrupted and have managed to technologically phase beyond The Curtain, which gives them the ability to summon allies.\n\nLooks like an easy fight, until it brings its friends.");
				enemy.encounterTrackable.journalData.PrimaryDisplayName = "#WALKIN";
				enemy.encounterTrackable.journalData.NotificationPanelDescription = "#WALKIN_SHORTDESC";
				enemy.encounterTrackable.journalData.AmmonomiconFullEntry = "#WALKIN_LONGDESC";
				EnemyAPI.EnemyBuilder.AddEnemyToDatabase(enemy.gameObject, enemy.aiActor.EnemyGuid);
				EnemyDatabase.GetEntry(enemy.aiActor.EnemyGuid).ForcedPositionInAmmonomicon = 13;
				EnemyDatabase.GetEntry(enemy.aiActor.EnemyGuid).isInBossTab = false;
				EnemyDatabase.GetEntry(enemy.aiActor.EnemyGuid).isNormalEnemy = true;
			}
		}

		private static string[] spritePaths = new string[]
		{
			//idle
			"GunRev/Resources/radioguy/radio_idle_left_001",
			"GunRev/Resources/radioguy/radio_idle_left_002",
			"GunRev/Resources/radioguy/radio_idle_right_001",
			"GunRev/Resources/radioguy/radio_idle_right_002",
			//run
			"GunRev/Resources/radioguy/radio_run_left_001",
			"GunRev/Resources/radioguy/radio_run_left_002",
			"GunRev/Resources/radioguy/radio_run_left_003",
			"GunRev/Resources/radioguy/radio_run_left_004",
			"GunRev/Resources/radioguy/radio_run_left_005",
			"GunRev/Resources/radioguy/radio_run_left_006",
			"GunRev/Resources/radioguy/radio_run_right_001",
			"GunRev/Resources/radioguy/radio_run_right_002",
			"GunRev/Resources/radioguy/radio_run_right_003",
			"GunRev/Resources/radioguy/radio_run_right_004",
			"GunRev/Resources/radioguy/radio_run_right_005",
			"GunRev/Resources/radioguy/radio_run_right_006",
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