using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Raylib_cs;

namespace RLChess;

internal class Board
{
    public static readonly Color boardBlack   = new(236, 167, 95, 255);
    public static readonly Color boardWhite   = new(252, 219, 166, 255);

    public static readonly Color boardHover   = new(127, 127, 255, 255);
    public static readonly Color boardSelect  = new(44, 200, 37, 255);
    public static readonly Color boardMove    = new(155, 235, 135, 255);
    public static readonly Color boardCapture = new(255, 80, 80, 255);

    public static Rectangle TileRect(int x, int y) => new(
        x * ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS,
        y * ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS,
            ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS,
            ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS);

    public static (int, int) OutputPixelToTile(Vector2 pixel) =>
        ((int)MathF.Floor(pixel.X / ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS),
         (int)MathF.Floor(pixel.Y / ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS));

    public static Vector2 TileToOutputPixel(int x, int y) =>
        new(x * ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS,
            y * ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS);

    public Unit[] units;
    readonly Unit.Type[] royalty = new Unit.Type[]{
        Unit.Type.Rook,
        Unit.Type.Knight,
        Unit.Type.Bishop,
        Unit.Type.Queen,
        Unit.Type.King,
        Unit.Type.Bishop,
        Unit.Type.Knight,
        Unit.Type.Rook,
    };

    public Board()
    {
        const int sideTiles = ChessConstants.NUM_BOARD_SIDE_TILES;

        units = new Unit[ChessConstants.NUM_BOARD_SIDE_TILES * 4];

        int totalAssets = ChessConstants.NUM_BOARD_SIDE_TILES * 4;
        int loadedAssets = 0;

        for (int col = 0; col < ChessConstants.NUM_BOARD_SIDE_TILES; ++col)
        {
            for (int i = 0; i < 4; ++i)
            {
                int index = col + sideTiles * i;
                int row = (sideTiles + i - 2) % sideTiles;
                Unit.Type type = (i is 1 or 2) ? royalty[col] : Unit.Type.Pawn;
                Unit.Team team = (i < 2) ? Unit.Team.White : Unit.Team.Black;

                units[index] = new(col, row, type, team);

                ++loadedAssets;

                float percentLoaded = MathF.Floor(((float)loadedAssets / totalAssets) * 100.0f);

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);
                Raylib.DrawRectangle(0, 0, (int)((percentLoaded / 100.0f) * ChessConstants.NUM_OUTPUT_BOARD_SIDE_PIXELS), 20, Color.BLUE);
                Raylib.DrawText("Loading assets... " + percentLoaded + "%", 0, 0, 20, Color.WHITE);
                Raylib.EndDrawing();
            }
        }
    }
}
