using System.Numerics;
using System.Runtime.InteropServices;

namespace CSharpAstralCanvasTest
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WorldViewProjection
    {
        public Matrix4x4 world;
        public Matrix4x4 view;
        public Matrix4x4 projection;

        public WorldViewProjection(Matrix4x4 world, Matrix4x4 view, Matrix4x4 projection)
        {
            this.world = world;
            this.view = view;
            this.projection = projection;
        }
    }
}
