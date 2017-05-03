using SwinGameSDK;
using System;

namespace Battleship
{
    sealed class DeploymentController
    {
        private const int MENU_TOP = 0;
		private const int MENU_LEFT = 0;
		private const int MENU_HEIGHT = 62;
		private const int MENU_WIDTH = 55;

		private const int SHIPS_TOP = 98;
        private const int SHIPS_LEFT = 20;
        private const int SHIPS_HEIGHT = 90;
        private const int SHIPS_WIDTH = 300;

        private const int TOP_BUTTONS_TOP = 72;
        private const int TOP_BUTTONS_HEIGHT = 40;

        private const int PLAY_BUTTON_LEFT = 693;
        private const int PLAY_BUTTON_WIDTH = 80;

        private const int UP_BUTTON_LEFT = 430;
        private const int LEFT_BUTTON_LEFT = 350;
        private const int DOWN_BUTTON_LEFT = 470;
        private const int RIGHT_BUTTON_LEFT = 390;

        private const int RANDOM_BUTTON_LEFT = 567;
        private const int RANDOM_BUTTON_WIDTH = 51;

        private const int DIR_BUTTONS_WIDTH = 40;

        private const int MUTE_BUTTON_TOP = 10;
        private const int MUTE_BUTTON_LEFT = 720;
        private const int MUTE_BUTTON_WIDTH = 40;
        private const int MUTW_BUTTON_HEIGHT = 30;


        private const int TEXT_OFFSET = 5;

        private static Direction _currentDirection = Direction.Up;
        private static ShipName _selectedShip = ShipName.Tug;
        public static bool Sound = true;


        /// <summary>
        /// Handles user input for the Deployment phase of the game.
        /// </summary>
        /// <remarks>
        /// Involves selecting the ships, deloying ships, changing the direction
        /// of the ships to add, randomising deployment, end then ending
        /// deployment
        /// </remarks>
        public static void HandleDeploymentInput()
        {
             

            if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE))
            {
                GameController.AddNewState(GameState.ViewingGameMenu);
            }

			else if (SwinGame.MouseClicked(MouseButton.LeftButton) & UtilityFunctions.IsMouseInRectangle(MENU_LEFT, MENU_TOP, MENU_WIDTH, MENU_WIDTH))
			{
				GameController.AddNewState(GameState.ViewingGameMenu);
			}

            if (SwinGame.KeyTyped(KeyCode.vk_UP) )
            {
                _currentDirection = Direction.Up;
            }
            if(SwinGame.KeyTyped(KeyCode.vk_DOWN))
            {
                _currentDirection = Direction.Down;
            }
            if (SwinGame.KeyTyped(KeyCode.vk_LEFT))
            {
                _currentDirection = Direction.Left;
            }
            if (SwinGame.KeyTyped(KeyCode.vk_RIGHT))
            {
                _currentDirection = Direction.Right;
            }

            if (SwinGame.KeyTyped(KeyCode.vk_r))
            {
               GameController.HumanPlayer.RandomizeDeployment();
            }

