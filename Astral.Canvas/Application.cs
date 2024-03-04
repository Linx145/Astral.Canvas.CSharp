using System;
using System.Runtime.InteropServices;

namespace Astral.Canvas
{
    public class Application
    {
        public static Application instance;
        public static LoggingMode loggingMode;
        public IntPtr handle;
        public int windowsCount;
        public Graphics graphicsDevice;

        public Application(string appName, string engineName, uint appVersion, uint engineVersion, float fps)
        {
            handle = AstralCanvas.Application_Init(appName, engineName, appVersion, engineVersion, fps);
            graphicsDevice = new Graphics(this);
            instance = this;
        }

        public string applicationName
        {
            get
            {
                return Marshal.PtrToStringAnsi(AstralCanvas.Application_GetApplicationName(handle));
            }
        }
        public string engineName
        {
            get
            {
                return Marshal.PtrToStringAnsi(AstralCanvas.Application_GetEngineName(handle));
            }
        }
        public float framesPerSecond
        {
            get
            {
                return AstralCanvas.Application_GetFramesPerSecond(handle);
            }
            set
            {
                AstralCanvas.Application_SetFramesPerSecond(handle, value);
            }
        }
        public unsafe void AddWindow(string name, int width, int height, bool resizeable, ReadOnlySpan<byte> iconData, uint iconWidth, uint iconHeight)
        {
            if (iconData == null)
            {
                AstralCanvas.Application_AddWindow(handle, name, width, height, resizeable, IntPtr.Zero, 0, 0);
            }
            else
            {
                fixed (byte* ptr = iconData)
                {
                    AstralCanvas.Application_AddWindow(handle, name, width, height, resizeable, (IntPtr)ptr, iconWidth, iconHeight);
                }
            }
            windowsCount += 1;
        }
        public Window GetWindow(int index)
        {
            if (index >= windowsCount)
            {
                throw new IndexOutOfRangeException();
            }
            return new Window(AstralCanvas.Application_GetWindow(handle, (uint)index));
        }
        public void ResetDeltaTimer()
        {
            AstralCanvas.Application_ResetDeltaTimer(handle);
        }
        public unsafe void Run(delegate* unmanaged<float, void> onUpdate, delegate* unmanaged<float, void> onDraw, delegate* unmanaged<float, void> postEndDraw, delegate* unmanaged<void> onProgramInitialize, delegate* unmanaged<void> onProgramEnd)
        {
            AstralCanvas.Application_Run(handle, onUpdate, onDraw, postEndDraw, onProgramInitialize, onProgramEnd);
        }
    }
}
