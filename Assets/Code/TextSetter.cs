using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSetter
    :
    MonoBehaviour
{
    void Start()
	{
        if( PlayerPrefs.GetInt( "has_jetpack",0 ) > 0 )
		{
			GetComponent<TextMesh>().text = "Hold space to use jetpack";
		}
	}
}
