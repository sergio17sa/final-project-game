using System;

public struct TilePosition 
{
    public int x;
    public int z;

    public TilePosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override string ToString()
    {
        return $"x: {x}; z: {z}";
    }
}
