using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Raylib_cs;

namespace RLChess;

internal class Sprites
{
    public Sprites()
    {
        tex    = new Texture2D[filenames.Length];
        for (int i = 0; i < filenames.Length; ++i)
        {
               tex[i] = Raylib.LoadTexture("resources/" + filenames[i] + ".png");
        }

        //meshes = new Mesh[filenames.Length];
        //for (int i = 0; i < filenames.Length; ++i)
        //{
        //    // filenames[0] + ".cc3asset";
        //    meshes[i] = Raylib.GenMeshCube(10,10,10);
        //}
    }

    ~Sprites()
    {
        for (int i = 0; i < tex.Length; ++i)
        {
            Raylib.UnloadTexture(tex[i]);
            //Raylib.UnloadMesh(ref meshes[i]);
        }
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

    /// <summary>
    /// Textures are grayscale and can be tinted
    /// </summary>
    readonly private Texture2D[] tex;

    /// <summary>
    /// Textures are grayscale and can be tinted
    /// </summary>
    readonly private Mesh[] meshes;

    /// <summary>
    /// Draws the CChess2 (raster) version of a sprite.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    public void DrawCC2(SpriteID id, Vector2 position) =>
        Raylib.DrawTextureEx(tex[(int)id], position, 0.0f, ChessConstants.OUTPUT_SCALE, Color.WHITE);

    /// <summary>
    /// Snaps sprite to the tile.
    /// </summary>
    public void DrawCC2Snapped(SpriteID id, int x, int y) =>
        DrawCC2(id, Board.TileToOutputPixel(x, y));

    /// <summary>
    /// Draws the CChess3 (vector) version of a sprite.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    public void DrawCC3(SpriteID id, Vector2 position)
    {
        Material mat = Raylib.LoadMaterialDefault();
        Raylib.DrawMesh(meshes[(int)id], mat, Raymath.MatrixTranslate(position.X, position.Y, 0.0f));
        Raylib.UnloadMaterial(mat);
    }

    /// <summary>
    /// Snaps sprite to the tile.
    /// </summary>
    public void DrawCC3Snapped(SpriteID id, int x, int y) =>
        DrawCC3(id, Board.TileToOutputPixel(x, y));
}
