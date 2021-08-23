/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class RegionalSoldierEnhance : RegionalModifier
{
#region Fields
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
		targetDice.soldierType = SoldierType.Super;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
