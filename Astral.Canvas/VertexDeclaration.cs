using System;
using System.Runtime.InteropServices;

namespace Astral.Canvas
{
    [StructLayout(LayoutKind.Explicit)]
    public struct VertexElement
    {
        [FieldOffset(0)]
        public VertexElementFormat format;
        [FieldOffset(4)]
        public UIntPtr offset;

        public VertexElement(VertexElementFormat format, uint offset)
        {
            this.format = format;
            this.offset = (UIntPtr)offset;
        }
    }
    public class VertexDeclaration
    {
        public IntPtr handle;
        public VertexElement[] elements;
        public uint size;
        public VertexInputRate inputRate;
        public object userDefinedMetadata;

        public VertexDeclaration(uint size, VertexInputRate inputRate, params VertexElement[] elements)
        {
            handle = AstralCanvas.VertexDeclaration_Create((UIntPtr)size, inputRate);
            for (int i = 0; i < elements.Length; i++)
            {
                AstralCanvas.VertexDeclaration_AddElement(handle, elements[i]);
            }
            this.elements = elements;
            this.inputRate = inputRate;
            this.size = size;
        }
        public VertexDeclaration(IntPtr handle, uint size, params VertexElement[] elements)
        {
            this.handle = handle;
            this.size = size;
            this.elements = elements;
            userDefinedMetadata = null;
        }
        public unsafe VertexDeclaration(IntPtr handle)
        {
            this.handle = handle;
            this.size = (uint)AstralCanvas.VertexDeclaration_GetElementSize(handle);
            this.inputRate = AstralCanvas.VertexDeclaration_GetInputRate(handle);
            userDefinedMetadata = null;

            UIntPtr outputCount = UIntPtr.Zero;
            AstralCanvas.VertexDeclaration_GetElements(handle, &outputCount, null);

            elements = new VertexElement[(int)outputCount];
            fixed (VertexElement* ptr = elements)
            {
                AstralCanvas.VertexDeclaration_GetElements(handle, &outputCount, ptr);
            }
        }
    }
}
