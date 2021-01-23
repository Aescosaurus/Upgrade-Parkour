﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamCtrl
	:
	MonoBehaviour
{
	void Start()
	{
		player = GameObject.Find( "Player" );

		distToPlayer = ( minDistToPlayer + maxDistToPlayer ) / 2.0f;

		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.Escape ) )
		{
			Cursor.lockState = CursorLockMode.None;
			escape = true;
		}
		if( Input.GetMouseButtonDown( 0 ) )
		{
			Cursor.lockState = CursorLockMode.Locked;
			escape = false;
		}

		if( !escape )
		{
			var aim = new Vector2( Input.GetAxis( "Mouse X" ),
				Input.GetAxis( "Mouse Y" ) );

			if( aim.y > maxAimMove ) aim.y = maxAimMove;
			if( aim.y < -maxAimMove ) aim.y = -maxAimMove;

			var tempAng = transform.eulerAngles;
			tempAng.x = tempAng.x - aim.y * rotationSpeed * Time.deltaTime;
			if( tempAng.x > 90.0f - verticalCutoff && tempAng.x < 180.0f ) tempAng.x = 90.0f - verticalCutoff;
			if( tempAng.x < 270.0f + verticalCutoff && tempAng.x > 180.0f ) tempAng.x = 270.0f + verticalCutoff;
			tempAng.y = tempAng.y + aim.x * rotationSpeed * Time.deltaTime;
			transform.eulerAngles = tempAng;

			distToPlayer -= Input.GetAxis( "Mouse ScrollWheel" ) *
				scrollSpeed * Time.deltaTime;

			distToPlayer = Mathf.Max( minDistToPlayer,distToPlayer );
			distToPlayer = Mathf.Min( maxDistToPlayer,distToPlayer );
		}

		transform.position = player.transform.position +
			transform.right * offset.x + transform.up * offset.y + transform.forward * offset.z;
		transform.position -= transform.forward * distToPlayer;
	}

	[SerializeField] float minDistToPlayer = 4.0f;
	[SerializeField] float maxDistToPlayer = 6.0f;
	[SerializeField] Vector3 offset = Vector3.zero;
	float distToPlayer;

	[SerializeField] float rotationSpeed = 100.0f;
	[SerializeField] float scrollSpeed = 50.0f;

	[SerializeField] float verticalCutoff = 10.0f;
	const float maxAimMove = 90.0f - 1.0f;

	GameObject player;

	bool escape = false;
}