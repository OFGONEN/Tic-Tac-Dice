/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class PatrollingDiceHalt : MonoBehaviour
{
#region Fields
    public ParticleSpawnEvent particleSpawnEvent;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnTriggerEnter( Collider other )
    {
		var dice = other.GetComponentInParent< Dice >();
		dice.Halt();

		// Raise modifier particle event
		particleSpawnEvent.changePosition = true;
		particleSpawnEvent.spawnPoint     = transform.position;
		particleSpawnEvent.particleAlias  = "modifier_debuff";
		particleSpawnEvent.Raise();

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
