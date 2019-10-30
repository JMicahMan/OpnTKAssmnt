using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.IO;

namespace OpnTKAssmnt
{
    public class Shader
    {
        public readonly int Handle;

        private readonly Dictionary<string, int> UniformLocations;

        private static string LoadSource(string path)
        {
            using (var SR = new StreamReader(path, Encoding.UTF8))
            {
                return SR.ReadToEnd();
            }
        }

        private static void CompileShader(int Shader)
        {
            GL.CompileShader(Shader);

            GL.GetShader(Shader, ShaderParameter.CompileStatus, out var Code);
            if (Code != (int) All.True)
            {
                throw new Exception($"Error occurred while compiling Shader({Shader})");
            }
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var Code);
            if (Code != (int) All.True)
            {
                throw new Exception($"Error occured while linking Program({program})");
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string AtrribName)
        {
            return GL.GetAttribLocation(Handle, AtrribName);
        }

        public void SetInt(string Name, int Data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(UniformLocations[Name], Data);
        }

        public void SetFloat(string Name, float Data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(UniformLocations[Name], Data);
        }

        public void SetMatrix4(string Name, Matrix4 Data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(UniformLocations[Name], true, ref Data);
        }

        public void SetVector3(string Name, Vector3 Data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(UniformLocations[Name], Data);
        }
        public Shader(string vertPath, string fragPath)
        {
            var ShaderSource = LoadSource(vertPath);

            var VertextShader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(VertextShader, ShaderSource);

            CompileShader(VertextShader);

            ShaderSource = LoadSource(fragPath);
            var FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, ShaderSource);
            CompileShader(FragmentShader);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertextShader);
            GL.AttachShader(Handle, FragmentShader);

            LinkProgram(Handle);

            GL.DetachShader(Handle, VertextShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertextShader);

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var NumberOfUniforms);

            UniformLocations = new Dictionary<string, int>();

            for (var i = 0; i < NumberOfUniforms; i++)
            {
                var Key = GL.GetActiveUniform(Handle, i, out _, out _);
                var Location = GL.GetUniformLocation(Handle, Key);

                UniformLocations.Add(Key, Location);
            }
        }
    }
}
