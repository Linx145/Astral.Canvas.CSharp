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
        public bool fullscreen
        {
            get
            {
                return AstralCanvas.Window_GetIsFullscreen(handle);
            }
            set
            {
                AstralCanvas.Window_SetFullscreen(handle, value);
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
        public void SetOnKeyInteractCallback(AstralCanvas.OnKeyInteractedFunction func)
        {
            AstralCanvas.Window_SetOnKeyInteractCallback(handle, func);
        }
        public void SetOnTextInputCallback(AstralCanvas.OnTextInputFunction func)
        {
            AstralCanvas.Window_SetOnTextInputCallback(handle, func);
        }
        public void Close()
        {
            AstralCanvas.Window_CloseWindow(handle);
        }
    }
}
