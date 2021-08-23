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

	public SoldierType soldierType;
	public float cofactor;
	public UnityMessage collisionEnter;

	// Private Fields
	private Rigidbody dice_Rigidbody;

	private int[] diceNumbers = new int[] { 1, 6, 3, 4, 5, 2};
	private float[] dotProducts = new float[ 6 ];

	// Delegates
	private UnityMessage fixedUpdateDelegate;
#endregion

#region Unity API
	private void Awake()
	{
		/* Initialization */
		// Components
		dice_Rigidbody = GetComponent< Rigidbody >();

		// Delegates
		fixedUpdateDelegate = ExtensionMethods.EmptyMethod;
		collisionEnter      = ExtensionMethods.EmptyMethod;
	}

	private void FixedUpdate() 
	{
		fixedUpdateDelegate();
	}

	private void OnCollisionEnter(Collision other) 
	{
		collisionEnter();
	}
#endregion

#region API
	public void Halt()
	{
		dice_Rigidbody.velocity        = Vector3.zero;
		dice_Rigidbody.angularVelocity = Vector3.zero;
	}

	public void Spawn( Vector3 localPosition, Quaternion localRotation )
	{
		gameObject.SetActive( true );

		transform.localPosition = localPosition;
		transform.localRotation = localRotation;

		soldierType = SoldierType.Normal;
		cofactor    = 1f;

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

	// Defaults the dice attributes.
	public void ReturnDefault()
	{
		dice_Rigidbody.useGravity = false;

		dice_Rigidbody.velocity        = Vector3.zero;
		dice_Rigidbody.angularVelocity = Vector3.zero;

		fixedUpdateDelegate = ExtensionMethods.EmptyMethod;
		collisionEnter      = ExtensionMethods.EmptyMethod;

		gameObject.SetActive( false ); // Disable the dice
		dicePool.Stack.Push( this ); // Return to stack after disable
	}

	public void DefaultModifiers()
	{
		cofactor    = 1f;
		soldierType = SoldierType.Normal;
	}
#endregion

#region Implementation
	private void RigidbodyCheck()
	{
		if( dice_Rigidbody.IsSleeping() ) // If rigidbody is stopped and changed its status to SLEEP
		{
			ReturnDefault();
			DOVirtual.DelayedCall( GameSettings.Instance.dice_waitTimeAfterSleep, OnRigidbodySleep ); // Delayed call 
		}
	}

	private void OnRigidbodySleep()
	{
		float diceNumber = CheckWhichSideIsUp();

		// Dice Event: Position, Ally or Enemy, Dice number
		diceEvent.position    = transform.position;
		diceEvent.diceNumber  = ( int )Mathf.Max( diceNumber * cofactor, 1);
		diceEvent.party       = party;
		diceEvent.soldierType = soldierType;
		diceEvent.diceID      = GetInstanceID();

		diceEvent.Raise();

		// Dice Disappear Particle Effect 
		diceDisappearParticleEvent.changePosition = true;
		diceDisappearParticleEvent.spawnPoint     = transform.position;
		diceDisappearParticleEvent.particleAlias  = "dice_disappear";

		diceDisappearParticleEvent.Raise();


		FFLogger.Log( $"{name}: {diceNumber}" );
	}

	// Up: 1, Right: 3, Forward: 5
	private int CheckWhichSideIsUp()
	{
		// Check Dot Product of every axis with Vector3.Up
		dotProducts[ 0 ] = Vector3.Dot( transform.up, Vector3.up );
		dotProducts[ 1 ] = Vector3.Dot( -transform.up, Vector3.up );
		dotProducts[ 2 ] = Vector3.Dot( transform.right, Vector3.up );
		dotProducts[ 3 ] = Vector3.Dot( -transform.right, Vector3.up );
		dotProducts[ 4 ] = Vector3.Dot( transform.forward, Vector3.up );
		dotProducts[ 5 ] = Vector3.Dot( -transform.forward, Vector3.up );

		int selected = 0;
		float max = dotProducts[ 0 ];

		// Find the biggest result
		for( var i = 0; i < diceNumbers.Length - 1; i++ )
		{
			if( dotProducts[ i + 1 ] >= max )
			{
				max = dotProducts[ i + 1 ]; // Cache the max 
				selected = i + 1;
			}
		}

		return diceNumbers[ selected ];
	}
#endregion
}
