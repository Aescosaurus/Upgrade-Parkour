﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiffyInput
	:
	MonoBehaviour
{
	public static bool CheckAxis( string axis,bool ignorePause = false )
	{
		if( !ignorePause && PauseMenu.IsOpen() ) return( false );

		if( !canPress.ContainsKey( axis ) ) canPress.Add( axis,false );

		bool pressing = Input.GetAxis( axis ) > 0.0f;

		if( canPress[axis] )
		{
			if( pressing )
			{
				canPress[axis] = false;
				return( true );
			}
			else return( false );
		}
		else
		{
			if( !pressing ) canPress[axis] = true;
			return( false );
		}
	}

	static Dictionary<string,bool> canPress = new Dictionary<string,bool>();
}
