﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase
	:
	MonoBehaviour
{
	protected virtual void Start()
	{
		// animCtrl = FindObjectOfType<PlayerWalk>().GetComponent<Animator>();
		team = animCtrl?.GetComponent<EnemyBase>() != null ? 2 : 1;

		refire.Update( refire.GetDuration() );
	}

	protected virtual void Update()
	{
		// todo enemy attack ai
		// if( refire.Update( Time.deltaTime ) &&
		// 	( team == 2 || Input.GetAxis( "Fire1" ) > 0.0f ) )
		// {
		// 	Fire();
		// 	animCtrl.SetFloat( "shot_spd",1.0f / refire.GetDuration() );
		// 	refire.Reset();
		// }
		refire.Update( Time.deltaTime );
	}

	protected abstract void Fire();

	public void LinkAnimator( Animator animCtrl )
	{
		this.animCtrl = animCtrl;
	}

	public bool TryPerformAttack()
	{
		bool done = refire.IsDone();

		if( done )
		{
			animCtrl.SetFloat( "shot_spd",1.0f / refire.GetDuration() );
			Fire();
			refire.Reset();
		}

		return( done );
	}

	public virtual void ToggleAttacking( bool on )
	{
		attacking = on;
	}

	public virtual void CancelAttack()
	{
		attacking = false;
	}

	public virtual int GetPreferredHand()
	{
		return( 1 );
	}

	public bool IsAttacking()
	{
		return( attacking );
	}

	protected Animator animCtrl;

	[SerializeField] protected Timer refire = new Timer( 0.5f );

	protected int team = -1; // 1 = player 2 = enemy

	protected bool attacking = false;
}