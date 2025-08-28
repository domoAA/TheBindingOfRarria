using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheBindingOfRarria.Common;

public struct VertexInfo : IVertexType
{
    private static readonly VertexDeclaration _vertexDeclaration = new
    ([
        new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
    ]);

    public Vector2 Position;
    public Color Color;
    public Vector3 TexCoord;

    public readonly VertexDeclaration VertexDeclaration => _vertexDeclaration;

    public override readonly string ToString() => $"position: {Position}, color: {Color}, texCoord: {TexCoord}";

    public VertexInfo(Vector2 position, Vector3 texCoord, Color color)
    {
        Position = position;
        TexCoord = texCoord;
        Color = color;
    }

    public VertexInfo(Vector3 position, Vector3 texCoord, Color color)
    {
        Position = new Vector2(position.X, position.Y);
        TexCoord = texCoord;
        Color = color;
    }

    public VertexInfo(Vector2 position, Color color, Vector3 texCoord)
    {
        Position = position;
        TexCoord = texCoord;
        Color = color;
    }

    public VertexInfo(Vector2 position, Vector3 texCoord)
    {
        Position = position;
        TexCoord = texCoord;
        Color = Color.White;
    }
}