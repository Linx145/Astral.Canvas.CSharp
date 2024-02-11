using System;

namespace Astral.Canvas
{
    public struct RenderPass
    {
        public IntPtr arrayMemberHandle;
        public RenderPass(IntPtr arrayMemberHandle)
        {
            this.arrayMemberHandle = arrayMemberHandle;
        }
        public RenderPass AddInput(int input)
        {
            AstralCanvas.RenderPass_AddInput(arrayMemberHandle, input);
            return this;
        }
        public unsafe void GetColorAttachmentIndices(Span<int> result)
        {
            UIntPtr count;
            AstralCanvas.RenderPass_GetColorAttachments(arrayMemberHandle, null, &count);

            if ((int)count > 0)
            {
                fixed (int* ptr = &result[0])
                {
                    AstralCanvas.RenderPass_GetColorAttachments(arrayMemberHandle, ptr, &count);
                }
            }
        }
        public unsafe int[] GetColorAttachmentIndices()
        {
            int[] result;
            UIntPtr count;
            AstralCanvas.RenderPass_GetColorAttachments(arrayMemberHandle, null, &count);

            if ((int)count > 0)
            {
                result = new int[(int)count];
                fixed (int* ptr = &result[0])
                {
                    AstralCanvas.RenderPass_GetColorAttachments(arrayMemberHandle, ptr, &count);
                }
                return result;
            }
            return null;
        }
        public int GetDepthAttachmentIndex()
        {
            return AstralCanvas.RenderPass_GetDepthAttachments(arrayMemberHandle);
        }
    }

    public class RenderProgram
    {
        public IntPtr handle;

        public RenderProgram()
        {
            handle = AstralCanvas.RenderProgram_Init();
        }
        public int AddAttachment(ImageFormat imageFormat, bool clearColor, bool clearDepth, RenderPassOutputType outputType)
        {
            return AstralCanvas.RenderProgram_AddAttachment(handle, imageFormat, clearColor, clearDepth, outputType);
        }
        public RenderPass GetRenderPass(int index)
        {
            return new RenderPass(AstralCanvas.RenderProgram_GetRenderPass(handle, (UIntPtr)index));
        }
        public RenderPass AddRenderPass(int colorAttachmentIndex, int depthAttachmentIndex = -1)
        {
            return new RenderPass(AstralCanvas.RenderProgram_AddRenderPass(handle, colorAttachmentIndex, depthAttachmentIndex));
        }
        public unsafe RenderPass AddRenderPasses(ReadOnlySpan<int> colorAttachmentIndex, int depthAttachmentIndex = -1)
        {
            fixed (int* ptr = colorAttachmentIndex)
            {
                return new RenderPass(AstralCanvas.RenderProgram_AddRenderPasses(handle, (IntPtr)ptr, (UIntPtr)colorAttachmentIndex.Length, depthAttachmentIndex));
            }
        }
        public void Construct()
        {
            AstralCanvas.RenderProgram_Construct(handle);
        }
        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                AstralCanvas.RenderProgram_Deinit(handle);
            }
            handle = IntPtr.Zero;
        }
    }
}
