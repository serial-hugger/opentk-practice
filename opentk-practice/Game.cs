using System.Drawing;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace opentk_practice;

public class Game : GameWindow
{
    private int vertexBufferHandle;
    private int shaderProgramHandle;
    private int vertexArrayHandle;
    public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        this.CenterWindow(new Vector2i(500,500));
    }

    protected override void OnLoad()
    {
        GL.ClearColor(Color.Blue);
        float[] vertices = new float[]
        {
            0.0f, 0.5f, 0f,
            0.5f, -0.5f, 0f,
            -0.5f, -0.5f, 0f
        };
        vertexBufferHandle = GL.GenBuffer();
        //Bind actual data to the buffer
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        
        vertexArrayHandle = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayHandle);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferHandle);
        GL.VertexAttribPointer(0,3, VertexAttribPointerType.Float, false, 3*sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        GL.BindVertexArray(0);

        string vertexShaderCode = File.ReadAllText("/home/jeremy/Documents/Projects/opentk-practice/opentk-practice/shader.vert");
        
        string fragShaderCode = File.ReadAllText("/home/jeremy/Documents/Projects/opentk-practice/opentk-practice/shader.frag");

        int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShaderHandle, vertexShaderCode);
        GL.CompileShader(vertexShaderHandle);
        
        int fragShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragShaderHandle, fragShaderCode);
        GL.CompileShader(fragShaderHandle);

        shaderProgramHandle = GL.CreateProgram();
        
        GL.AttachShader(shaderProgramHandle, vertexShaderHandle);
        GL.AttachShader(shaderProgramHandle, fragShaderHandle);
        
        GL.LinkProgram(shaderProgramHandle);
        
        GL.DetachShader(shaderProgramHandle, vertexShaderHandle);
        GL.DetachShader(shaderProgramHandle, fragShaderHandle);
        
        GL.DeleteShader(vertexShaderHandle);
        GL.DeleteShader(fragShaderHandle);
        
        base.OnLoad();
    }

    protected override void OnUnload()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.DeleteBuffer(vertexBufferHandle);
        
        GL.UseProgram(0);
        GL.DeleteProgram(shaderProgramHandle);
        base.OnUnload();
    }

    protected override void OnResize(ResizeEventArgs window)
    {
        GL.Viewport(0, 0, window.Width, window.Height);
        base.OnResize(window);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        GL.UseProgram(shaderProgramHandle);
        GL.BindVertexArray(vertexArrayHandle);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        
        this.Context.SwapBuffers();
        base.OnRenderFrame(args);
    }
}