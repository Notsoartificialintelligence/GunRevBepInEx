using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.ObjectModel;
using System.Collections;
using GunRev;
using Brave.BulletScript;
using Dungeonator;

public class OmegearController : BraveBehaviour
{
	public void Start()
	{
		base.aiActor.HasBeenEngaged = false;
		m_StartRoom = aiActor.GetAbsoluteParentRoom();
		base.healthHaver.OnPreDeath += this.OnPreDeath;
	}
	private RoomHandler m_StartRoom;
	private void Update()
	{
		if (!base.aiActor.HasBeenEngaged)
		{
			CheckPlayerRoom();
		}
	}
	private void CheckPlayerRoom()
	{
		if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom)
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
	public override void OnDestroy()
	{
		if (base.healthHaver)
		{
			base.healthHaver.OnPreDeath -= this.OnPreDeath;
		}
		base.OnDestroy();
	}
	private void OnPreDeath(Vector2 obj)
	{

	}
}