﻿using System;
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
	public class Servont : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "b37bbc3f456b465a8822fc024b74ee73";
		public static GameObject shootpoint;
		private static tk2dSpriteCollectionData ServoCollection;
		public static void Init()
		{
			Servont.BuildPrefab();
		}
		public static void BuildPrefab()
		{

			if (prefab == null || !EnemyAPI.EnemyBuilder.Dictionary.ContainsKey(guid))
			{
				prefab = EnemyAPI.EnemyBuilder.BuildPrefab("Servont", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(0, 0), false);
				var enemy = prefab.AddComponent<EnemyBehavior>();
				enemy.aiActor.MovementSpeed = 1f;
				enemy.aiActor.knockbackDoer.weight = 16f;
				enemy.aiActor.IgnoreForRoomClear = false;
				enemy.aiActor.CollisionDamage = 2f;
				enemy.aiActor.healthHaver.ForceSetCurrentHealth(15f);
				enemy.aiActor.healthHaver.SetHealthMaximum(20f, null, false);
				enemy.aiActor.PreventBlackPhantom = false;
				enemy.aiActor.PreventFallingInPitsEver = true;
				enemy.aiActor.IgnoreForRoomClear = true;
				enemy.gameObject.AddComponent<KillOnRoomClear>();
				GameObject hand = enemy.transform.Find("GunAttachPoint").gameObject;
				Destroy(hand);

				AIAnimator aiAnimator = enemy.aiAnimator;
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Prefix = "idle",
					Type = DirectionalAnimation.DirectionType.Single,
					Flipped = new DirectionalAnimation.FlipType[1],
					AnimNames = new string[]
					{
						"idle"
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

				bool flag = ServoCollection == null;
				if (flag)
				{
					ServoCollection = SpriteBuilder.ConstructCollection(prefab, "Servo_Collection");
					UnityEngine.Object.DontDestroyOnLoad(ServoCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], ServoCollection);
					}
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, ServoCollection, new List<int>
					{
						0,
						1,
						2,
						3,
						4,
						5
					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 6f;
					SpriteBuilder.AddAnimation(enemy.spriteAnimator, ServoCollection, new List<int>
					{
						6
					}, "die", tk2dSpriteAnimationClip.WrapMode.Once).fps = 6f;
				}
				shootpoint = new GameObject("servocentre");
				shootpoint.transform.parent = enemy.transform;
				shootpoint.transform.position = enemy.sprite.WorldCenter;
				GameObject position = enemy.transform.Find("servocentre").gameObject;
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
					BulletScript = new CustomBulletScriptSelector(typeof(ServontAttack)),
					LeadAmount = 0f,
					AttackCooldown = 4f,
					FireAnimation = "idle",
					RequiresLineOfSight = true,
					Uninterruptible = false,
				}
				};
				bs.MovementBehaviors = new List<MovementBehaviorBase>()
                {
					new SeekTargetBehavior
				{
					StopWhenInRange = false,
					CustomRange = 15f,
					LineOfSight = false,
					ReturnToSpawn = false,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0f,
					MaxActiveRange = 0f
				}
			};
				Game.Enemies.Add("ai:servont", enemy.aiActor);
			}
		}

		private static string[] spritePaths = new string[]
		{

			"GunRev/Resources/servont/servont_idle_001",
			"GunRev/Resources/servont/servont_idle_002",
			"GunRev/Resources/servont/servont_idle_003",
			"GunRev/Resources/servont/servont_idle_004",
			"GunRev/Resources/servont/servont_idle_005",
			"GunRev/Resources/servont/servont_idle_006",

			"GunRev/Resources/servont/servont_die_001"

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
				base.aiActor.healthHaver.OnPreDeath += (obj) => { Exploder.Explode(this.aiActor.Position, GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultExplosionData, this.aiActor.Position); };
			}
			
		}
		public class ServontAttack : Script
		{
			public override IEnumerator Top()
			{
				hasSpawnedAllProjectiles = false;
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody) { base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("cd4a4b7f612a4ba9a720b9f97c52f38c").bulletBank.GetBullet("default")); }
				for (int l = 0; l < 2; l++)
				{
					for (int i = 0; i < 4; i++)
					{
						base.Fire(new Offset(3, 0, i * 90f + (l * 45)), new Direction(i * 90f + (l * 45)), new Speed(0f, SpeedType.Absolute), new ServontBullet(this, "default"));
						yield return this.Wait(4);
					}
				}
				hasSpawnedAllProjectiles = true;
				yield break;
			}
			public bool hasSpawnedAllProjectiles;
		}
		public class ServontBullet : Bullet
		{
			public ServontBullet(ServontAttack parent, string BulletName) : base(BulletName, false, false, false)
			{
				this.m_parent = parent;
			}
			public override IEnumerator Top()
            {
                while (true)
                {
					if (m_parent.hasSpawnedAllProjectiles)
					{
						base.ChangeSpeed(new Speed(12f, SpeedType.Absolute));
						yield break;
					}
					else if (!this.IsOwnerAlive)
                    {
						BulletManager.DestroyBullet(this, false);
						yield break;
                    }
					else
                    {
						yield return this.Wait(1);
                    }
                }
            }
            private ServontAttack m_parent;
		}
	}
}