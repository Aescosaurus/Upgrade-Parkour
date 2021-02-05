﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VendorUIBase
	:
	MonoBehaviour
{
	public virtual void CloseUI()
	{
		vendor.GetComponent<NPCVendor>().CloseUI();
	}

	protected void DropItems( InventorySlot slot )
	{
		for( int i = 0; i < slot.CountItems(); ++i )
		{
			var item = Instantiate( slot.GetPrefab() );
			item.transform.position = vendor.transform.position + vendor.transform.forward;
			slot.RemoveItem();
		}
	}

	public void SetVendor( GameObject vendor )
	{
		this.vendor = vendor;
	}

	GameObject vendor = null;
}