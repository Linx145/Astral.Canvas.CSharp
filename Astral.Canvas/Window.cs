using System;

namespace Astral.Canvas
{
    public class Window
    {
        public IntPtr handle;
        private string internalTitle;

        public Window()
        {
            handle = IntPtr.Zero;
        }
        public Window(IntPtr handle)
        {
            this.handle = handle; 
        }

        public Point resolution
        {
            get
            {
                return AstralCanvas.Window_GetResolution(handle);
            }
            set
            {
                AstralCanvas.Window_SetResolution(handle, value);
            }
        }
        public Point position
        {
            get
            {
                return AstralCanvas.Window_GetPosition(handle);
            }
            set
            {
                AstralCanvas.Window_SetPosition(handle, value);
            }
        }
        public string title
        {
            get
            {
                return internalTitle;
            }
            set
            {
                AstralCanvas.Window_SetTitle(handle, value);
                internalTitle = value;
            }
        }
    }
}
