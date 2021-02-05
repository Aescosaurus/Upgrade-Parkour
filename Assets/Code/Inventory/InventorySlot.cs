﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class InventorySlot
	:
	MonoBehaviour,
	IBeginDragHandler,
	IEndDragHandler,
	IDragHandler
{
	void Awake()
	{
		itemPos = transform.Find( "ItemPos" );

		uiLayer = LayerMask.NameToLayer( "UI" );

		rect = GetComponent<RectTransform>();
		img = GetComponent<Image>();

		ToggleActivation( false );

		item = GetComponent<LoadableItem>();

		counterText = GetComponentInChildren<Text>();
		UpdateCounter();
	}

	void Start()
	{
		hotbar = FindObjectOfType<HotbarHandler>();
	}

	// void OnDestroy()
	// {
	// 	string idStr = "";
	// 	foreach( var c in gameObject.name )
	// 	{
	// 		if( char.IsNumber( c ) ) idStr += c;
	// 	}
	// 	int id = ( idStr.Length > 0 ? int.Parse( idStr ) : 0 );
	// 	
	// 	var storage = transform.parent.name;
	// 	
	// 	print( storage + " " + id );
	// }

	public void AddItem( LoadableItem item )
	{
		++nItems;

		Assert.IsTrue( this.item != null );

		if( this.item.GetPrefab() == null )
		{
			// heldPrefab = prefab;
			// heldItem = Instantiate( prefab.transform.GetChild( 0 ).gameObject,itemPos );

			// this.item = item;
			// this.item = GetComponent<LoadableItem>();
			this.item.Copy( item );
			heldModel = Instantiate( item.GetPrefab().transform.GetChild( 0 ).gameObject,itemPos );

			heldModel.transform.localPosition = new Vector3( -16.6f,-16.7f,-0.8f );
			heldModel.transform.localScale *= itemScaleFactor;
			heldModel.transform.localEulerAngles = new Vector3( 39.2f,70.5f,-12.6f );

			var meshRend = heldModel.transform.GetComponentInChildren<MeshRenderer>();
			meshRend.gameObject.layer = uiLayer;
			meshRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		}
		else
		{
			UpdateCounter();
		}
	}

	public void RemoveItem()
	{
		--nItems;
		UpdateCounter();

		if( nItems <= 0 )
		{
			// heldPrefab = null;
			// Destroy( heldItem );

			// item = null;
			item.Clear();
			Destroy( heldModel );
			heldModel = null;
		}
	}

	public void OnBeginDrag( PointerEventData eventData )
	{
		if( item.GetPrefab() != null )
		{
			home = rect.localPosition;
			dragOffset = eventData.pointerCurrentRaycast.worldPosition - transform.position;
			img.raycastTarget = false;
		}
	}

	public void OnEndDrag( PointerEventData eventData )
	{
		if( item.GetPrefab() != null )
		{
			// eventData.pointerEnter
			RectTransform otherItem = eventData.pointerEnter?.GetComponent<RectTransform>();
			var invSlot = otherItem?.GetComponent<InventorySlot>();
			if( otherItem != null && invSlot != null )
			{
				// transform.position = otherItem.position;
				// otherItem.position = home;
				// invSlot.home = home;
				// home = rect.position;
				TransferItem( invSlot );
			}
			// else
			{
				// transform.position = home;
				rect.localPosition = home;
			}

			img.raycastTarget = true;
		}
	}

	public void OnDrag( PointerEventData eventData )
	{
		if( item.GetPrefab() != null )
		{
			transform.position = eventData.pointerCurrentRaycast.worldPosition - dragOffset;
		}
	}

	// more like swap item
	public void TransferItem( InventorySlot receiver )
	{
		heldModel.transform.SetParent( receiver.itemPos,false );
		receiver.heldModel?.transform.SetParent( itemPos,false );

		var tempHeldModel = heldModel;
		heldModel = receiver.heldModel;
		receiver.heldModel = tempHeldModel;
		// heldItem = null;

		// var tempHeldPrefab = heldPrefab;
		// heldPrefab = receiver.heldPrefab;
		// receiver.heldPrefab = tempHeldPrefab;
		// heldPrefab = null;

		// var tempItem = item;
		// item = receiver.item;
		// receiver.item = tempItem;
		item.Swap( receiver.item );

		var tempNItems = nItems;
		nItems = receiver.nItems;
		receiver.nItems = tempNItems;
		UpdateCounter();
		receiver.UpdateCounter();

		hotbar.RefreshSlot();
	}

	public void ToggleActivation( bool on )
	{
		var c = img.color;
		c.a = on ? 1.0f : defaultAlpha;
		img.color = c;
	}

	// return true if success in setting item, false if already full
	public bool TrySetItem( LoadableItem item )
	{
		Assert.IsTrue( item != null );
		bool canSwap = true;
		// if( heldItem != null ) print( prefab.name + " " + heldPrefab.name );

		// weps are not stackable
		// if( this.item != null &&
		// 	( !item.CheckEqual( this.item ) ||
		// 	( item.GetPrefab().GetComponent<WeaponBase>() != null ||
		// 	this.item.GetPrefab().GetComponent<WeaponBase>() != null ) ) )
		// {
		// 	return( false );
		// }
		if( this.item.GetPrefab() != null )
		{
			// return( !item.CheckEqual( this.item ) ||
			// 	item.GetPrefab().GetComponent<WeaponBase>() != null ||
			// 	this.item.GetPrefab().GetComponent<WeaponBase>() != null );
			if( !item.CheckEqual( this.item ) ||
				item.GetPrefab().GetComponent<WeaponBase>() != null ||
				this.item.GetPrefab().GetComponent<WeaponBase>() != null )
			{
				canSwap = false;
			}
		}

		// print( item.GetPrefab().GetComponent<WeaponBase>() );
		// print( this.item );

		if( canSwap ) AddItem( item );

		return( canSwap );
	}

	void UpdateCounter()
	{
		counterText.text = "";
		if( nItems > 1 ) counterText.text = nItems.ToString();
	}

	public GameObject GetPrefab()
	{
		return( item?.GetPrefab() );
	}

	public LoadableItem GetItem()
	{
		return( item );
	}

	public int CountItems()
	{
		return( nItems );
	}

	Transform itemPos;
	// GameObject heldItem = null;
	// [SerializeField] GameObject heldPrefab = null;
	[SerializeField] LoadableItem item = null;
	GameObject heldModel = null;

	RectTransform rect;

	[SerializeField] float itemScaleFactor = 50.0f;
	[Range( 0.0f,1.0f )]
	[SerializeField] float defaultAlpha = 0.5f;

	int uiLayer;

	Vector3 dragOffset = Vector3.zero;
	Vector3 home;

	Image img;

	HotbarHandler hotbar;

	Text counterText;
	int nItems = 0;
}