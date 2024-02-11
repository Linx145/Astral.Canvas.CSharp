using System;

namespace Astral.Canvas
{
    public class RenderTarget : IDisposable
    {
        public IntPtr handle;
        public uint width;
        public uint height;
        public bool isBackbuffer;
        public bool hasBeenUsedBefore
        {
            get
            {
                return AstralCanvas.RenderTarget_GetUseStatus(handle);
            }
            set
            {
                AstralCanvas.RenderTarget_SetUseStatus(handle, value);
            }
        }
        /// <summary>
        /// Retrieves the bound texture at the specified slot. Warning: This function generates garbage as Texture2D is a class, so you should store the Texture2D result!
        /// </summary>
        /// <param name="slot">The slot of the texture that you intend to retrieve</param>
        /// <returns></returns>
        public Texture2D GetTextureInSlot(uint slot)
        {
            return new Texture2D(AstralCanvas.RenderTarget_GetTexture(handle, (UIntPtr)slot));
        }
        public unsafe RenderTarget(uint width, uint height, params Texture2D[] texturesToUse)
        {
            IntPtr *ptrs = stackalloc IntPtr[texturesToUse.Length];
            for (int i = 0; i < texturesToUse.Length; i++)
            {
                ptrs[i] = texturesToUse[i].handle;
                texturesToUse[i].handle = IntPtr.Zero;
            }
            this.handle = AstralCanvas.RenderTarget_CreateWithInputTextures(width, height, ptrs, (UIntPtr)texturesToUse.Length);
            this.width = width;
            this.height = height;
            this.isBackbuffer = false;
        }
        public RenderTarget(Texture2D backendTexture, Texture2D depthBuffer, bool isBackbuffer)
        {
            this.handle = AstralCanvas.RenderTarget_CreateFromTextures(backendTexture.handle, depthBuffer == null ? IntPtr.Zero : depthBuffer.handle, isBackbuffer);
            this.width = backendTexture.width;
            this.height = backendTexture.height;
            this.isBackbuffer = isBackbuffer;
        }
        public RenderTarget(uint width, uint height, ImageFormat imageFormat, ImageFormat depthFormat)
        {
            this.handle = AstralCanvas.RenderTarget_Create(width, height, imageFormat, depthFormat);
            this.width = width;
            this.height = height;
            this.isBackbuffer = false;
        }

        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                //also deinits backend texture and depth buffer
                AstralCanvas.RenderTarget_Deinit(handle);
            }
            handle = IntPtr.Zero;
        }
    }
}
