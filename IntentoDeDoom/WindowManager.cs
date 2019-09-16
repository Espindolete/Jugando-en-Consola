using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;




//this was taken from
//http://cgp.wikidot.com/consle-screen-buffer
//and edited

class WindowManager
{
    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int key);//no encontre otra forma que importando esto xd


    public double depth = 16;
    public int roomWidth { get; private set; }
    public int roomHeight { get; private set; }
    //initiate important variables
    protected static char[,] screenBufferArray;
    //main buffer array
    protected static string screenBuffer; //buffer as string (used when drawing)
    
    protected static int i = 0; //keeps track of the place in the array to draw to
    protected DateTime t1;
    protected DateTime t2;


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

    
    public void Draw(int text, int x, int y)
    {
        List<int> digits = new List<int>();
        while (text > 0)
        {
            digits.Add(text % 10);
            text /= 10;
        }
        int i = 0;
        while (i<digits.Count && x+i<roomWidth)
        {
            screenBufferArray[x + i, y] = (char)((char)(digits[digits.Count-1-i]) + '0');
            i++;
        }
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

    public void Draw(string[] text, int x=0, int y=0)
    {
        for (int i = 0; i < text.Length; i++)
        {
            for (int j = 0; j < text[i].Length; j++)
            {
                screenBufferArray[x + i, y+j] = text[i][j];
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
                    char charActual = screenBufferArray[ix, iy];
                    screenBuffer +=( charActual=='\0') ? ' ':charActual;//ternary operator just for fun
                }
                screenBuffer += '\n';//without this i need to maximize the screen two times to get the Screen to show as i want
            }

            //set cursor position to top left and draw the string
            Console.SetCursorPosition(0, 0);
            Console.Write(screenBuffer);
            screenBufferArray = new char[roomWidth, roomHeight];
            //note that the screen is NOT cleared at any point as this will simply overwrite the existing values on screen. Clearing will cause flickering again.
        }


}

