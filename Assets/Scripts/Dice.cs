/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;
using DG.Tweening;

public class Dice : MonoBehaviour
{
#region Fields
	[ Header( "Fired Events" ) ]
	public DiceEvent diceEvent;
	public ParticleSpawnEvent diceDisappearParticleEvent;

	[ Header( "Shared Variables" ) ]
	public DicePool dicePool;

	[ HorizontalLine ]
	public Parties party;

	// Private Fields
	private Rigidbody dice_Rigidbody;

	// Delegates
	private UnityMessage fixedUpdateDelegate;
#endregion

#region Unity API
	private void OnDisable()
	{
		dicePool.Stack.Push( this ); // Return to stack when disabled.
		transform.SetParent( dicePool.mainParent ); // Return to main parent as a child.
	}

	private void Awake()
	{
		/* Initialization */
		// Components
		dice_Rigidbody = GetComponent< Rigidbody >();

		// Delegates
		fixedUpdateDelegate = ExtensionMethods.EmptyMethod;

	}

	private void FixedUpdate() 
	{
		fixedUpdateDelegate();
	}
#endregion

#region API
	public void Spawn( Vector3 position, Quaternion rotation )
	{
		transform.position = position;
		transform.rotation = rotation;

		// Don't use gravity after spawn
		dice_Rigidbody.useGravity = false; 
	}

	public void Launch( Vector3 launchForce )
	{
		transform.SetParent( null ); // Null parent

		dice_Rigidbody.useGravity = true; // Use gravity

		dice_Rigidbody.AddForce( launchForce, ForceMode.Impulse ); // Give IMPULSE FORCE
		dice_Rigidbody.AddTorque( Random.insideUnitSphere, ForceMode.Impulse ); // Give RANDOM IMPULSE TORQUE

		fixedUpdateDelegate = RigidbodyCheck; // Check Rigidbodies status after launch
	}
	
#endregion

#region Implementation
	private void RigidbodyCheck()
	{
		if( dice_Rigidbody.IsSleeping() ) // If rigidbody is stopped and changed its status to SLEEP
		{
			DOVirtual.DelayedCall( GameSettings.Instance.dice_waitTimeAfterSleep, OnRigidbodySleep ); // Delayed call 
		}
	}

	private void OnRigidbodySleep()
	{
		int diceNumber;

		var isFacingUp = CheckIfFaceUpSide( out diceNumber );

		if( isFacingUp )
		{
			fixedUpdateDelegate = ExtensionMethods.EmptyMethod;

			// Dice Event: Position, Ally or Enemy, Dice number
			diceEvent.position   = transform.position;
			diceEvent.diceNumber = diceNumber;
			diceEvent.party      = party;

			diceEvent.Raise();

			// Dice Disappear Particle Effect 
			diceDisappearParticleEvent.changePosition = true;
			diceDisappearParticleEvent.spawnPoint     = transform.position;
			diceDisappearParticleEvent.particleAlias  = "dice_disappear";

			diceDisappearParticleEvent.Raise();

		}
		else
		{
			dice_Rigidbody.AddForce( Vector3.up * GameSettings.Instance.dice_correctionForceMagnitute, ForceMode.Impulse );
		}
	}

	// Up: 1, Right: 3, Forward: 5
	private bool CheckIfFaceUpSide( out int diceNumber )
	{
		     diceNumber = 0;      // Default value 
		bool isFaceUp   = false;  // At least one face of the dice is facing upwards

		var dotProduct_up     = Vector3.Dot( transform.up, Vector3.up );
		var dotProduct_foward = Vector3.Dot( transform.forward, Vector3.up );
		var dotProduct_right  = Vector3.Dot( transform.right, Vector3.up );

		// If dot product is 1 or -1 means that axis facing up or down
		var facing_up     = Mathf.Approximately( 1, Mathf.Abs( dotProduct_up ) );
		var facing_foward = Mathf.Approximately( 1, Mathf.Abs( dotProduct_foward ) );
		var facing_right  = Mathf.Approximately( 1, Mathf.Abs( dotProduct_right ) );

		if( facing_up )
		{
			diceNumber = Mathf.Sign( dotProduct_up ) == 1 ? 1 : 6;
			isFaceUp   = true;
		}
		else if( facing_foward )
		{
			diceNumber = Mathf.Sign( dotProduct_foward ) == 1 ? 5 : 2;
			isFaceUp   = true;
		}
		else if( facing_right )
		{
			diceNumber = Mathf.Sign( dotProduct_right ) == 1 ? 3 : 3;
			isFaceUp   = true;
		}

		return isFaceUp;
	}
#endregion
}
