using System.Numerics;

using Raylib_cs;

using RLChess;
using static RLChess.ChessConstants;
using static RLChess.Sprites;

class Program
{
    public static void Main(string[] args)
    {
        Raylib.InitWindow(NUM_OUTPUT_BOARD_SIDE_PIXELS, NUM_OUTPUT_BOARD_SIDE_PIXELS, "Raylib Chess");

        Board board = new();
        SpriteStyle spriteStyle = SpriteStyle.CC3;

        Camera3D cam = new(
            position: Vector3.UnitZ * 100,
            target: Vector3.Zero,
            up: Vector3.UnitY,
            fovy: NUM_OUTPUT_BOARD_SIDE_PIXELS,
            projection: CameraProjection.CAMERA_ORTHOGRAPHIC
        );

        while (!Raylib.WindowShouldClose())
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            var (hoveredTileX, hoveredTileY) = Board.OutputPixelToTile(mousePos);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            board.DrawBackground();

            Raylib.DrawRectangleRec(Board.TileRect(hoveredTileX, hoveredTileY), Board.boardHover);

            switch (spriteStyle)
            {
                case SpriteStyle.CC2:
                {
                    foreach (Unit unit in board.units)
                    {
                        unit.DrawCC2(Vector2.Zero);
                    }
                } break;

                case SpriteStyle.CC3:
                {
                    Raylib.BeginMode3D(cam);

                    foreach (Unit unit in board.units)
                    {
                        unit.DrawCC3(Vector2.Zero);
                    }

                    Raylib.EndMode3D();
                } break;

                default:
                    throw new NotImplementedException();
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}