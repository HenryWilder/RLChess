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
        Camera3D cam = new(Vector3.UnitZ, -Vector3.UnitZ, Vector3.UnitY, NUM_OUTPUT_BOARD_SIDE_PIXELS, CameraProjection.CAMERA_ORTHOGRAPHIC);

        Model pawnModel = Raylib.LoadModel("resources/pawn.obj");
        Shader shader = Raylib.LoadShader("resources/displacement.vert", null);

        int displacementTimeLoc = Raylib.GetShaderLocation(shader, "time");

        for (int i = 0; i < pawnModel.materialCount; ++i)
        {
            Raylib.SetMaterialShader(ref pawnModel, i, ref shader);
        }

        while (!Raylib.WindowShouldClose())
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            var (hoveredTileX, hoveredTileY) = Board.OutputPixelToTile(mousePos);

            float time = (float)Raylib.GetTime();
            Raylib.SetShaderValue(shader, displacementTimeLoc, time, ShaderUniformDataType.SHADER_UNIFORM_FLOAT);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            board.DrawBackground();

            Raylib.DrawRectangleRec(Board.TileRect(hoveredTileX, hoveredTileY), Board.boardHover);

            board.DrawUnits(sprites);
            Raylib.BeginMode3D(cam);
            const float scale = NUM_OUTPUT_TILE_SIDE_PIXELS / 50.0f;
            Raylib.DrawModel(pawnModel, new(0,0,0), scale, Color.WHITE);
            Raylib.EndMode3D();

            Raylib.EndDrawing();
        }

        Raylib.UnloadModel(pawnModel);
        Raylib.UnloadShader(shader);

        Raylib.CloseWindow();
    }
}