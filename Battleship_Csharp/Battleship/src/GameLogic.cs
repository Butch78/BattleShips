using SwinGameSDK;
using System;

/// <summary>
/// The EndingGameController is responsible for managing the interactions at the end
/// of a game.
/// </summary>

namespace Battleship

{
    sealed class GameLogic
    {
        public static void Main()
        {
            //Opens a new Graphics Window
            SwinGame.OpenGraphicsWindow("Battle Ships", 800, 600);

            //Load Resources
           GameResources.LoadResources();

            SwinGame.PlayMusic(GameResources.GameMusic("Background"));
            
            //Game Loop
            do
            {
               GameController.HandleUserInput();
               GameController.DrawScreen();
            } while (!(SwinGame.WindowCloseRequested() == true ||GameController.CurrentState == GameState.Quitting));

            SwinGame.StopMusic();

            //Free Resources and Close Audio, to end the program.
           GameResources.FreeResources();
        }
    }
}