/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DelayedEnable : MonoBehaviour
{
#region Fields
    public float delayTime;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		DOVirtual.DelayedCall( delayTime, ActivateChilds );
	}
#endregion

#region API
#endregion

#region Implementation
    private void ActivateChilds()
    {
        for( var i = 0; i < transform.childCount; i++ )
        {
			transform.GetChild( i ).gameObject.SetActive( true );
		}
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
