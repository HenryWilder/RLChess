using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

    public Sprites.SpriteID SpriteID => type switch
    {
        Type.Pawn   => Sprites.SpriteID.Pawn,
        Type.Rook   => Sprites.SpriteID.Rook,
        Type.Knight => Sprites.SpriteID.Knight,
        Type.Bishop => Sprites.SpriteID.Bishop,
        Type.Queen  => Sprites.SpriteID.Queen,
        Type.King   => Sprites.SpriteID.King,
        _ => throw new InvalidEnumArgumentException(),
    };

    public Unit(int x, int y, Type type, Team team)
    {
        this.x = x;
        this.y = y;
        this.type = type;
        this.team = team;
    }

    public Type type;
    public Team team;
    public int x, y;

    public void Draw(in Sprites sp)
    {
        sp.DrawSnapped(SpriteID, x, y, team switch
        {
            Team.White => Color.WHITE,
            Team.Black => Color.GRAY,
            _ => throw new InvalidEnumArgumentException(),
        });
    }
}
