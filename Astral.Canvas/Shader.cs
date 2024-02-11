using System;

namespace Astral.Canvas
{
    public struct ShaderVariable
    {
        public IntPtr handle;
        public ShaderVariable(IntPtr handle)
        {
            this.handle = handle;
        }
        public ShaderResourceType type => AstralCanvas.ShaderVariable_GetType(handle);
        public ShaderInputAccessedBy accessedBy => AstralCanvas.ShaderVariable_GetAccessedBy(handle);
        public uint size => AstralCanvas.ShaderVariable_GetSize(handle);
        public uint arrayLength => AstralCanvas.ShaderVariable_GetArrayLength(handle);
        public uint set => AstralCanvas.ShaderVariable_GetSet(handle);
        public uint binding => AstralCanvas.ShaderVariable_GetBinding(handle);
        public string name => AstralCanvas.ShaderVariable_GetName(handle);
    }
    public class Shader : IDisposable
    {
        public IntPtr handle;

        public Shader()
        {
            handle = IntPtr.Zero;
        }
        public unsafe Shader(ShaderType shaderType, string jsonString)
        {
            IntPtr temp = IntPtr.Zero;
            int result = AstralCanvas.Shader_FromString(shaderType, jsonString, &temp);
            if (result != 0 || temp == IntPtr.Zero)
            {
                throw new InvalidOperationException("Error loading shader with error code " + result.ToString());
            }

            handle = temp;
        }
        public IntPtr ShaderModule1
        {
            get
            {
                return AstralCanvas.Shader_GetModule1(handle);
            }
        }
        public IntPtr ShaderModule2
        {
            get
            {
                return AstralCanvas.Shader_GetModule2(handle);
            }
        }
        public IntPtr ShaderPipelineLayout
        {
            get
            {
                return AstralCanvas.Shader_GetPipelineLayout(handle);
            }
        }
        public ShaderType type
        {
            get
            {
                return AstralCanvas.Shader_GetType(handle);
            }
        }
        public IntPtr pipelineLayout
        {
            get
            {
                return AstralCanvas.Shader_GetPipelineLayout(handle);
            }
        }
        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                AstralCanvas.Shader_Deinit(handle);
            }
            handle = IntPtr.Zero;
        }
        public int GetVariableBindings(string variableName)
        {
            return AstralCanvas.Shader_GetVariableBinding(handle, variableName);
        }
        public ShaderVariable GetVariableAt(int index)
        {
            return new ShaderVariable(AstralCanvas.Shader_GetVariableAt(handle, (UIntPtr)index));
        }
    }
}
