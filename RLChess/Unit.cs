using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Raylib_cs;

namespace RLChess;

internal class Unit
{
    public enum Team
    {
        White,
        Black,
    }

    public enum Type
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King,
    }

    static int currentId = 0;

    public Unit(int x, int y, Type type, Team team)
    {
        this.x = x;
        this.y = y;
        this.type = type;
        this.team = team;

        string fileStart = "resources/" + type.ToString().ToLower();

        cc2Sprite = Raylib.LoadTexture(fileStart + ".png");
        cc3Model  = Raylib.LoadModel(fileStart + ".obj");
        cc3Shader = Raylib.LoadShader("resources/displacement.vert", null);

        Raylib.SetShaderValue(cc3Shader, Raylib.GetShaderLocation(cc3Shader, "id"), currentId++, ShaderUniformDataType.SHADER_UNIFORM_INT);
        cc3ShaderTimeLoc = Raylib.GetShaderLocation(cc3Shader, "time");
        cc3ShaderIntensityLoc = Raylib.GetShaderLocation(cc3Shader, "intensity");

        for (int i = 0; i < cc3Model.materialCount; ++i)
        {
            Raylib.SetMaterialShader(ref cc3Model, i, ref cc3Shader);
        }
    }

    ~Unit()
    {
        Raylib.UnloadTexture(cc2Sprite);
        Raylib.UnloadModel(cc3Model);
        Raylib.UnloadShader(cc3Shader);
    }

    public Type type;
    public Team team;
    public int x, y;

    Texture2D cc2Sprite;
    Model cc3Model;
    Shader cc3Shader;

    readonly int cc3ShaderTimeLoc;
    readonly int cc3ShaderIntensityLoc;

    const float NUM_MODEL_SIDE_UNITS = 50.0f;
    const float NUM_OUTPUT_BOARD_SIDE_PIXELS_HALF = ChessConstants.NUM_OUTPUT_BOARD_SIDE_PIXELS * 0.5f;
    const float MODEL_SCALE = ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS / NUM_MODEL_SIDE_UNITS;

    readonly static Color[] cc2Tints = new Color[] { Color.WHITE, Color.GRAY };

    public void DrawCC2(Vector2 pixelOffset)
    {
        Vector2 position = new (
            (x * ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS + pixelOffset.X),
            (y * ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS + pixelOffset.Y));

        Raylib.DrawTextureEx(cc2Sprite, position, 0.0f, ChessConstants.OUTPUT_SCALE, cc2Tints[(int)team]);
    }

    readonly static Color[] cc3Tints = new Color[] { Color.BLUE, Color.RED };

    // Assumes calling context has already called BeginMode3D.
    public void DrawCC3(Vector2 pixelOffset)
    {
        Vector3 position = new(
            +(x * ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS + pixelOffset.X - NUM_OUTPUT_BOARD_SIDE_PIXELS_HALF),
            -(y * ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS + pixelOffset.Y - NUM_OUTPUT_BOARD_SIDE_PIXELS_HALF),
            0.0f);

        Vector2 screenPosition = new(
            x * ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS + pixelOffset.X + ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS * 0.5f,
            y * ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS + pixelOffset.Y + ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS * 0.5f);

        float distance = Vector2.Distance(screenPosition, Raylib.GetMousePosition()) / (ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS * MathF.Sqrt(2));
        float intensity = 1.0f + 10.0f * MathF.Pow(MathF.Max(1.0f - distance, 0.0f), 3.0f);
        Raylib.SetShaderValue(cc3Shader, cc3ShaderTimeLoc, (float)Raylib.GetTime(), ShaderUniformDataType.SHADER_UNIFORM_FLOAT);
        Raylib.SetShaderValue(cc3Shader, cc3ShaderIntensityLoc, intensity, ShaderUniformDataType.SHADER_UNIFORM_FLOAT);

        Raylib.DrawModel(cc3Model, position, MODEL_SCALE, cc3Tints[(int)team]);
    }
}
