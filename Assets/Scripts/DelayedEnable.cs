/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public class DelayedEnable : MonoBehaviour
{
#region Fields
    public EventListenerDelegateResponse levelStartListener;
    public float delayTime;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		levelStartListener.OnEnable();
	}

    private void OnDisable()
    {
		levelStartListener.OnDisable();
	}

    private void Awake()
    {
		levelStartListener.response = LevelStartResponse;
		DisableChilds();
	}
#endregion

#region API
#endregion

#region Implementation
    private void LevelStartResponse()
    {
		DOVirtual.DelayedCall( delayTime, ActivateChilds, false );
    }

    private void ActivateChilds()
    {
        for( var i = 0; i < transform.childCount; i++ )
        {
			transform.GetChild( i ).gameObject.SetActive( true );
		}
    }

    private void DisableChilds()
    {
        for( var i = 0; i < transform.childCount; i++ )
        {
			transform.GetChild( i ).gameObject.SetActive( false );
		}
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
