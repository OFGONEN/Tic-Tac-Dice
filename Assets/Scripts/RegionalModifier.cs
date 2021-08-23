/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public abstract class RegionalModifier : MonoBehaviour
{
#region Fields
    [ Header( "EventListeners" ) ]
	public EventListenerDelegateResponse allyDiceEventListener;
	public EventListenerDelegateResponse enemyDiceEventListener;

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
            
            if( dice.GetInstanceID() == targetDice_ID )
            {
				targetDice.DefaultModifiers();
				targetDice = null;
				targetDice_ID = -1;

				var diceLayerMask = LayerMask.GetMask( "AllyDice", "EnemyDice" );
				var colliders     = capsuleCollider.CapsuleCast( diceLayerMask );

				if( colliders.Length > 0 && colliders[ 0 ] != null )
                {
					targetDice    = colliders[ 0 ].GetComponentInParent< Dice >();
					targetDice_ID = targetDice.GetInstanceID();
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
			gameObject.SetActive( false );
		}
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
