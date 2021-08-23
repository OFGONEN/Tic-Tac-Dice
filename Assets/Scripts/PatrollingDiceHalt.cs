/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingDiceHalt : MonoBehaviour
{
#region Fields
#endregion

#region Properties
#endregion

#region Unity API
    private void OnTriggerEnter( Collider other )
    {
		var dice = other.GetComponentInParent< Dice >();
		dice.Halt();

        //TODO(ofg): Spawn a particle effect
		gameObject.SetActive( false );
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
