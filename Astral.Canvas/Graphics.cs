using System;

namespace Astral.Canvas
{
    public class Graphics
    {
        public IntPtr handle;
        public RenderTarget currentRenderTarget;
        public RenderProgram currentRenderProgram;
        public int currentRenderPass;

        public Graphics(IntPtr handle)
        {
            this.handle = handle;
            currentRenderTarget = null;
            currentRenderProgram = null;
            currentRenderPass = 0;
        }
        public Graphics(Application application)
        {
            handle = AstralCanvas.Application_GetGraphicsDevice(application.handle);
        }

        public void StartRenderProgram(RenderProgram program, Color clearColor)
        {
            AstralCanvas.Graphics_StartRenderProgram(handle, program.handle, clearColor);
            currentRenderProgram = program;
        }
        public void NextRenderPass()
        {
            currentRenderPass++;
            AstralCanvas.Graphics_NextRenderPass(handle);
        }
        public void EndRenderProgram()
        {
            currentRenderPass = 0;
            AstralCanvas.Graphics_EndRenderProgram(handle);
            currentRenderProgram = null;
        }

        public void UseRenderPipeline(RenderPipeline pipeline)
        {
            AstralCanvas.Graphics_UseRenderPipeline(handle, pipeline.handle);
        }

        public void SetRenderTarget(RenderTarget renderTarget)
        {
            currentRenderTarget = renderTarget;
            if (renderTarget == null)
            {
                AstralCanvas.Graphics_SetRenderTarget(handle, IntPtr.Zero);
            }
            else AstralCanvas.Graphics_SetRenderTarget(handle, renderTarget.handle);
        }
        public void SetVertexBuffer(VertexBuffer vertexBuffer, uint bindSlot)
        {
            AstralCanvas.Graphics_SetVertexBuffer(handle, vertexBuffer.handle, bindSlot);
        }
        public void SetInstanceBuffer(InstanceBuffer instanceBuffer, uint bindSlot)
        {
            AstralCanvas.Graphics_SetInstanceBuffer(handle, instanceBuffer.handle, bindSlot);
        }
        public void SetIndexBuffer(IndexBuffer indexBuffer)
        {
            AstralCanvas.Graphics_SetIndexBuffer(handle, indexBuffer.handle);
        }
        public void DrawIndexedPrimitives(uint indexCount, uint instanceCount, uint firstIndex = 0, uint vertexOffset = 0, uint firstInstance = 0)
        {
            AstralCanvas.Graphics_DrawIndexedPrimitives(handle, indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
        }
        public Rectangle GetClipArea()
        {
            return AstralCanvas.Graphics_GetClipArea(handle);
        }
        public void SetClipArea(Rectangle rectangle)
        {
            AstralCanvas.Graphics_SetClipArea(handle, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
        public Rectangle GetViewport()
        {
            return AstralCanvas.Graphics_GetViewport(handle);
        }
        public void SetViewport(Rectangle rectangle)
        {
            AstralCanvas.Graphics_SetViewport(handle, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
        public unsafe void SetShaderVariables<T>(string variableName, ReadOnlySpan<T> span) where T : unmanaged
        {
            fixed (T* ptr = span)
            {
                AstralCanvas.Graphics_SetShaderVariable(handle, variableName, (IntPtr)ptr, (UIntPtr)span.Length);
            }
        }
        public unsafe void SetShaderVariable<T>(string variableName, T data) where T : unmanaged
        {
            T* ptr = &data;
            AstralCanvas.Graphics_SetShaderVariable(handle, variableName, (IntPtr)ptr, (UIntPtr)sizeof(T));
        }
        public unsafe void SetShaderVariableTexture(string variableName, Texture2D texture)
        {
            AstralCanvas.Graphics_SetShaderVariableTexture(handle, variableName, texture.handle);
        }
        public unsafe void SetShaderVariableTextures(string variableName, ReadOnlySpan<Texture2D> textures)
        {
            IntPtr* ptrs = stackalloc IntPtr[textures.Length];
            for (int i = 0; i < textures.Length; i++)
            {
                ptrs[i] = textures[i].handle;
            }
            AstralCanvas.Graphics_SetShaderVariableTextures(handle, variableName, (IntPtr)ptrs, (UIntPtr)textures.Length);
        }
        public unsafe void SetShaderVariableSampler(string variableName, SamplerState sampler)
        {
            AstralCanvas.Graphics_SetShaderVariableSampler(handle, variableName, sampler.handle);
        }
        public unsafe void SetShaderVariableSamplers(string variableName, ReadOnlySpan<SamplerState> samplers)
        {
            IntPtr* ptrs = stackalloc IntPtr[samplers.Length];
            for (int i = 0; i < samplers.Length; i++)
            {
                ptrs[i] = samplers[i].handle;
            }
            AstralCanvas.Graphics_SetShaderVariableSamplers(handle, variableName, (IntPtr)ptrs, (UIntPtr)samplers.Length);
        }
        public void AwaitGraphicsIdle()
        {
            AstralCanvas.Graphics_AwaitGraphicsIdle(handle);
        }
    }
}
