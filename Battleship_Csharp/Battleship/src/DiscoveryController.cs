using SwinGameSDK;
using System;

/// <summary>
/// The DeploymentController controls the players actions
/// during the deployment phase.
/// </summary>
/// 
namespace Battleship

{
    sealed class DiscoveryController
    {

        private const int MUTE_BUTTON_TOP = 10;
        private const int MUTE_BUTTON_LEFT = 720;
        private const int MUTE_BUTTON_WIDTH = 40;
        private const int MUTW_BUTTON_HEIGHT = 30;

        public static bool Sound = true;

        /// <summary>
        /// Handles input during the discovery phase of the game.
        /// </summary>
        /// <remarks>
        /// Escape opens the game menu. Clicking the mouse will
        /// attack a location.
        /// </remarks>
        public static void HandleDiscoveryInput()
        {
            if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE))
            {
               GameController.AddNewState(GameState.ViewingGameMenu);
            }

            if (SwinGame.MouseClicked(MouseButton.LeftButton))
            {
                DoAttack();
            }


            if (SwinGame.MouseClicked(MouseButton.LeftButton))
            {
                if (UtilityFunctions.IsMouseInRectangle(MUTE_BUTTON_LEFT, MUTE_BUTTON_TOP, MUTE_BUTTON_WIDTH, MUTW_BUTTON_HEIGHT) && (Sound == true))
                {
                    SwinGame.StopMusic();
                    SwinGame.DrawBitmap(GameResources.GameImage("MuteButton"), MUTE_BUTTON_LEFT, MUTE_BUTTON_TOP);
                    Sound = false;
                }
                else if (UtilityFunctions.IsMouseInRectangle(MUTE_BUTTON_LEFT, MUTE_BUTTON_TOP, MUTE_BUTTON_WIDTH, MUTW_BUTTON_HEIGHT) && (Sound == false))
                {
                    SwinGame.PlayMusic(GameResources.GameMusic("Background"));
                    SwinGame.DrawBitmap(GameResources.GameImage("SoundButton"), MUTE_BUTTON_LEFT, MUTE_BUTTON_TOP);
                    Sound = true;
                }
            }
            
        }

        /// <summary>
        /// Attack the location that the mouse if over.
        /// </summary>
        private static void DoAttack()
        {
            Point2D mouse = default(Point2D);

            mouse = SwinGame.MousePosition();

            //Calculate the row/col clicked
            int row = 0;
            int col = 0;
            row = System.Convert.ToInt32(Convert.ToInt32(Math.Floor(System.Convert.ToDouble(mouse.Y - UtilityFunctions.FIELD_TOP) / System.Convert.ToDouble(UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP))));
            col = System.Convert.ToInt32(Convert.ToInt32(Math.Floor(System.Convert.ToDouble(mouse.X - UtilityFunctions.FIELD_LEFT) / System.Convert.ToDouble(UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP))));

            if (row >= 0 & row < GameController.HumanPlayer.EnemyGrid.Height)
            {
                if (col >= 0 & col < GameController.HumanPlayer.EnemyGrid.Width)
                {
                   GameController.Attack(row, col);
                }
            }
        }

        /// <summary>
        /// Draws the game during the attack phase.
        /// </summary>s
        public static void DrawDiscovery()
        {
            const int SCORES_LEFT = 172;
            const int SHOTS_TOP = 157;
            const int HITS_TOP = 206;
            const int SPLASH_TOP = 256;

            if ((SwinGame.KeyDown(KeyCode.vk_LSHIFT) || SwinGame.KeyDown(KeyCode.vk_RSHIFT)) && SwinGame.KeyDown(KeyCode.vk_c))
            {
               UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, true);
            }
            else
            {
                UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, false);
            }

            UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
            UtilityFunctions.DrawMessage();

            SwinGame.DrawText(GameController.HumanPlayer.Shots.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SHOTS_TOP);
            SwinGame.DrawText(GameController.HumanPlayer.Hits.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, HITS_TOP);
            SwinGame.DrawText(GameController.HumanPlayer.Missed.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SPLASH_TOP);

            if (Sound == true)
            {
                SwinGame.DrawBitmap(GameResources.GameImage("SoundButton"), MUTE_BUTTON_LEFT, MUTE_BUTTON_TOP);
            }
            if(Sound == false)
            {
                SwinGame.DrawBitmap(GameResources.GameImage("MuteButton"), MUTE_BUTTON_LEFT, MUTE_BUTTON_TOP);
            }

        }

    }
}