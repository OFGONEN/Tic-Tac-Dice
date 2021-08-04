/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using NaughtyAttributes;

public class DiceThrower : MonoBehaviour
{
#region Fields
	[ Header( "Shared Variables" ) ]
	public EventListenerDelegateResponse levelStartListener;

	[ Header( "Shared Variables" ) ]
	public DicePool dicePool;

	[ Header( "Setup" ) ]
	[ SerializeField ] private Image cooldownIndicator;
	[ SerializeField ] private Animator animator;
	[ SerializeField ] private Transform closest_TargetPosition;
	[ SerializeField ] private Transform farest_TargetPosition;

	// Properties \\
	public Vector3 ClosestTargetPosition => closest_TargetPosition.position;
	public bool CanThrowDice => canThrowDice;


	// Private Fields \\
	private Dice currentDice;
	private float nextDiceThrow = 0; // Cooldown for next dice throw
	private bool canThrowDice = false;
	private float dice_TravelTime;
	private Vector3 dice_LaunchVector;
	private Vector3[] dice_TrajectoryPoints;
	private float distanceBetweenTargetPoints;

	// Components
	private LineRenderer dice_TrajectoryLine;

	// Delegates
	private UnityMessage updateMethod;
#endregion

#region Unity API
	private void OnEnable()
	{
		levelStartListener.OnEnable();
	}

	private void OnDisable()
	{
		levelStartListener.OnDisable();
	}

	private void Awake()	
	{
		// Set delegates.
		updateMethod                   = ExtensionMethods.EmptyMethod;
		levelStartListener.response = LevelStartResponse;

		// Create trajectory points array.
		dice_TrajectoryPoints  = new Vector3[ GameSettings.Instance.dice_TrajectoryPointCount ];

		// Get and configure line renderer.
		dice_TrajectoryLine               = GetComponent< LineRenderer >();
		dice_TrajectoryLine.positionCount = GameSettings.Instance.dice_TrajectoryPointCount;
		dice_TrajectoryLine.enabled       = false;

		// Cache the distance between the nearest and farest point in the board.
		distanceBetweenTargetPoints = Vector3.Distance( farest_TargetPosition.position, closest_TargetPosition.position );

		cooldownIndicator.fillAmount = 0; // Since there is no dice when level is loaded.
	}

	private void Update()
	{
		updateMethod();
	}
#endregion

#region API
	// Update the position of the target point.
	public void UpdateTargetPoint( Vector3 position )
	{
		UpdateLaunchVector( position ); // Update the launch vector for throwing dice to the target position.
		UpdateTrajectory( position ); // Update the trajectory of the dice.

		// Look at the target point rotating only in the +Y axis.
		transform.LookAtAxis( position, Vector3.up );
	}

	public void EnableTrajectoryLine()
	{
		dice_TrajectoryLine.enabled = true;
	}

	// Launch the dice with cached launch vector.
	public void Launch()
	{
		// Start countdown for next dice throw.
		canThrowDice = false;
		nextDiceThrow = 0; // Set the cooldown counter to 0.
		updateMethod = CountDownForNextThrow;

		// Launch the dice towards target positon.
		currentDice.Launch( dice_LaunchVector );

		// Play the dice throw animation.
		animator.SetTrigger( "launch" );

		//TODO: Disable trajectory when dice hits the board first time.
		dice_TrajectoryLine.enabled = false; // Disable Trajectory.
	}
#endregion

#region Implementation
	// Calculate the launch vector for throwing the dice to the target position.
	private void UpdateLaunchVector( Vector3 position )
	{
		// Travel time is max when target position at the farest point of the board.
		// Travel time is min when target position at the closest point of the board.

		var distance      = Vector3.Distance( position, closest_TargetPosition.position ); // Find the distance between target point and the farest point.
		var distanceRatio = distance / distanceBetweenTargetPoints; // Calculate a ratio between the max distance and the current distance.

		var minMax_TravelTime = GameSettings.Instance.dice_MinMaxTravelTime;
		    dice_TravelTime   = Mathf.Lerp( minMax_TravelTime.x, minMax_TravelTime.y, distanceRatio );  // Find the travel time according the distance ratio.

		// Calculate the launch vector. 
		dice_LaunchVector.x = ( position.x - currentDice.transform.position.x ) / dice_TravelTime;
		dice_LaunchVector.z = ( position.z - currentDice.transform.position.z ) / dice_TravelTime;
		dice_LaunchVector.y = -Physics.gravity.y * dice_TravelTime / 2;
	}

	// Update the dice travel trajectory.
	private void UpdateTrajectory( Vector3 position )
	{
		var pointCount = dice_TrajectoryPoints.Length;

		// Find positions of the dice in the certain points in time. 
		for( var i = 0; i < pointCount; i++ )
		{
			// Total elapsed time in the step.
			var step = dice_TravelTime * i / ( pointCount - 1 ); 

			// Calculate the position in this exact step.
			Vector3 timePosition = new Vector3(
				dice_LaunchVector.x * step,
				dice_LaunchVector.y * step + Physics.gravity.y * step * step / 2f,
				dice_LaunchVector.z * step
			);

			// Final position. 
			dice_TrajectoryPoints[ i ] = timePosition + currentDice.transform.position;
		}

		dice_TrajectoryLine.SetPositions( dice_TrajectoryPoints ); // Set points into line renderer.
	}

	// Count down cooldown for next dice throw.
	private void CountDownForNextThrow()
	{
		var cooldown = GameSettings.Instance.dice_coolDown; // Cache cooldown value.

		nextDiceThrow += Time.deltaTime; // Add passed time.

		// Find ratio of the cooldown.
		var cooldownRatio = Mathf.Min( nextDiceThrow / cooldown , 1f ); // Max value can ratio be is 1.
		cooldownIndicator.fillAmount = cooldownRatio; // Set indicator fill amount.

		// When cooldown is over.
		if( nextDiceThrow >= cooldown )
		{
			SpawnDice(); // Spawn a dice.
			updateMethod  = ExtensionMethods.EmptyMethod;  // Stop counting for cooldown.
		}
	}

	// Spawn and cache a dice.
	private void SpawnDice()
	{
		currentDice = dicePool.GiveEntity( transform, true ); // Get a dice from dice pool.

		// Spawn the dice, set it's parent to this so dice can rotate with, set its rotation to same rotation as this.
		currentDice.Spawn( Vector3.zero, Quaternion.identity ); 

		canThrowDice  = true;// Set flag value for throwing dice true.
		cooldownIndicator.fillAmount = 1f; // Set indicator fill amount.
	}

	// Response given when the Level is revealed.
	private void LevelStartResponse()
	{
		SpawnDice(); // Spawn a dice when level is started.
	}
#endregion
}
