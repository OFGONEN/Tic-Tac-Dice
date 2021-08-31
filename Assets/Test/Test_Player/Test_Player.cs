/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class Test_Player : MonoBehaviour
{
#region Fields
    public GameEvent levelStartEvent;
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {
		levelStartEvent.Raise();
	}
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
