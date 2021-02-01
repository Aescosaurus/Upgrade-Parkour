﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory
	:
	MonoBehaviour
{
	void Start()
	{
		invPanel = GameObject.Find( "InventoryPanel" );

		for( int i = 0; i < invPanel.transform.childCount; ++i )
		{
			slots.Add( invPanel.transform.GetChild( i ).GetComponent<InventorySlot>() );
		}

		hotbar = GetComponent<HotbarHandler>();

		// var swordPrefab = Resources.Load<GameObject>( "Prefabs/BasicSword" );
		// slots[0].AddItem( swordPrefab );
		// slots[0].AddItem( Resources.Load<GameObject>( "Prefabs/MonsterShardSmall" ) );

		ToggleOpen( false );
	}

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.Tab ) )
		{
			ToggleOpen( !open );
		}
	}

	void ToggleOpen( bool on )
	{
		open = on;
		invPanel.SetActive( on );
		Cursor.visible = on;
		Cursor.lockState = on ? CursorLockMode.None : CursorLockMode.Locked;
	}

	// true if success false if full
	public bool AddItem( LoadableItem item )
	{
		bool full = true;

		// try stacking item in hotbar, then inventory, before creating new stack
		if( hotbar.TryStackItem( item ) || TryStackItem( item ) )
		{
			full = false;
		}

		if( full && hotbar.TryAddItem( item ) ) full = false;

		if( full )
		{
			foreach( var slot in slots )
			{
				if( slot.TrySetItem( item ) )
				{
					full = false;
					break;
				}
			}
		}

		return( !full );
	}

	bool TryStackItem( LoadableItem item )
	{
		foreach( var slot in slots )
		{
			// if( slot.GetItem() == item )
			if( slot.TrySetItem( item ) )
			{
				// slot.TrySetItem( item );
				return( true );
			}
		}

		return( false );
	}

	public bool IsOpen()
	{
		return( open );
	}

	GameObject invPanel;
	GameObject invSlotPrefab;

	List<InventorySlot> slots = new List<InventorySlot>();

	HotbarHandler hotbar;

	bool open = false;
}
