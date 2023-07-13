using System.Numerics;

using Raylib_cs;

using RLChess;
using static RLChess.ChessConstants;

enum SpriteStyle
{
    CC2, // Raster
    CC3, // Vector
}

class Program
{
    // Allows for asset construction and destruction only while window exists
    public static void Game()
    {
        SpriteStyle spriteStyle = SpriteStyle.CC2;

        Camera3D cam = new(
            position: Vector3.UnitZ * 100,
            target: Vector3.Zero,
            up: Vector3.UnitY,
            fovy: NUM_OUTPUT_BOARD_SIDE_PIXELS,
            projection: CameraProjection.CAMERA_ORTHOGRAPHIC
        );

        Board board = new();

        double finishedLoading = Raylib.GetTime();

        while (!Raylib.WindowShouldClose())
        {
            bool fadeIn = Raylib.GetTime() < finishedLoading + 1.0;

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_TWO))
            {
                spriteStyle = SpriteStyle.CC2;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_THREE))
            {
                spriteStyle = SpriteStyle.CC3;
            }

            Vector2 mousePos = Raylib.GetMousePosition();
            var (hoveredTileX, hoveredTileY) = Board.OutputPixelToTile(mousePos);

            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.BLACK);

            Board.DrawBackground();

            Raylib.DrawRectangleRec(Board.TileRect(hoveredTileX, hoveredTileY), Board.boardHover);

            switch (spriteStyle)
            {
                case SpriteStyle.CC2:
                {
                    foreach (Unit unit in board.units)
                    {
                        unit.DrawCC2(Vector2.Zero);
                    }
                }
                break;

                case SpriteStyle.CC3:
                {
                    Raylib.BeginMode3D(cam);

                    foreach (Unit unit in board.units)
                    {
                        unit.DrawCC3(Vector2.Zero);
                    }

                    Raylib.EndMode3D();
                }
                break;

                default:
                    throw new NotImplementedException();
            }

            if (fadeIn)
            {
                float t = (float)(1.0 - (Raylib.GetTime() - finishedLoading));
                Raylib.DrawRectangle(0, 0, NUM_OUTPUT_BOARD_SIDE_PIXELS, NUM_OUTPUT_BOARD_SIDE_PIXELS, Raylib.ColorAlpha(Color.BLACK, t));
                Raylib.DrawRectangle(0, 0, NUM_OUTPUT_BOARD_SIDE_PIXELS, 20, Raylib.ColorAlpha(Color.BLUE, t));
                Raylib.DrawText("Ready", 0, 0, 20, Raylib.ColorAlpha(Color.WHITE, t));
            }

            Raylib.EndDrawing();
        }
    }

    public static void Main()
    {
        Raylib.InitWindow(NUM_OUTPUT_BOARD_SIDE_PIXELS, NUM_OUTPUT_BOARD_SIDE_PIXELS, "Raylib Chess");

        Game();

        Raylib.CloseWindow();
    }
}