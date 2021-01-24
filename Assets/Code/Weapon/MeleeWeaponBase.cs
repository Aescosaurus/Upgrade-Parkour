﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MeleeWeaponBase
	:
	WeaponBase
{
	protected override void Start()
	{
		base.Start();

		hurtArea = GetComponent<Collider>();
		Assert.IsTrue( hurtArea.isTrigger );
	}

	protected override void Update()
	{
		base.Update();

		if( refire.IsDone() )
		{
			hurtArea.enabled = false;
			animCtrl.SetBool( "swing",false );
		}
	}

	protected override void Fire()
	{
		// StopCoroutine( HandleAttack( 0.0f ) );
		// StartCoroutine( HandleAttack( refire.GetDuration() ) );

		animCtrl.SetBool( "swing",true );
		hurtArea.enabled = true;
	}

	IEnumerator HandleAttack( float s )
	{
		animCtrl.SetBool( "swing",true );
		hurtArea.enabled = true;
		yield return( new WaitForSeconds( s ) );
		hurtArea.enabled = false;
		animCtrl.SetBool( "swing",false );
	}

	void OnTriggerEnter( Collider coll )
	{
		var enemyScr = coll.GetComponent<EnemyBase>();
		var playerScr = coll.GetComponent<PlayerWalk>();
		if( team == 1 && enemyScr != null )
		{
			enemyScr.Damage( damage );
		}
		else if( playerScr != null )
		{
			// damage player
		}
	}

	Collider hurtArea;

	[SerializeField] float damage = 1.0f;
}
