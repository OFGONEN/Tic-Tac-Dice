/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class CooldownMultiplier : MonoBehaviour
{
#region Fields
    [ Header( "Fired Events" ) ]
    public FloatGameEvent allyCooldownEvent;
    public FloatGameEvent enemyCooldownEvent;

    [ HorizontalLine ]
    public float cooldown;
#endregion

#region Properties
#endregion

#region Unity API

    private void OnTriggerEnter( Collider other )
    {
		var dice = other.GetComponentInParent< Dice >();

        if( dice.party == Parties.Ally )
        {
			allyCooldownEvent.eventValue = cooldown;
            allyCooldownEvent.Raise();
        }
        else if( dice.party == Parties.Enemy)
        {
			enemyCooldownEvent.eventValue = cooldown;
			enemyCooldownEvent.Raise();
		}

		//TODO(ofg): Call a particle effect
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
