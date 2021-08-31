/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionalSoldierMultiply : RegionalModifier
{
#region Fields
    public float multiplyCofactor;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
    protected override void OnDiceEnter()
    {
		targetDice.cofactor = multiplyCofactor;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
