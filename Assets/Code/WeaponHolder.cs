﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder
	:
	MonoBehaviour
{
	void Awake()
	{
		bh = GetComponent<BipedHandler>();
		animCtrl = GetComponent<Animator>();
		if( heldWeapon != null && bh != null )
		{
			InitNewWeapon( heldWeapon );
		}
	}

	void Update()
	{
		if( curWB.IsAttacking() )
		{
			SetRot();
		}
	}

	public void TryAttack( float aimDir )
	{
		if( curWB != null )
		{
			curWB.TryPerformAttack();
		}

		storedRot = aimDir;
		SetRot();
	}

	public void CancelAttack()
	{
		curWB.CancelAttack();
	}

	// int cuz anim events cant handle bool?
	// public void TryToggleMeleeHurtArea( int on )
	// {
	// 	meleeWB?.ToggleHurtArea( on > 0 );
	// }

	public void ToggleAttacking( int on )
	{
		curWB.ToggleAttacking( on == 1 );
	}

	void SetRot()
	{
		var rot = transform.eulerAngles;
		// rot.y = Mathf.Atan2( xMove,yMove ) * Mathf.Rad2Deg;
		// rot.y = Mathf.LerpAngle( transform.eulerAngles.y,rot.y,rotSpeed * Time.deltaTime );
		rot.y = storedRot;
		transform.eulerAngles = rot;
	}

	public void SetTargetDir( float angle )
	{
		storedRot = angle;
	}

	public void ReplaceWeapon( GameObject replacement )
	{
		curWB.CancelAttack();
		Destroy( curWB.gameObject );
		InitNewWeapon( replacement );
		animCtrl.SetBool( "aim",false );
		animCtrl.SetBool( "swing",false );
	}

	void InitNewWeapon( GameObject prefab )
	{
		var curWeapon = Instantiate( prefab );
		curWB = curWeapon.GetComponent<WeaponBase>();
		// meleeWB = curWeapon.GetComponent<MeleeWeaponBase>();

		var handPref = curWB.GetPreferredHand();
		// curWeapon.transform.parent = bh.GetHand( handPref ).transform;
		curWeapon.transform.SetParent( bh.GetHand( handPref ).transform,false );
		curWeapon.transform.Rotate( Vector3.right,90.0f * ( handPref == 1 ? 1 : -1 ) );
		curWB.LinkAnimator( animCtrl );
	}

	[SerializeField] GameObject heldWeapon = null;

	// GameObject curWeapon = null;
	WeaponBase curWB = null;
	// MeleeWeaponBase meleeWB = null;
	BipedHandler bh;
	Animator animCtrl;

	float storedRot = 0.0f;
}