            if (SwinGame.MouseClicked(MouseButton.LeftButton))
            {
                ShipName selected = default(ShipName);
               
                selected = GetShipMouseIsOver();
                if (selected != ShipName.None)
                {
                    _selectedShip = selected;
                                
                }


                if (GameController.HumanPlayer.ReadyToDeploy && UtilityFunctions.IsMouseInRectangle(PLAY_BUTTON_LEFT, TOP_BUTTONS_TOP, PLAY_BUTTON_WIDTH, TOP_BUTTONS_HEIGHT))
                {
                    GameController.EndDeployment();
                }
                else if (UtilityFunctions.IsMouseInRectangle(UP_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT))
                {
                    _currentDirection = Direction.Up;
                    DoUpClick();
                }
                else if (UtilityFunctions.IsMouseInRectangle(DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT))
                {
                    _currentDirection = Direction.Down;
                    DoDownClick();
                }
                else if (UtilityFunctions.IsMouseInRectangle(LEFT_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT))
                {
                    _currentDirection = Direction.Left;
                    DoLeftClick();
                }
                else if (UtilityFunctions.IsMouseInRectangle(RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT))
                {
                    _currentDirection = Direction.Right;
                    DoRightClick();
                }
                else if (UtilityFunctions.IsMouseInRectangle(RANDOM_BUTTON_LEFT, TOP_BUTTONS_TOP, RANDOM_BUTTON_WIDTH, TOP_BUTTONS_HEIGHT))
                {
                    GameController.HumanPlayer.RandomizeDeployment();
                }
                else if (UtilityFunctions.IsMouseInRectangle(MUTE_BUTTON_LEFT, MUTE_BUTTON_TOP, MUTE_BUTTON_WIDTH, MUTW_BUTTON_HEIGHT) && (Sound == true))
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
                else
                {
                    DoDeployClick();
                }
            }
        }

        /// <summary>
        /// The user has clicked somewhere on the screen, check if its is a deployment and deploy
        /// the current ship if that is the case.
        /// </summary>
        /// <remarks>
        /// If the click is in the grid it deploys to the selected location
        /// with the indicated direction
        /// </remarks>
        private static void DoDeployClick()
        {
            
            Point2D mouse = default(Point2D);

            mouse = SwinGame.MousePosition();

            //Calculate the row/col clicked
            int row = 0;
            int col = 0;
           // row = System.Convert.ToInt32(Convert.ToInt32(Math.Floor((mouse.Y) / (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP))));
           row= System.Convert.ToInt32(Convert.ToInt32(Math.Floor(System.Convert.ToDouble(mouse.Y - UtilityFunctions.FIELD_TOP) / System.Convert.ToDouble(UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP))));
            col = System.Convert.ToInt32(Convert.ToInt32(Math.Floor(System.Convert.ToDouble(mouse.X - UtilityFunctions.FIELD_LEFT) / System.Convert.ToDouble(UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP))));

         

          
            if (row >= 0 & row < GameController.HumanPlayer.PlayerGrid.Height)
            {
                if (col >= 0 & col < GameController.HumanPlayer.PlayerGrid.Width)
                {
                    //if in the area try to deploy
                    try
                    {
                       GameController.HumanPlayer.PlayerGrid.MoveShip(row, col, _selectedShip, _currentDirection);
                    }
                    catch (Exception ex)
                    {
                        Audio.PlaySoundEffect(GameResources.GameSound("Error"));
                        UtilityFunctions.Message = ex.Message;
                    }
                }
            }
        }

        /// <summary>
        /// Draws the deployment screen showing the field and the ships
        /// that the player can deploy.
        /// </summary>
        public static void DrawDeployment()
        {
           UtilityFunctions.DrawField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer, true);
            SwinGame.DrawBitmap(GameResources.GameImage("LeftButton"), LEFT_BUTTON_LEFT, TOP_BUTTONS_TOP);
            SwinGame.DrawBitmap(GameResources.GameImage("RightButton"), RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP);
            SwinGame.DrawBitmap(GameResources.GameImage("UpButton"), UP_BUTTON_LEFT, TOP_BUTTONS_TOP);
            SwinGame.DrawBitmap(GameResources.GameImage("DownButton"), DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP);
            //Draw the Left/Right and Up/Down buttons
            if (_currentDirection == Direction.Left)
            {
                SwinGame.DrawRectangleOnScreen(Color.Yellow, LEFT_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT);
                //SwinGame.DrawBitmap(GameResources.GameImage("LeftButton"), LEFT_BUTTON_LEFT, TOP_BUTTONS_TOP);
                //SwinGame.DrawText("U/D", Color.Gray, GameResources.GameFont("Menu"), UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP)
                //SwinGame.DrawText("L/R", Color.White, GameResources.GameFont("Menu"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP)
            }
            else if (_currentDirection == Direction.Right)
            {
                SwinGame.DrawRectangleOnScreen(Color.Yellow, RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT);
                //SwinGame.DrawBitmap(GameResources.GameImage("RightButton"), LEFT_BUTTON_LEFT, TOP_BUTTONS_TOP);

                //SwinGame.DrawText("U/D", Color.Gray, GameResources.GameFont("Menu"), UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP)
                //SwinGame.DrawText("L/R", Color.White, GameResources.GameFont("Menu"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP)
            }
            else if(_currentDirection == Direction.Up)
            {
                SwinGame.DrawRectangleOnScreen(Color.Yellow, UP_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT);
                // SwinGame.DrawBitmap(GameResources.GameImage("UpButton"), LEFT_BUTTON_LEFT, TOP_BUTTONS_TOP);

                //SwinGame.DrawText("U/D", Color.White, GameResources.GameFont("Menu"), UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP)
                //SwinGame.DrawText("L/R", Color.Gray, GameResources.GameFont("Menu"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP)
            }
             else if(_currentDirection==Direction.Down)
            {
                SwinGame.DrawRectangleOnScreen(Color.Yellow, DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP, DIR_BUTTONS_WIDTH, TOP_BUTTONS_HEIGHT);

                // SwinGame.DrawBitmap(GameResources.GameImage("DownButton"), LEFT_BUTTON_LEFT, TOP_BUTTONS_TOP);
                //SwinGame.DrawText("U/D", Color.White, GameResources.GameFont("Menu"), UP_DOWN_BUTTON_LEFT, TOP_BUTTONS_TOP)
                //SwinGame.DrawText("L/R", Color.Gray, GameResources.GameFont("Menu"), LEFT_RIGHT_BUTTON_LEFT, TOP_BUTTONS_TOP)
            }

            //DrawShips
            foreach (ShipName sn in Enum.GetValues(typeof(ShipName)))
            {
                int i = 0;
                i = System.Convert.ToInt32(System.Convert.ToInt32(sn) - 1);
                if (i >= 0)
                {
                    if (sn == _selectedShip)
                    {
                        SwinGame.DrawBitmap(GameResources.GameImage("SelectedShip"), SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT);
                        
                        //    SwinGame.FillRectangle(Color.LightBlue, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                        //Else
                        //    SwinGame.FillRectangle(Color.Gray, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                    }

                    //SwinGame.DrawRectangle(Color.Black, SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT)
                    //SwinGame.DrawText(sn.ToString(), Color.Black, GameResources.GameFont("Courier"), SHIPS_LEFT + TEXT_OFFSET, SHIPS_TOP + i * SHIPS_HEIGHT)

                }
            }

            if (GameController.HumanPlayer.ReadyToDeploy)
            {
                SwinGame.DrawBitmap(GameResources.GameImage("PlayButton"), PLAY_BUTTON_LEFT, TOP_BUTTONS_TOP);
                //SwinGame.FillRectangle(Color.LightBlue, PLAY_BUTTON_LEFT, PLAY_BUTTON_TOP, PLAY_BUTTON_WIDTH, PLAY_BUTTON_HEIGHT)
                //SwinGame.DrawText("PLAY", Color.Black, GameResources.GameFont("Courier"), PLAY_BUTTON_LEFT + TEXT_OFFSET, PLAY_BUTTON_TOP)
            }

            SwinGame.DrawBitmap(GameResources.GameImage("RandomButton"), RANDOM_BUTTON_LEFT, TOP_BUTTONS_TOP);
			SwinGame.DrawBitmap(GameResources.GameImage("MenuButton"), MENU_LEFT, MENU_TOP);

            if(Sound == true)
            {
                SwinGame.DrawBitmap(GameResources.GameImage("SoundButton"), MUTE_BUTTON_LEFT, MUTE_BUTTON_TOP);
            }
            else
            {
                SwinGame.DrawBitmap(GameResources.GameImage("MuteButton"), MUTE_BUTTON_LEFT, MUTE_BUTTON_TOP);
            }
            


           UtilityFunctions.DrawMessage();




        }

        /// <summary>
        /// Gets the ship that the mouse is currently over in the selection panel.
        /// </summary>
        /// <returns>The ship selected or none</returns>
        private static ShipName GetShipMouseIsOver()
        {
            foreach (ShipName sn in Enum.GetValues(typeof(ShipName)))
            {
                int i = 0;
                i = System.Convert.ToInt32(System.Convert.ToInt32(sn) - 1);

                if (UtilityFunctions.IsMouseInRectangle(SHIPS_LEFT, SHIPS_TOP + i * SHIPS_HEIGHT, SHIPS_WIDTH, SHIPS_HEIGHT))
                {
                    Ship selectedShip = new Ship(sn);
                    return sn;
                }
            }

            return ShipName.None;
        }
        /// <summary>
        /// Move the ships left
        /// </summary>
        private static void DoLeftClick()
        {
            int row = GameController.HumanPlayer.PlayerGrid.Ships[_selectedShip].Row;
            int col = GameController.HumanPlayer.PlayerGrid.Ships[_selectedShip].Column;
            GameController.HumanPlayer.PlayerGrid.MoveShip(row, col, _selectedShip, _currentDirection);
            col = col - 1;


            //Calculate the row / col clicked

            if (row >= 0 & row < GameController.HumanPlayer.PlayerGrid.Height)
            {
                if (col >= 0 & col < GameController.HumanPlayer.PlayerGrid.Width)
                {
                    //if in the area try to deploy
                    try
                    {
                        GameController.HumanPlayer.PlayerGrid.MoveShip(row, col, _selectedShip, _currentDirection);
                    }
                    catch (Exception ex)
                    {
                        GameController.HumanPlayer.PlayerGrid.MoveShip(row, col+1, _selectedShip, _currentDirection);
                        Audio.PlaySoundEffect(GameResources.GameSound("Error"));
                        UtilityFunctions.Message = ex.Message;
                    }
                }
            }

            else
            {
                Audio.PlaySoundEffect(GameResources.GameSound("Error"));
                UtilityFunctions.Message = "Ship can't fit on the board if further moved";

            }
        }

        private static void DoRightClick()
        {

            int row = GameController.HumanPlayer.PlayerGrid.Ships[_selectedShip].Row;
            int col = GameController.HumanPlayer.PlayerGrid.Ships[_selectedShip].Column ;
            GameController.HumanPlayer.PlayerGrid.MoveShip(row, col, _selectedShip, _currentDirection);
            col = col + 1;

            //Calculate the row / col clicked

            if (row >= 0 & row < GameController.HumanPlayer.PlayerGrid.Height)
            {
                if (col >= 0 & col < GameController.HumanPlayer.PlayerGrid.Width)
                {
                    //if in the area try to deploy
                    try
                    {
                        GameController.HumanPlayer.PlayerGrid.MoveShip(row, col, _selectedShip, _currentDirection);
                    }
                    catch (Exception ex)
                    {
                        GameController.HumanPlayer.PlayerGrid.MoveShip(row, col-1, _selectedShip, _currentDirection);
                        Audio.PlaySoundEffect(GameResources.GameSound("Error"));
                        UtilityFunctions.Message = ex.Message;

                    }
                }
            }

            else
            {
                Audio.PlaySoundEffect(GameResources.GameSound("Error"));
                UtilityFunctions.Message = "Ship can't fit on the board if further moved";

            }
        }

        private static void DoUpClick()
        {
           

            int row = GameController.HumanPlayer.PlayerGrid.Ships[_selectedShip].Row;
            int col = GameController.HumanPlayer.PlayerGrid.Ships[_selectedShip].Column;

            GameController.HumanPlayer.PlayerGrid.MoveShip(row, col, _selectedShip, _currentDirection);
            row = row - 1;


            //Calculate the row / col clicked

            if (row >= 0 & row < GameController.HumanPlayer.PlayerGrid.Height)
            {
                if (col >= 0 & col < GameController.HumanPlayer.PlayerGrid.Width)
                {
                    //if in the area try to deploy
                    try
                    {
                        GameController.HumanPlayer.PlayerGrid.MoveShip(row, col, _selectedShip, _currentDirection);
                    }
                    catch (Exception ex)
                    {
                        GameController.HumanPlayer.PlayerGrid.MoveShip(row+1, col, _selectedShip, _currentDirection);
                        Audio.PlaySoundEffect(GameResources.GameSound("Error"));
                        UtilityFunctions.Message = ex.Message;
                    }
                }
            }

            else
            {
                Audio.PlaySoundEffect(GameResources.GameSound("Error"));
                UtilityFunctions.Message = "Ship can't fit on the board if further moved";
              

            }
        }

        private static void DoDownClick()
        {


            int row = GameController.HumanPlayer.PlayerGrid.Ships[_selectedShip].Row;
            int col = GameController.HumanPlayer.PlayerGrid.Ships[_selectedShip].Column;

            GameController.HumanPlayer.PlayerGrid.MoveShip(row, col, _selectedShip, _currentDirection);
            row = row + 1;


            //Calculate the row / col clicked

            if (row >= 0 & row < GameController.HumanPlayer.PlayerGrid.Height)
            {
                if (col >= 0 & col < GameController.HumanPlayer.PlayerGrid.Width)
                {
                    //if in the area try to deploy
                    try
                    {
                        GameController.HumanPlayer.PlayerGrid.MoveShip(row, col, _selectedShip, _currentDirection);
                    }
                    catch (Exception ex)
                    {
                        GameController.HumanPlayer.PlayerGrid.MoveShip(row-1, col, _selectedShip, _currentDirection);
                        Audio.PlaySoundEffect(GameResources.GameSound("Error"));
                        UtilityFunctions.Message = ex.Message;
                    }
                }
            }

            else
            {
                Audio.PlaySoundEffect(GameResources.GameSound("Error"));
                UtilityFunctions.Message = "Ship can't fit on the board if further moved";

            }
        }
    }
}
   