using Astral.Canvas;
using System.Numerics;
using System.Text;
using System.IO;

namespace CSharpAstralCanvasTest
{
    internal class DemoTexture
    {
        public static Application application;
        public static Texture2D texture;
        public static Shader shader;
        public static RenderPipeline renderPipeline;
        public static RenderProgram renderProgram;
        public static VertexBuffer vertexBuffer;
        public static IndexBuffer indexBuffer;
        public static SamplerState samplerState;
        public static Vector2 drawPos;

        static float fpsRecordTime = 0f;
        public static void Update(float deltaTime)
        {
            if (Input.IsKeyDown(Keys.A))
            {
                drawPos.X -= 256f * deltaTime;
            }
            if (Input.IsKeyDown(Keys.D))
            {
                drawPos.X += 256f * deltaTime;
            }
            if (Input.IsKeyDown(Keys.W))
            {
                drawPos.Y -= 256f * deltaTime;
            }
            if (Input.IsKeyDown(Keys.S))
            {
                drawPos.Y += 256f * deltaTime;
            }
            fpsRecordTime += deltaTime;
            if (fpsRecordTime >= 0.66f)
            {
                application.GetWindow(0).title = ((int)(1f / deltaTime)).ToString();
                fpsRecordTime -= 0.5f;
            }
        }
        public static void Draw(float deltaTime)
        {
            Point resolution = application.GetWindow(0).resolution;
            WorldViewProjection wvp = new WorldViewProjection(
                Matrix4x4.CreateTranslation(drawPos.X, drawPos.Y, 0f) * Matrix4x4.CreateScale(2f, 2f, 2f), 
                System.OperatingSystem.IsMacOS() ? Matrix4x4.CreateScale(1f, -1f, 1f) : Matrix4x4.Identity, 
                Matrix4x4.CreateOrthographic(1024, 768, 0f, 1000f)
                );

            application.graphicsDevice.StartRenderProgram(renderProgram, Color.Black);

            application.graphicsDevice.UseRenderPipeline(renderPipeline);

            application.graphicsDevice.SetVertexBuffer(vertexBuffer, 0);
            application.graphicsDevice.SetIndexBuffer(indexBuffer);
            application.graphicsDevice.SetShaderVariable("Matrices", wvp);
            application.graphicsDevice.SetShaderVariableTexture("inputTexture", texture);
            application.graphicsDevice.SetShaderVariableSampler("samplerState", samplerState);

            application.graphicsDevice.DrawIndexedPrimitives(6, 1);

            application.graphicsDevice.EndRenderProgram();
        }
        public static void Initialize()
        {
            samplerState = new SamplerState(SampleMode.Point, RepeatMode.ClampToEdgeColor, false, 0f);

            string[] allLines = File.ReadAllLines("Texture.shaderobj");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < allLines.Length; i++)
            {
                sb.AppendLine(allLines[i]);
            }
            shader = new Shader(ShaderType.VertexFragment, sb.ToString());

            renderPipeline = new RenderPipeline(shader, CullMode.CullNone, PrimitiveType.TriangleList, BlendState.AlphaBlend, false, false, new VertexDeclaration[] { VertexPositionColorTexture.Declaration });

            const float width = 26f;
            const float height = 19f;
            vertexBuffer = new VertexBuffer(VertexPositionColorTexture.Declaration, 4, false);
            vertexBuffer.SetData(new VertexPositionColorTexture[] {
                new VertexPositionColorTexture(new Vector3(-width * 0.5f, -height * 0.5f, 0f), new Vector4(1f, 1f, 1f, 1f), new Vector2(0f, 0f)),
                new VertexPositionColorTexture(new Vector3(width * 0.5f, -height * 0.5f, 0f), new Vector4(1f, 1f, 1f, 1f), new Vector2(1f, 0f)),
                new VertexPositionColorTexture(new Vector3(width * 0.5f, height * 0.5f, 0f), new Vector4(1f, 1f, 1f, 1f), new Vector2(1f, 1f)),
                new VertexPositionColorTexture(new Vector3(-width * 0.5f, height * 0.5f, 0f), new Vector4(1f, 1f, 1f, 1f), new Vector2(0f, 1f))
            });

            indexBuffer = new IndexBuffer(IndexBufferSize.UInt16, 6);
            indexBuffer.SetData(new ushort[] { 0, 1, 2, 3, 0, 2 });

            renderProgram = new RenderProgram();
            int colorAttachment = renderProgram.AddAttachment(ImageFormat.BackbufferFormat, true, false, RenderPassOutputType.ToWindow);
            int depthAttachment = renderProgram.AddAttachment(ImageFormat.Depth32, false, true, RenderPassOutputType.ToWindow);
            renderProgram.AddRenderPass(colorAttachment, depthAttachment);
            renderProgram.Construct();

            texture = Texture2D.FromFile("tbh.png");
        }
        public static void Unload()
        {
            renderProgram.Dispose();

            indexBuffer.Dispose();

            vertexBuffer.Dispose();

            texture.Dispose();

            shader.Dispose();

            renderPipeline.Dispose();

            samplerState.Dispose();
        }
        public static void Start()
        {
            application = new Application("YIPPEE", "", 0, 0, 0f);
            application.AddWindow(1024, 768, true);
            application.Run(Update, Draw, Initialize, Unload);
        }
    }
}
