using System;
using System.Drawing;

namespace CubeRenderer
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(600, 600, "Cube Renderer");
            window.Location = new Point(0, 0);
            window.Run(60.0);
        }
    }
}
