using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace IntentoDeTetris
{
    class Tetris
    {
        //campo
        public char[,] field { get; }
        static int fieldWidth;
        static int fieldHeight;

        //Piezas
        public string[] tetromino = new string[7];
        public static int indexCurrentPiece { get; protected set; }
        public static int indexCurrentRotation { get; protected set; }
        public int currentX;
        public int currentY = 0;

        //inputs
        public bool[] keys { get; set; }
        private bool FijadorDeRotacion = false;

        //logica
        private int ticksInGame = 0;
        private int speed = 20;
        private bool forzarBajada = false;
        public bool gameOver { get; private set; }
        private List<int> LinesToClear { get; set; }

        //extra
        public bool noInput{get;private set;}
        private int masBajo { get; set; }
        private int masAlto { get; set; }
        Random random;

        public Tetris(int x, int y)
        {
            fieldWidth = x;
            fieldHeight = y;
            currentX = fieldWidth / 2;
            field = new char[fieldWidth ,fieldHeight];
            for (int i = 0; i < fieldWidth; i++)
            {
                for (int j = 0; j < fieldHeight; j++)
                {
                    field[i, j] = (i == 0 || i == fieldWidth - 1 || j == fieldHeight - 1) ? '#':' ';
                }
            }


            keys = new bool[4];
            indexCurrentPiece = 1;
            indexCurrentRotation = 0;
            gameOver = false;

            tetromino[0] =  "..X.";
            tetromino[0] += "..X.";
            tetromino[0] += "..X.";
            tetromino[0] += "..X.";

            tetromino[1] =  "..X.";
            tetromino[1] += ".XX.";
            tetromino[1] += ".X..";
            tetromino[1] += "....";

            tetromino[2] =  ".X..";
            tetromino[2] += ".XX.";
            tetromino[2] += "..X.";
            tetromino[2] += "....";

            tetromino[3] =  "....";
            tetromino[3] += ".XX.";
            tetromino[3] += ".XX.";
            tetromino[3] += "....";

            tetromino[4] =  "..X.";
            tetromino[4] += ".XX.";
            tetromino[4] += "..X.";
            tetromino[4] += "....";

            tetromino[5] =  ".XX.";
            tetromino[5] += "..X.";
            tetromino[5] += "..X.";
            tetromino[5] += "....";

            tetromino[6] =  ".XX.";
            tetromino[6] += ".X..";
            tetromino[6] += ".X..";
            tetromino[6] += "....";


            random = new Random();
            LinesToClear = new List<int>();
            masBajo = 0;
            masAlto = fieldHeight;
        }

        
        public char[,] GetPiece() {
            char[,] pieza=new char[4,4];
            char[] piezita=tetromino[indexCurrentPiece].ToCharArray();
            char letra = 'A';
            letra+= (char)indexCurrentPiece;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    pieza[x, y] = (piezita[Rotate(x, y, indexCurrentRotation)] != '.') ? letra : ' ';
                }
            }

            return pieza;
        }

        private int Rotate(int px, int py, int r)
        {
            switch (r)
            {
                case 0: return py * 4 + px;         //0 grados
                case 1: return 12 + py - (px * 4);  //90 grados
                case 2: return 15 - (py * 4) - px;  //180 grados
                case 3: return 3 - py + (px * 4);   //270 grados
                case 4: indexCurrentRotation = 0;  return py * 4 + px;//0 grados pero con Bound bouncing digamos
            }
            return 0;
        }

        public void CheckInputs()
        {
            currentX -= (keys[0] && this.DoesFitIn(indexCurrentPiece, indexCurrentRotation, currentX - 1, currentY)) ? 1 : 0;

            currentX += (keys[1] && this.DoesFitIn(indexCurrentPiece, indexCurrentRotation, currentX + 1, currentY)) ? 1 : 0;

            currentY += (keys[2] && this.DoesFitIn(indexCurrentPiece, indexCurrentRotation, currentX, currentY + 1)) ? 1 : 0;
            if (keys[3])
            {
                indexCurrentRotation += (!FijadorDeRotacion && this.DoesFitIn(indexCurrentPiece, indexCurrentRotation + 1, currentX, currentY)) ? 1 : 0;
                FijadorDeRotacion = true;
            }
            else FijadorDeRotacion = false;
        }

        public bool DoesFitIn(int IPieza,int rotation,int x , int y)
        {
            List<int> xd = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                int fi = (x + i);//para hacer un calculo 3 veces menos xd
                for (int j = 0; j < 4; j++)
                {
                    int pi = Rotate(i,j,rotation);
                    xd.Add(pi);
                    int fj = (j + y);
                    //checking for OoB
                    if (fi>=0 && fi<fieldWidth)
                    {
                        if (fj >= 0 && fj < fieldHeight)
                        {
                            if (tetromino[IPieza][pi]!='.' && field[fi,fj]!=' ')
                            {
                                return false;//No se puede
                            }
                        }
                    }
                }
            }

            return true;
        }

        public void GameClocking()
        {
            ticksInGame++;
            forzarBajada = (ticksInGame % speed==0);
        }


        public bool extra()
        {
           
            if (LinesToClear.Count != 0)
            {
                masAlto = LinesToClear[LinesToClear.Count-1];
                masBajo = LinesToClear[0];
                for (int i = 0; i < LinesToClear.Count; i++)
                {
                    int xd=LinesToClear[i];
                    for (int j = 1; j < fieldWidth-1; j++)
                    {
                        field[j, xd] = ' ';
                    }
                }
                LinesToClear.Clear();
                noInput = true;
                return false;
            }
            if (noInput)
            {
                noInput = false ;
                int margen= masAlto - masBajo+1;//margen entre las lineas borradas y las q tienen q bajar
                for (int i = masAlto; i >= 0+margen; i--)
                {
                    for (int j = 1; j < fieldWidth-1; j++)
                    {
                        field[j, i] = field[j, i - margen];
                    }
                }
                //cuando termina de eliminar las lineas reseteamos esto para la proxima
                masAlto = 0;
                masBajo = 0;
                return false;
            }

            return true;
        }

        public List<int> ForceDown()
        {
            
            if (forzarBajada)
            {
                if(this.DoesFitIn(indexCurrentPiece, indexCurrentRotation, currentX, currentY + 1))
                {
                    currentY++;
                }
                else
                {
                    //clavar la pieza en el lugar
                    for (int x = 0; x < 4; x++)
                    {
                        for (int y = 0; y < 4; y++)
                        {
                            if (tetromino[indexCurrentPiece][Rotate(x, y, indexCurrentRotation)] != '.')
                            {
                                char letra = 'A';
                                letra += (char)indexCurrentPiece;
                                field[currentX+x,currentY+y] = letra;
                            }
                        }
                    }
                    //fijarse si no hay algun tetris
                    for (int y = 0; y < 4; y++)
                    {
                        if (currentY+y<fieldHeight-1)
                        {
                            bool IsALine = true;
                            for (int x = 1; x < fieldWidth-1; x++)
                                if (field[x,currentY+y]==' ')
                                {
                                    IsALine = false;
                                    break;
                                }
                             
                            //si hay una linea cmabiarla a === y despues eliminarlas xd
                            if (IsALine)
                            {
                                for (int x = 1; x < fieldWidth-1; x++)
                                {
                                    field[x,currentY+ y] = '=';
                                    LinesToClear.Add(currentY+y);
                                }
                            }
                        }
                    }
                    //elegir nueva pieza
                    indexCurrentPiece = random.Next(0, 7);
                    indexCurrentRotation = 0;
                    currentX = fieldWidth / 2;
                    currentY = 0;
                    //y si la pieza no entra EN LA PANTALLA
                    gameOver = (!this.DoesFitIn(indexCurrentPiece, indexCurrentRotation, currentX, currentY));
                }
            }
                return LinesToClear;
        }

    }
}
