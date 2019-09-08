using System;
using System.Threading;


namespace IntentoDeTetris
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Console.LargestWindowHeight+" "+Console.LargestWindowWidth);
            WindowManager window = new WindowManager(80,30);
            Tetris tetris = new Tetris(12,18);


            while (!tetris.gameOver)
            {
                //timeo trucho
                Thread.Sleep(50);
                tetris.GameClocking();
                //input
                //chequeamos las banderas que pasan en el juego xd
                if (tetris.extra()) { 
                    for (int i = 0; i < 4; i++)
                    {                                  //las teclas son  //<-  ->  v   ^
                        tetris.keys[i]=( WindowManager.GetAsyncKeyState(("\x25\x27\x28\x26"[i]))!=0);
                    }
                    tetris.CheckInputs();
                    tetris.ForceDown();
                }

                //Dibujando las cosas
                
                window.Draw("Score", 0, 0);
                window.Draw(tetris.Score, 7, 1);
                window.Draw(tetris.field,10,2);
                window.Draw(tetris.GetPiece(), tetris.currentX+10, tetris.currentY+2,' ');

                window.DrawScreen();
            }
            
        }
    }
}
