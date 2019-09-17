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
            doom.Draw(doom.map, 0, 0);
            doom.Draw('P', (int)doom.playerX, (int)doom.playerY);
            doom.Draw((int)(1/doom.DeltaTime), 16, 0);//you can think this as if they were the FPS lol
            doom.DrawScreen();
        }


    }
    
}

