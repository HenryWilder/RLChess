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

            // Background
            {
                Color boardBlack = spriteStyle switch {
                    SpriteStyle.CC2 => Board.boardBlack,
                    SpriteStyle.CC3 => Color.BLACK,
                    _ => throw new NotImplementedException(),
                };

                Color boardWhite = spriteStyle switch {
                    SpriteStyle.CC2 => Board.boardWhite,
                    SpriteStyle.CC3 => Color.WHITE,
                    _ => throw new NotImplementedException(),
                };

                Raylib.DrawRectangle(0, 0, NUM_OUTPUT_BOARD_SIDE_PIXELS, NUM_OUTPUT_BOARD_SIDE_PIXELS, boardBlack);

                for (int row = 0; row <= NUM_BOARD_SIDE_TILES; ++row)
                {
                    for (int col = row & 1; col <= NUM_BOARD_SIDE_TILES; col += 2)
                    {
                        Raylib.DrawRectangleRec(Board.TileRect(col, row), boardWhite);
                    }
                }
            }

            // Hovered tile
            Raylib.DrawRectangleRec(Board.TileRect(hoveredTileX, hoveredTileY), Board.boardHover);

            // Units
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

            // Loading screen fadein
            if (fadeIn)
            {
                float t = (float)(1.0 - (Raylib.GetTime() - finishedLoading));
                Raylib.DrawRectangle(0, 0, NUM_OUTPUT_BOARD_SIDE_PIXELS, NUM_OUTPUT_BOARD_SIDE_PIXELS, Raylib.ColorAlpha(Color.BLACK, t));
                Raylib.DrawRectangle(0, 0, NUM_OUTPUT_BOARD_SIDE_PIXELS, 20, Raylib.ColorAlpha(Color.BLUE, t));
                Raylib.DrawText("Ready", 0, 0, 20, Raylib.ColorAlpha(Color.WHITE, t));
            }

            Raylib.EndDrawing();
        }

        board.Unload();
    }

    public static void Main()
    {
        Raylib.InitWindow(NUM_OUTPUT_BOARD_SIDE_PIXELS, NUM_OUTPUT_BOARD_SIDE_PIXELS, "Raylib Chess");

        Game();

        Raylib.CloseWindow();
    }
}