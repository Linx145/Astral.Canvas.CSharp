using System;

namespace Astral.Canvas
{
    public class Texture2D : IDisposable
    {
        public IntPtr handle { get; internal set; }
        public readonly uint width;
        public readonly uint height;
        public readonly bool storesData;
        public readonly bool ownsHandle;
        public readonly bool usedForRenderTarget;
        public IntPtr imageHandle
        {
            get
            {
                return AstralCanvas.Texture2D_GetImageHandle(handle);
            }
        }
        public IntPtr imageView
        {
            get
            {
                return AstralCanvas.Texture2D_GetImageView(handle);
            }
        }
        public ImageFormat imageFormat
        {
            get
            {
                return AstralCanvas.Texture2D_GetImageFormat(handle);
            }
        }

        public Texture2D(IntPtr handle)
        {
            this.handle = handle;
            if (handle == IntPtr.Zero)
            {

            }
            else
            {
                this.width = AstralCanvas.Texture2D_GetWidth(handle);
                this.height = AstralCanvas.Texture2D_GetHeight(handle);
                this.storesData = AstralCanvas.Texture2D_StoreData(handle);
                this.ownsHandle = AstralCanvas.Texture2D_OwnsHandle(handle);
                this.usedForRenderTarget = AstralCanvas.Texture2D_UsedForRenderTarget(handle);
            }
        }
        public Texture2D(IntPtr handle,  uint width, uint height, bool storesData, bool ownsHandle, bool usedForRenderTarget)
        {
            this.handle = handle;
            this.width = width;
            this.height = height;
            this.storesData = storesData;
            this.ownsHandle = ownsHandle;
            this.usedForRenderTarget = usedForRenderTarget;
        }
        public unsafe ReadOnlySpan<T> GetData<T>() where T : unmanaged
        {
            void *ptr = AstralCanvas.Texture2D_GetData(handle);
            return new ReadOnlySpan<T>(ptr, (int)((width * height * 4) / sizeof(T)));
        }
        public unsafe ReadOnlySpan<T> RetrieveCurrentData<T>() where T : unmanaged
        {
            void* ptr = AstralCanvas.Texture2D_RetrieveCurrentData(handle);
            return new ReadOnlySpan<T>(ptr, (int)((width * height * 4) / sizeof(T)));
        }

        public static Texture2D FromHandle(IntPtr imageHandle, uint width, uint height, ImageFormat imageFormat, bool usedForRenderTarget)
        {
            IntPtr textureHandle = AstralCanvas.Texture2D_FromHandle(imageHandle, width, height, imageFormat, usedForRenderTarget);
            return new Texture2D(textureHandle, width, height, false, false, usedForRenderTarget);
        }
        public static unsafe Texture2D FromData(byte[] bytes, uint width, uint height, ImageFormat imageFormat, bool usedForRenderTarget, bool storeData = false)
        {
            if (bytes == null)
            {
                return new Texture2D(AstralCanvas.Texture2D_FromData(IntPtr.Zero, width, height, imageFormat, usedForRenderTarget, storeData), width, height, false, true, usedForRenderTarget);
            }
            fixed (byte* fixedBytes = &bytes[0])
            {
                IntPtr textureHandle = AstralCanvas.Texture2D_FromData((IntPtr)fixedBytes, width, height, imageFormat, usedForRenderTarget, storeData);
                return new Texture2D(textureHandle, width, height, false, true, usedForRenderTarget);
            }
        }
        public static Texture2D FromFile(string fileName, bool storeData = false)
        {
            IntPtr textureHandle = AstralCanvas.Texture2D_FromFile(fileName, storeData);
            return new Texture2D(textureHandle);
        }

        public void Dispose()
        {
            if (handle != IntPtr.Zero)
                AstralCanvas.Texture2D_Deinit(handle);
            handle = IntPtr.Zero;
        }
    }
}
