using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using PixleFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace OpnTKAssmnt
{
    class Texture
    {
        public readonly int Handle;

        public void Use(TextureUnit Unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(Unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
        public Texture(string path)
        {

            Handle = GL.GenTexture();

            Use();

            using (var image = new Bitmap(path))
            {
                var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixleFormat.Bgra, PixelType.UnsignedByte, data.Scan0);


            }
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        }
        
        //Image<Rgba32> image = SixLabors.ImageSharp.Image.Load("Shades\fish.png");



        

    }
}
