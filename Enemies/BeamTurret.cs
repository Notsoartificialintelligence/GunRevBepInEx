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
	public class BeamTurret : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "fdd43f47902945bab28910c8c01b6ea5";
		public static GameObject shootpoint;
		private static tk2dSpriteCollectionData BeamTurretCollection;
		public static void Init()
		{
			BeamTurret.BuildPrefab();
		}
		public static void BuildPrefab()
		{

			if (prefab == null || !EnemyAPI.EnemyBuilder.Dictionary.ContainsKey(guid))
			{
				prefab = EnemyAPI.EnemyBuilder.BuildPrefab("Beam Turret", guid, "GunRev/Resources/beamturret/beamturret_idle_001", new IntVector2(0, 0), new IntVector2(0, 0), false);
				var enemy = prefab.AddComponent<EnemyBehavior>();
				enemy.aiActor.MovementSpeed = 0f;
				enemy.aiActor.knockbackDoer.weight = 0;
				enemy.aiActor.IgnoreForRoomClear = false;
				enemy.aiActor.CollisionDamage = 2f;
				enemy.aiActor.healthHaver.ForceSetCurrentHealth(30f);
				enemy.aiActor.healthHaver.SetHealthMaximum(40f, null, false);
				enemy.aiActor.PreventBlackPhantom = true;
				GameObject hand = enemy.transform.Find("GunAttachPoint").gameObject;
				Destroy(hand);

				AIAnimator aiAnimator = enemy.aiAnimator;

				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Flipped = new DirectionalAnimation.FlipType[1],
					AnimNames = new string[]
					{
						"idle",
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
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
					name = "tell",
					anim = new DirectionalAnimation
						{
							Type = DirectionalAnimation.DirectionType.Single,
							Flipped = new DirectionalAnimation.FlipType[1],
							AnimNames = new string[]
							{

								"tell",

							}

						}
					}
				};

				bool flag = BeamTurretCollection == null;
				if (flag)
				{
					BeamTurretCollection = SpriteBuilder.ConstructCollection(prefab, "Beamturret_Collection");
					UnityEngine.Object.DontDestroyOnLoad(BeamTurretCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], BeamTurretCollection);
					}
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, BeamTurretCollection, new List<int>
					{
						0,
					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 4f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, BeamTurretCollection, new List<int>
					{
						1,
						2,
						3,
						4,
						5,
					}, "die", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, BeamTurretCollection, new List<int>
					{
						6,
						7,
						8,
						9,
						10,
					}, "tell", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
				}
				shootpoint = new GameObject("beamturret top");
				shootpoint.transform.parent = enemy.transform;
				shootpoint.transform.position = enemy.sprite.WorldTopCenter;
				GameObject position = enemy.transform.Find("beamturret top").gameObject;
				var bs = prefab.GetComponent<BehaviorSpeculator>();
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
					ShootPoint = position,
					BulletScript = new CustomBulletScriptSelector(typeof(BeamTurretScript)),
					LeadAmount = 0f,
					AttackCooldown = 8f,
					ChargeAnimation = "tell",
					FireAnimation = "tell",
					RequiresLineOfSight = true,
					Uninterruptible = false,
				}
				};
				Game.Enemies.Add("ai:beamturret", enemy.aiActor);
				SpriteBuilder.AddSpriteToCollection("GunRev/Resources/beamturret/beamturret_idle_001", AmmonomiconController.ForceInstance.EncounterIconCollection);
				if (enemy.GetComponent<EncounterTrackable>() != null)
				{
					UnityEngine.Object.Destroy(enemy.GetComponent<EncounterTrackable>());
				}
				enemy.encounterTrackable = enemy.gameObject.AddComponent<EncounterTrackable>();
				enemy.encounterTrackable.journalData = new JournalEntry();
				enemy.encounterTrackable.EncounterGuid = "b80a0d3258fd49fe97748e02529af5c5";
				enemy.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
				enemy.encounterTrackable.journalData.SuppressKnownState = false;
				enemy.encounterTrackable.journalData.IsEnemy = true;
				enemy.encounterTrackable.journalData.SuppressInAmmonomicon = false;
				enemy.encounterTrackable.ProxyEncounterGuid = string.Empty;
				enemy.encounterTrackable.journalData.AmmonomiconSprite = "GunRev/Resources/beamturret/beamturret_idle_001";
				enemy.encounterTrackable.journalData.enemyPortraitSprite = Alexandria.ItemAPI.ResourceExtractor.GetTextureFromResource("GunRev\\Resources\\beamturretammonomicon.png");
				Module.Strings.Enemies.Set("#BEAMTURRET", "Beam Turret");
				Module.Strings.Enemies.Set("#BEAMTURRET_SHORTDESC", "Infrared");
				Module.Strings.Enemies.Set("#BEAMTURRET_LONGDESC", "Cold, unloving machines that dwell in the Black Powder Mines.\n\nBuilt by some madman with too much time on their hands, these killing machines have remarkable intricacy.");
				enemy.encounterTrackable.journalData.PrimaryDisplayName = "#BEAMTURRET";
				enemy.encounterTrackable.journalData.NotificationPanelDescription = "#BEAMTURRET_SHORTDESC";
				enemy.encounterTrackable.journalData.AmmonomiconFullEntry = "#BEAMTURRET_LONGDESC";
				EnemyAPI.EnemyBuilder.AddEnemyToDatabase(enemy.gameObject, enemy.aiActor.EnemyGuid);
				EnemyDatabase.GetEntry(enemy.aiActor.EnemyGuid).ForcedPositionInAmmonomicon = 74;
				EnemyDatabase.GetEntry(enemy.aiActor.EnemyGuid).isInBossTab = false;
				EnemyDatabase.GetEntry(enemy.aiActor.EnemyGuid).isNormalEnemy = true;
			}
		}

		private static string[] spritePaths = new string[]
{

			"GunRev/Resources/beamturret/beamturret_idle_001",

			"GunRev/Resources/beamturret/beamturret_die_001",
			"GunRev/Resources/beamturret/beamturret_die_002",
			"GunRev/Resources/beamturret/beamturret_die_003",
			"GunRev/Resources/beamturret/beamturret_die_004",
			"GunRev/Resources/beamturret/beamturret_die_005",

			"GunRev/Resources/beamturret/beamturret_tell_001",
			"GunRev/Resources/beamturret/beamturret_tell_002",
			"GunRev/Resources/beamturret/beamturret_tell_003",
			"GunRev/Resources/beamturret/beamturret_tell_004",
			"GunRev/Resources/beamturret/beamturret_tell_005",

};

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
				base.aiActor.healthHaver.OnPreDeath += (obj) => { AkSoundEngine.PostEvent("Play_OBJ_turret_fade_01", base.aiActor.gameObject); };
			}
		}
		public class BeamTurretScript : Script
		{
			public override IEnumerator Top()
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody) { base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").bulletBank.GetBullet("default")); }
				for (int i = 0; i < 180; i++)
				{
					base.Fire(new Direction(i * 2, Brave.BulletScript.DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), new BeamTurretBullet());
					yield return this.Wait(1);
				}
				yield break;
			}
		}
		public class BeamTurretBullet : Bullet
		{
			public BeamTurretBullet() : base("default", false, false, false)
			{
			}
		}
	}
}