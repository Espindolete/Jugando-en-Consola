using System;


class Program
{
    static void Main(string[] args)
    {
        Doom doom = new Doom(120,40);

        while (true)
        {
            doom.CheckInputs();
            doom.RayCasting();
            doom.DrawScreen();
        }


    }
    
}

