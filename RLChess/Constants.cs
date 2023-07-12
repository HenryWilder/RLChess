namespace RLChess;

class ChessConstants
{
    /// <summary>
    /// Edge tiles on the board.
    /// </summary>
    public const int NUM_BOARD_SIDE_TILES = 8;

    /// <summary>
    /// The board's area in tiles.
    /// </summary>
    public const int NUM_BOARD_TILES = NUM_BOARD_SIDE_TILES * NUM_BOARD_SIDE_TILES;

    /// <summary>
    /// Edge sprite-pixels on a tile.
    /// </summary>
    public const int NUM_TILE_SIDE_PIXELS = 16;

    /// <summary>
    /// A tile's area in sprite-pixels.
    /// </summary>
    public const int NUM_TILE_PIXELS = NUM_TILE_SIDE_PIXELS * NUM_TILE_SIDE_PIXELS;

    /// <summary>
    /// Edge sprite-pixels on the board.
    /// </summary>
    public const int NUM_BOARD_SIDE_PIXELS = NUM_BOARD_SIDE_TILES * NUM_TILE_SIDE_PIXELS;

    /// <summary>
    /// The board's area in sprite-pixels.
    /// </summary>
    public const int NUM_BOARD_PIXELS = NUM_BOARD_SIDE_PIXELS * NUM_BOARD_SIDE_PIXELS;

    /// <summary>
    /// Scaling of the game (sprite-pixels to true pixels).
    /// </summary>
    public const int OUTPUT_SCALE = 4;

    /// <summary>
    /// True edge pixels on a tile.
    /// </summary>
    public const int NUM_OUTPUT_TILE_SIDE_PIXELS = OUTPUT_SCALE * NUM_TILE_SIDE_PIXELS;

    /// <summary>
    /// A tile's area in true pixels.
    /// </summary>
    public const int NUM_OUTPUT_TILE_PIXELS = OUTPUT_SCALE * NUM_TILE_PIXELS;

    /// <summary>
    /// True edge pixels on the board.
    /// </summary>
    public const int NUM_OUTPUT_BOARD_SIDE_PIXELS = OUTPUT_SCALE * NUM_BOARD_SIDE_PIXELS;

    /// <summary>
    /// The board's area in true pixels.
    /// </summary>
    public const int NUM_OUTPUT_BOARD_PIXELS = OUTPUT_SCALE * NUM_TILE_PIXELS;
}