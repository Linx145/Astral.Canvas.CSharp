using System;

namespace Astral.Canvas
{
    public class InstanceBuffer : IDisposable
    {
        public IntPtr handle;
        public int instanceCount;
        public int instanceSize;

        public InstanceBuffer(int instanceSize, int instanceCount, bool canRead = false)
        {
            handle = AstralCanvas.InstanceBuffer_Create((UIntPtr)instanceSize, (UIntPtr)instanceCount, canRead);
            this.instanceCount = instanceCount;
            this.instanceSize = instanceSize;
        }
        public unsafe void SetData<T>(ReadOnlySpan<T> vertices) where T : unmanaged
        {
            fixed (T* ptr = vertices)
            {
                AstralCanvas.InstanceBuffer_SetData(handle, (IntPtr)ptr, (UIntPtr)vertices.Length);
            }
        }
        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                AstralCanvas.InstanceBuffer_Deinit(handle);
            }
            handle = IntPtr.Zero;
        }
    }
}
