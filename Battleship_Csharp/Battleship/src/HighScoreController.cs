
using SwinGameSDK;
using System;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// The GameStates represent the state of the Battleships game play.
/// This is used to control the actions and view displayed to
/// the player.
/// </summary>
namespace Battleship
{
    sealed class HighScoreController
    {
        private const int NAME_WIDTH = 3;
        private const int SCORES_LEFT = 510;

        /// <summary>
        /// The score structure is used to keep the name and
        /// score of the top players together.
        /// </summary>
        private struct Score : IComparable
        {
            public string Name;
            public int Value;

            /// <summary>
            /// Allows scores to be compared to facilitate sorting
            /// </summary>
            /// <param name="obj">the object to compare to</param>
            /// <returns>a value that indicates the sort order</returns>
            public int CompareTo(object obj)
            {
                if (obj is Score)
                {
                    Score other = (Score)obj;

                    return other.Value - this.Value;
                }
                else
                {
                    return 0;
                }
            }
        }

        private static List<Score> _Scores = new List<Score>();

        /// <summary>
        /// Loads the scores from the highscores text file.
        /// </summary>
        /// <remarks>
        /// The format is
        /// # of scores
        /// NNNSSS
        ///
        /// Where NNN is the name and SSS is the score
        /// </remarks>
        private static void LoadScores()
        {
            string filename = "";
            filename = System.Convert.ToString(SwinGame.PathToResource("highscores.txt"));

            StreamReader input = default(StreamReader);
            input = new StreamReader(filename);

            //Read in the # of scores
            int numScores = 0;
            numScores = System.Convert.ToInt32(Convert.ToInt32(input.ReadLine()));

            _Scores.Clear();

            int i = 0;

            for (i = 1; i <= numScores; i++)
            {
                Score s = new Score();
                string line = "";

                line = System.Convert.ToString(input.ReadLine());

                s.Name = line.Substring(0, NAME_WIDTH);
                s.Value = System.Convert.ToInt32(Convert.ToInt32(line.Substring(NAME_WIDTH)));
                _Scores.Add(s);
            }
            input.Close();
        }

        /// <summary>
        /// Saves the scores back to the highscores text file.
        /// </summary>
        /// <remarks>
        /// The format is
        /// # of scores
        /// NNNSSS
        ///
        /// Where NNN is the name and SSS is the score
        /// </remarks>
        private static void SaveScores()
        {
            string filename = "";
            filename = System.Convert.ToString(SwinGame.PathToResource("highscores.txt"));

            StreamWriter output = default(StreamWriter);
            output = new StreamWriter(filename);

            output.WriteLine(_Scores.Count);

            foreach (Score s in _Scores)
            {
                output.WriteLine(s.Name + System.Convert.ToString(s.Value));
            }

            output.Close();
        }

        /// <summary>
        /// Draws the high scores to the screen.
        /// </summary>
        public static void DrawHighScores()
        {
            const int SCORES_HEADING = 40;
            const int SCORES_TOP = 80;
            const int SCORE_GAP = 30;

            if (_Scores.Count == 0)
            {
                LoadScores();
            }

            SwinGame.DrawText("   High Scores   ", Color.White, GameResources.GameFont("HighScore"), SCORES_LEFT, SCORES_HEADING);

            //For all of the scores
            int i = 0;
            for (i = 0; i <= _Scores.Count - 1; i++)
            {
                Score s = new Score();

                s = _Scores[i];

                //for scores 1 - 10
                if (i < 10)
                {
                    SwinGame.DrawText(" " + System.Convert.ToString(i + 1) + ":   " + s.Name + "   " + System.Convert.ToString(s.Value), Color.White, GameResources.GameFont("HighScore"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
                }
            }
        }

        /// <summary>
        /// Handles the user input during the top score screen.
        /// </summary>
        /// <remarks></remarks>
        public static void HandleHighScoreInput()
        {
            if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.vk_ESCAPE) || SwinGame.KeyTyped(KeyCode.vk_RETURN))
            {
               GameController.EndCurrentState();
            }
        }

        /// <summary>
        /// Read the user's name for their highsSwinGame.
        /// </summary>
        /// <param name="value">the player's sSwinGame.</param>
        /// <remarks>
        /// This verifies if the score is a highsSwinGame.
        /// </remarks>
        public static void ReadHighScore(int value)
        {
            const int ENTRY_TOP = 500;

            if (_Scores.Count == 0)
            {
                LoadScores();
            }

            //is it a high score
            if (value > _Scores[_Scores.Count - 1].Value)
            {
                Score s = new Score();
                s.Value = value;

                GameController.AddNewState(GameState.ViewingHighScores);

                int x = 0;
                x = SCORES_LEFT + SwinGame.TextWidth(GameResources.GameFont("Courier"), "Name: ");

                SwinGame.StartReadingText(Color.White, NAME_WIDTH, GameResources.GameFont("Courier"), x, ENTRY_TOP);

                //Read the text from the user
                while (SwinGame.ReadingText())
                {
                    SwinGame.ProcessEvents();

                  UtilityFunctions.DrawBackground();
                    DrawHighScores();
                    SwinGame.DrawText("Name: ", Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, ENTRY_TOP);
                    SwinGame.RefreshScreen();
                }

                s.Name = System.Convert.ToString(SwinGame.TextReadAsASCII());

                if (s.Name.Length < 3)
                {
                    s.Name = s.Name + new string(' ', 3 - s.Name.Length);
                }

                _Scores.RemoveAt(_Scores.Count - 1);
                _Scores.Add(s);
                _Scores.Sort();

               GameController.EndCurrentState();
            }
        }
    }

}