using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Raylib_cs;

namespace RLChess;

internal class Sprites
{
    public Sprites()
    {
        tex = new Texture2D[filenames.Length];
        models = new Model[filenames.Length];
        hoveredModels = new Model[filenames.Length];

        displacementShader = Raylib.LoadShader("resources/displacement.vert", null);
        displacementTimeLoc = Raylib.GetShaderLocation(displacementShader, "time");

        hoveredDisplacementShader = Raylib.LoadShader("resources/displacement.vert", null);
        hoveredDisplacementTimeLoc = Raylib.GetShaderLocation(hoveredDisplacementShader, "time");
        displacementTimeLoc = Raylib.GetShaderLocation(displacementShader, "intensity");

        cam = new(
            position: Vector3.UnitZ * 100,
            target: Vector3.Zero,
            up: Vector3.UnitY,
            fovy: ChessConstants.NUM_OUTPUT_BOARD_SIDE_PIXELS,
            projection: CameraProjection.CAMERA_ORTHOGRAPHIC
        );

        for (int i = 0; i < filenames.Length; ++i)
        {
            tex[i] = Raylib.LoadTexture("resources/" + filenames[i] + ".png");

            // Models
            {
                models[i] = Raylib.LoadModel("resources/" + filenames[i] + ".obj");

                int numMaterials = models[i].materialCount;
                for (int j = 0; j < numMaterials; ++j)
                {
                    Raylib.SetMaterialShader(ref models[i], j, ref displacementShader);
                }
            }

            // Hovered models
            {
                hoveredModels[i] = Raylib.LoadModel("resources/" + filenames[i] + ".obj");

                int numMaterials = hoveredModels[i].materialCount;
                for (int j = 0; j < numMaterials; ++j)
                {
                    Raylib.SetMaterialShader(ref hoveredModels[i], j, ref hoveredDisplacementShader);
                }
            }
        }
    }

    ~Sprites()
    {
        for (int i = 0; i < tex.Length; ++i)
        {
            Raylib.UnloadTexture(tex[i]);
            Raylib.UnloadModel(models[i]);
            Raylib.UnloadModel(hoveredModels[i]);
        }
        Raylib.UnloadShader(displacementShader);
        Raylib.UnloadShader(hoveredDisplacementShader);
    }

    public enum SpriteID
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King,
    }

    public enum SpriteStyle
    {
        /// <summary>
        /// CChess 2 - Raster
        /// </summary>
        CC2,

        /// <summary>
        /// CChess 3 - Vector
        /// </summary>
        CC3,
    }

    readonly string[] filenames = {
        "pawn",
        "rook",
        "knight",
        "bishop",
        "queen",
        "king",
    };

    readonly private Texture2D[] tex = Array.Empty<Texture2D>();
    readonly private Model[] models = Array.Empty<Model>();
    readonly private Model[] hoveredModels = Array.Empty<Model>();

    private Shader displacementShader;
    readonly private int displacementTimeLoc;

    private Shader hoveredDisplacementShader;
    readonly private int hoveredDisplacementTimeLoc;

    private Camera3D cam;

    const float NUM_MODEL_SIDE_UNITS = 50.0f;
    const float NUM_OUTPUT_BOARD_SIDE_PIXELS_HALF = ChessConstants.NUM_OUTPUT_BOARD_SIDE_PIXELS * 0.5f;
    const float modelScale = ChessConstants.NUM_OUTPUT_TILE_SIDE_PIXELS / NUM_MODEL_SIDE_UNITS;

    public void BeginModeCC3() => Raylib.BeginMode3D(cam);
    public static void EndModeCC3() => Raylib.EndMode3D();

    /// <summary>
    /// Draws the CChess2 (raster) version of a sprite.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    public void DrawCC2(SpriteID id, Vector2 position, Color tint) =>
        Raylib.DrawTextureEx(tex[(int)id], position, 0.0f, ChessConstants.OUTPUT_SCALE, tint);

    /// <summary>
    /// Snaps sprite to the tile.
    /// </summary>
    public void DrawCC2Snapped(SpriteID id, int x, int y, Color tint) =>
        DrawCC2(id, Board.TileToOutputPixel(x, y), tint);

    /// <summary>
    /// Draws the CChess3 (vector) version of a sprite.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    public void DrawCC3(SpriteID id, Vector2 position, bool hovered, Color tint)
    {
        Vector3 position3 = new(
            position.X - NUM_OUTPUT_BOARD_SIDE_PIXELS_HALF,
            -position.Y + NUM_OUTPUT_BOARD_SIDE_PIXELS_HALF,
            0.0f
        );

        Model model = hovered ? hoveredModels[(int)id] : models[(int)id];
        
        Raylib.DrawModel(model, position3, modelScale, tint);
    }

    /// <summary>
    /// Snaps sprite to the tile.
    /// </summary>
    public void DrawCC3Snapped(SpriteID id, int x, int y, bool hovered, Color tint) =>
        DrawCC3(id, Board.TileToOutputPixel(x, y), hovered, tint);

    /// <summary>
    /// Updates the displacement animation.
    /// </summary>
    public void Tick()
    {
        float time = (float)Raylib.GetTime();

        Raylib.SetShaderValue(
            displacementShader,
            displacementTimeLoc,
            time,
            ShaderUniformDataType.SHADER_UNIFORM_FLOAT);

        Raylib.SetShaderValue(
            hoveredDisplacementShader,
            hoveredDisplacementTimeLoc,
            time,
            ShaderUniformDataType.SHADER_UNIFORM_FLOAT);
    }
}