using Astral.Canvas;
using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace CSharpAstralCanvasTest
{
    public static class DemoTriangle
    {
        public static Application application;
        public static Shader shader;
        public static RenderPipeline renderPipeline;
        public static RenderProgram renderProgram;
        public static VertexBuffer vertexBuffer;
        public static IndexBuffer indexBuffer;
        public static void Update(float deltaTime)
        {

        }
        public static void Draw(float deltaTime)
        {
            application.graphicsDevice.StartRenderProgram(renderProgram, Color.Black);

            application.graphicsDevice.UseRenderPipeline(renderPipeline);

            application.graphicsDevice.SetVertexBuffer(vertexBuffer, 0);
            application.graphicsDevice.SetIndexBuffer(indexBuffer);

            application.graphicsDevice.DrawIndexedPrimitives(3, 1);

            application.graphicsDevice.EndRenderProgram();
        }
        public static void Initialize()
        {
            string[] allLines = File.ReadAllLines("Triangle.shaderobj");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < allLines.Length; i++)
            {
                sb.AppendLine(allLines[i]);
            }
            shader = new Shader(ShaderType.VertexFragment, sb.ToString());

            renderPipeline = new RenderPipeline(shader, CullMode.CullNone, PrimitiveType.TriangleList, BlendState.AlphaBlend, false, false, new VertexDeclaration[] { VertexPositionColor.Declaration });

            vertexBuffer = new VertexBuffer(VertexPositionColor.Declaration, 3, false);
            vertexBuffer.SetData<VertexPositionColor>(new VertexPositionColor[] {
                new VertexPositionColor(new Vector3(-1f, 1f, 0f), new Vector4(1f, 0f, 0f, 1f)),
                new VertexPositionColor(new Vector3(0f, -1f, 0f), new Vector4(0f, 1f, 0f, 1f)),
                new VertexPositionColor(new Vector3(1f, 1f, 0f), new Vector4(0f, 0f, 1f, 1f)),
            });

            indexBuffer = new IndexBuffer(IndexBufferSize.UInt16, 3);
            indexBuffer.SetData(new ushort[] { 0, 1, 2 });

            renderProgram = new RenderProgram();
            int colorAttachment = renderProgram.AddAttachment(ImageFormat.BackbufferFormat, true, false, RenderPassOutputType.ToWindow);
            int depthAttachment = renderProgram.AddAttachment(ImageFormat.Depth32, false, true, RenderPassOutputType.ToWindow);
            renderProgram.AddRenderPass(colorAttachment, depthAttachment);
            renderProgram.Construct();
        }
        public static void Unload()
        {
            renderProgram.Dispose();

            indexBuffer.Dispose();

            vertexBuffer.Dispose();

            renderPipeline.Dispose();

            shader.Dispose();
        }
        public static void Start()
        {
            application = new Application("Hello World", "", 0, 0, 0f);
            application.AddWindow(1920, 1080, true);
            application.Run(Update, Draw, Initialize, Unload);
        }
    }
}
