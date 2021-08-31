/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public abstract class RegionalModifier : MonoBehaviour
{
#region Fields
    [ Header( "EventListeners" ) ]
	public EventListenerDelegateResponse allyDiceEventListener;
	public EventListenerDelegateResponse enemyDiceEventListener;
    
    [ Header( "Fired Events" ) ]
    public ParticleSpawnEvent particleSpawnEvent;

    // Private Fields \\
    private CapsuleCollider capsuleCollider;

    protected Dice targetDice;
    protected int targetDice_ID;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		allyDiceEventListener.OnEnable();
        enemyDiceEventListener.OnEnable();
	}

    private void OnDisable()
    {
        allyDiceEventListener.OnDisable();
        enemyDiceEventListener.OnDisable();
	}

    private void Awake()
    {
		allyDiceEventListener.response  = AllyDiceEventResponse;
		enemyDiceEventListener.response = EnemyDiceEventResponse;

		capsuleCollider = GetComponent< CapsuleCollider >();
	}

    protected virtual void OnTriggerEnter( Collider other )
    {
		if( targetDice == null )
        {
			targetDice    = other.GetComponentInParent< Dice >();
			targetDice_ID = targetDice.GetInstanceID();
			OnDiceEnter();
		}
	}

    protected virtual void OnTriggerExit( Collider other )
    {
        if( targetDice != null )
        {
			var dice = other.GetComponentInParent< Dice >();
			var dice_ID = dice.GetInstanceID();

			if( dice_ID == targetDice_ID )
            {
				targetDice.DefaultModifiers();

				targetDice = null;
				targetDice_ID = -1;

				var diceLayerMask = LayerMask.GetMask( "AllyDice", "EnemyDice" );
				var colliders     = capsuleCollider.CapsuleCast( diceLayerMask );

                for( var i = 0; i < colliders.Length; i++ )
                {
					var otherDice = colliders[ i ].GetComponentInParent< Dice >();
					var otherDice_ID = otherDice.GetInstanceID();

					if( dice_ID != otherDice_ID )
                    {
						targetDice    = otherDice;
						targetDice_ID = otherDice_ID;
					}
                }
			}
		}
    }
#endregion

#region API
#endregion

#region Implementation
    protected abstract void OnDiceEnter();

    private void AllyDiceEventResponse()
    {
		var diceEvent = allyDiceEventListener.gameEvent as DiceEvent;
		DiceEventResponse( diceEvent );
	}
    
    private void EnemyDiceEventResponse()
    {
		var diceEvent = enemyDiceEventListener.gameEvent as DiceEvent;
		DiceEventResponse( diceEvent );
    }

    private void DiceEventResponse( DiceEvent diceEvent )
    {
        if( targetDice != null && diceEvent.diceID == targetDice_ID )
        {
			// Raise modifier particle event
			particleSpawnEvent.changePosition = true;
			particleSpawnEvent.spawnPoint = transform.position;
			particleSpawnEvent.particleAlias = "modifier_buff";
			particleSpawnEvent.Raise();

			gameObject.SetActive( false );
		}
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
