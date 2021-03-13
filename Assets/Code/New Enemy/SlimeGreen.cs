﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGreen
    :
    SlimeBase
{
	protected override void Start()
	{
		base.Start();

		Transition( "idle","hop" );
	}

	public override void HopStart()
	{
		base.HopStart();

		Vector3 hopDir;

		if( activated ) hopDir = player.transform.position - transform.position;
		else
		{
			hopDir = new Vector3(
				Random.Range( -1.0f,1.0f ),
				0.0f,
				Random.Range( -1.0f,1.0f ) );
		}

		Hop( hopDir );
	}

	public override void HopEnd()
	{
		base.HopEnd();

		Transition( "hop","idle" );
		StartCoroutine( JumpRest() );
	}

	// protected override void Activate()
	// {
	// 	Transition( "idle","hop" );
	// }

	IEnumerator JumpRest()
	{
		yield return( new WaitForSeconds( restDuration ) );

		Transition( "idle","hop" );
	}

	[SerializeField] float restDuration = 1.0f;
}
