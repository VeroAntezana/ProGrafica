// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;
using OpenTK;
using OpenTK.Graphics;

namespace ProGrafica
{

        class Program
        {
            static void Main(string[] args)
            {
            Console.WriteLine("Hola mundo");
            using (Game game = new Game()) { 
                game.Run();
            }
            }
        }
    
}
