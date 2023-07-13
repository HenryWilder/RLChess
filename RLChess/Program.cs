using System.Numerics;

using Raylib_cs;

using RLChess;
using static RLChess.ChessConstants;

class Program
{
    public static void Main(string[] args)
    {
        Raylib.InitWindow(NUM_OUTPUT_BOARD_SIDE_PIXELS, NUM_OUTPUT_BOARD_SIDE_PIXELS, "Raylib Chess");

        Sprites sprites = new();
        Board board = new();

        while (!Raylib.WindowShouldClose())
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            var (hoveredTileX, hoveredTileY) = Board.OutputPixelToTile(mousePos);

            sprites.Tick();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            board.DrawBackground();

            Raylib.DrawRectangleRec(Board.TileRect(hoveredTileX, hoveredTileY), Board.boardHover);

            board.DrawUnits(sprites, Sprites.SpriteStyle.CC3);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}