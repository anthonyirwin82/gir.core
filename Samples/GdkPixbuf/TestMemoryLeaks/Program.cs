﻿using System;
using System.IO;
using GdkPixbuf;

namespace TestMemoryLeaks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var imageBytes = File.ReadAllBytes("gtk.jpg");
            int cycles = 50;
            
            Console.WriteLine("File: Memory can go up. GC.Collect() is called in the end which must free everything up.");
            for (int i = 0; i < cycles; i++)
            {
                var a = Pixbuf.NewFromFile("gtk.jpg");
            }
            Collect();
            Done();
            
            Console.WriteLine("Bytes: Memory can go up. GC.Collect() is called in the end which must free everything up.");

            for (int i = 0; i < cycles; i++)
            {
                var p = PixbufLoader.FromBytes(imageBytes);
            }
            Done();

            Console.WriteLine("File: Memory should not go up as it is freed explicity via Dispose().");
            
            for (int i = 0; i < cycles; i++)
            {
                var a = Pixbuf.NewFromFile("gtk.jpg");
                a.Dispose();
            }
            Done();
            

            Console.WriteLine("Bytes: Memory should not go up as it is freed explicity via Dispose().");
            
            for (int i = 0; i < cycles; i++)
            {
                var p = PixbufLoader.FromBytes(imageBytes);
                p.Dispose();
            }
            Done();
        }

        private static void Collect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        
        private static void Done()
        {
            Console.WriteLine("DONE.");
            Console.ReadLine();
        }
    }
}
