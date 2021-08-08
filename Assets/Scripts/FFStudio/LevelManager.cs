/* Created by and for usage of FF Studios (2021). */

using UnityEngine;

namespace FFStudio
{
    public class LevelManager : MonoBehaviour
    {
#region Fields
        [ Header("Event Listeners" ) ]
        public EventListenerDelegateResponse levelLoadedListener;
        public EventListenerDelegateResponse levelRevealedListener;
        public EventListenerDelegateResponse levelStartedListener;
		public EventListenerDelegateResponse diceTriggerdNetListener; // ReferenceGameEvent
		public EventListenerDelegateResponse tileCaptureListener;

		[ Header("Fired Events" ) ]
        public GameEvent levelFailedEvent;
        public GameEvent levelCompleted;

        [ Header("Level Releated" ) ]
        public SharedFloatProperty levelProgress;

		// Private Fields
		private Parties[,] scoreBoard = new Parties[ 3, 3 ];
#endregion

#region UnityAPI
		private void OnEnable()
        {
            // Level Releated
            levelLoadedListener  .OnEnable();
            levelRevealedListener.OnEnable();
            levelStartedListener .OnEnable();

			// Game Releated
			diceTriggerdNetListener.OnEnable();
			tileCaptureListener    .OnEnable();
		}

        private void OnDisable()
        {
            // Level Releated
            levelLoadedListener  .OnDisable();
            levelRevealedListener.OnDisable();
            levelStartedListener .OnDisable();

			// Game Releated
			diceTriggerdNetListener.OnDisable();
			tileCaptureListener    .OnDisable();
        }

        private void Awake()
        {
            levelLoadedListener.response   = LevelLoadedResponse;
            levelRevealedListener.response = LevelRevealedResponse;
            levelStartedListener.response  = LevelStartedResponse;

			diceTriggerdNetListener.response = DiceTriggedNetResponse;
			tileCaptureListener.response     = ExtensionMethods.EmptyMethod;
		}
#endregion

#region Implementation
        void LevelLoadedResponse()
        {
            levelProgress.SetValue(0);
        }

        void LevelRevealedResponse()
        {
        }

        void LevelStartedResponse()
        {
			DefaultScoreBoard();

			tileCaptureListener.response = TileCaptureResponse;
		}

        // Return the dice to Default when it triggered the Net
        private void DiceTriggedNetResponse()
        {
			var dice = ( ( diceTriggerdNetListener.gameEvent as ReferenceGameEvent ).eventValue as Collider ).GetComponentInParent< Dice >();
			dice.ReturnDefault();
		}

        // Dice if any parties have won the game
        private void TileCaptureResponse()
        {
			var tileEvent = tileCaptureListener.gameEvent as TileEvent;

			var x = tileEvent.tileID / 3;
			var y = tileEvent.tileID % 3;
			
            scoreBoard[ x, y ] = tileEvent.party;

			FFLogger.Log( "Tile: { " + x + "," + y + " } - " + tileEvent.party );
            FFLogger.Log( "Score Board\n" + ExtensionMethods.Log2DArray( scoreBoard, 3 ) );

			Parties winnerParty;
			var hasWinner = CheckIfBoardIsSolved( out winnerParty );

            if( hasWinner )
            {
                if( winnerParty == Parties.Ally )
                {
                    FFLogger.Log( "Ally Won" );
					levelCompleted.Raise();
                }
                else if ( winnerParty == Parties.Enemy )
                {
                    FFLogger.Log( "Enemy Won" );
					levelFailedEvent.Raise();
                }

                FFLogger.Log( "Has Winner!" );
				tileCaptureListener.response = ExtensionMethods.EmptyMethod;
			}
		}

        private bool CheckIfBoardIsSolved( out Parties winner )
        {
            // Check Rows
            for( var i = 0; i < 3; i++ )
            {
                // Check if every cell of the same row is owned by same party
				if( scoreBoard[ i, 0 ] == scoreBoard[ i, 1 ] && scoreBoard[ i, 1 ] == scoreBoard[ i, 2 ] )
                {
					winner = scoreBoard[ i, 0 ];

                    if( winner != Parties.Neutral )
                    {
						return true;
					}
				}
			}

            // Check Columns
            for( var i = 0; i < 3; i++ )
            {
                // Check if every cell of the same column is owned by same party
				if( scoreBoard[ 0, i ] == scoreBoard[ 1, i ] && scoreBoard[ 1, i ] == scoreBoard[ 2, i ] )
                {
					winner = scoreBoard[ 0, i ];

					if( winner != Parties.Neutral )
					{
						return true;
					}
				}
			}

			// Check left to right diagonal
			bool leftToRight = scoreBoard[ 2, 0 ] == scoreBoard[ 1, 1 ] && scoreBoard[ 1, 1 ] == scoreBoard[ 0, 2 ];
			// Check right to left diagonal
			bool rightToLeft = scoreBoard[ 0, 0 ] == scoreBoard[ 1, 1 ] && scoreBoard[ 1, 1 ] == scoreBoard[ 2, 2 ];

			if( leftToRight || rightToLeft )
            {
				winner = scoreBoard[ 1, 1 ];

				if( winner != Parties.Neutral )
				{
					return true;
				}
			}

			winner = Parties.Neutral;
			return false;
		}

        private void DefaultScoreBoard()
        {
            for( var x = 0; x < 3; x++ )
            {
                for( var y = 0; y < 3; y++ )
                {
					scoreBoard[ x, y ] = Parties.Neutral;
				}
            }
        }
#endregion
    }
}