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
        tex = new Texture2D[filenames.Length];
        for (int i = 0; i < filenames.Length; ++i)
        {
            tex[i] = Raylib.LoadTexture(filenames[i]);
        }
    }

    ~Sprites()
    {
        for (int i = 0; i < tex.Length; ++i)
        {
            Raylib.UnloadTexture(tex[i]);
        }
    }

    public enum SpriteID
    {
        Pawn = 0,
        Rook,
        Knight,
        Bishop,
        Queen,
        King,
    }

    public enum UnitSpriteID
    {
        Pawn   = SpriteID.Pawn,
        Rook   = SpriteID.Rook,
        Knight = SpriteID.Knight,
        Bishop = SpriteID.Bishop,
        Queen  = SpriteID.Queen,
        King   = SpriteID.King,
    }

    readonly string[] filenames = {
          "pawn.png",
          "rook.png",
        "knight.png",
        "bishop.png",
         "queen.png",
          "king.png",
    };

    /// <summary>
    /// Textures are grayscale and can be tinted
    /// </summary>
    readonly private Texture2D[] tex;

    Texture2D this[SpriteID id] => tex[(int)id];

    public void Draw(SpriteID id, Vector2 position, Color tint) =>
        Raylib.DrawTextureEx(this[id], position, 0.0f, ChessConstants.OUTPUT_SCALE, tint);

    /// <summary>
    /// Snaps sprite to the tile.
    /// </summary>
    public void DrawSnapped(SpriteID id, int x, int y, Color tint) =>
        Draw(id, Board.TileToOutputPixel(x, y), tint);
}
