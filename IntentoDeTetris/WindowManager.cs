using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;




//this was taken from
//http://cgp.wikidot.com/consle-screen-buffer
//and edited

namespace IntentoDeTetris
{
    class WindowManager
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int key);//no encontre otra forma que importando esto xd

        private int roomWidth { get; set; }
        private int roomHeight { get; set; }
        //initiate important variables
        public static char[,] screenBufferArray; //main buffer array
        public static string screenBuffer; //buffer as string (used when drawing)
        public static Char[] arr; //temporary array for drawing string
        public static int i = 0; //keeps track of the place in the array to draw to

        public WindowManager()
        {
            roomHeight= Console.BufferHeight;
            roomWidth = Console.BufferWidth;
            screenBufferArray = new char[roomWidth, roomHeight]; //main buffer array
        }

        public WindowManager(int x,int y)
        {
            roomHeight = y;
            roomWidth = x;
            Console.SetWindowSize(x,y);
            screenBufferArray = new char[x, y]; //main buffer array
        }

        public void Draw(char[,] text, int x, int y)
        {
            int sx = text.GetLength(0);
            int sy = text.GetLength(1);
            for (int i = 0; i < sx; i++)
            {
                for (int j = 0; j < sy; j++)
                {
                        screenBufferArray[(x + i), (j+y)] = text[i,j];
                }
            }
        }

        public void Draw(char[,] text, int x, int y, char ignoredChar)
        {
            int sx = text.GetLength(0);
            int sy = text.GetLength(1);
            for (int i = 0; i < sx; i++)
            {
                for (int j = 0; j < sy; j++)
                {
                    if (text[i,j]!=ignoredChar)
                    {
                        screenBufferArray[(x + i), (j + y)] = text[i, j];
                    }
                }
            }
        }

        //this method takes a string, and a pair of coordinates and writes it to the buffer
        public void Draw(string text, int x, int y)
        {
            for (int i = 0; i < text.Length; i++)
            {
                screenBufferArray[x + i, y] = text[i];
            }
        }

        public void Draw(string text, int x, int y,char ignoredChar)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != ignoredChar)
                {
                    screenBufferArray[x + i, y] = text[i];
                }
            }
        }

        public void DrawScreen()
        {
            screenBuffer = "";
            //iterate through buffer, adding each value to screenBuffer
            for (int iy = 0; iy < roomHeight - 1; iy++)
            {
                for (int ix = 0; ix < roomWidth; ix++)
                {
                    char charActual = screenBufferArray[ix, iy];//solo para optimizar un poquito para no tener que acceder dos veces aca por iteracion
                    screenBuffer +=( charActual=='\0') ? ' ':charActual;//el operador ternario es para que no me tire para la izquierda si no hay nada escrito
                }
                screenBuffer += '\n';//Sin esto hay que maximizar la pantalla dos veces para que funque
            }

            //set cursor position to top left and draw the string
            Console.SetCursorPosition(0, 0);
            Console.Write(screenBuffer);
            screenBufferArray = new char[roomWidth, roomHeight];
            //note that the screen is NOT cleared at any point as this will simply overwrite the existing values on screen. Clearing will cause flickering again.
        }


    }
}
