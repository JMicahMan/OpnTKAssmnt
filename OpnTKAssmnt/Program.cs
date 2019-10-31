using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Windows;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;


namespace OpnTKAssmnt
{
    public static class Program
    {
        private static void Main()
        {
            using (var window = new Window(800, 600, "OpenTKAssmnt"))
            {
                window.Run(60.0);
            }
        }
    }
}

