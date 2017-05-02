/// <summary>
/// The BattleShipsGame controls a big part of the game. It will add the two players
/// to the game and make sure that both players ships are all deployed before starting the game.
/// It also allows players to shoot and swap turns between player. It will also check if players
/// are destroyed.
/// </summary>
namespace Battleship
{
    public enum Direction
    {
        /// <summary>
        /// The ship is oriented left/right
        /// </summary>
        Left,
        Right,
        Up,

        /// <summary>
        /// The ship is oriented up/down
        /// </summary>
        Down
    }
}