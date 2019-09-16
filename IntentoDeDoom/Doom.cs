using System;
using System.Collections.Generic;
using System.Text;


class Doom:WindowManager
{
    
    public double playerX;
    public double playerY;
    public double playerAngle;
    public double FOV;
    public int mapX;
    public int mapY;
    public string[] map;
    public float DeltaTime { get; private set; }


    public Doom(int x , int y):base(x,y)
    {
        playerX = 7.0f;
        playerY = 7.0f;
        playerAngle = 0.0f;
        FOV = (float)Math.PI / 4;

        mapX = 16;
        mapY = 16;
        map = new string[mapX];
        int i = 0;
        map[i++] = "################";
        map[i++] = "#..............#";
        map[i++] = "#..............#";
        map[i++] = "#..............#";
        map[i++] = "#..............#";
        map[i++] = "#..............#";
        map[i++] = "#..............#";
        map[i++] = "#..............#";
        map[i++] = "#..............#";
        map[i++] = "#..............#";
        map[i++] = "#....#.........#";
        map[i++] = "#.......#......#";
        map[i++] = "#..............#";
        map[i++] = "#..............#";
        map[i++] = "#..............#";
        map[i++] = "################";

        t1 = DateTime.Now;
        t2 = DateTime.Now;
    }

    


    public void CheckInputs()
    {
        t2 = DateTime.Now;

        //en vdd no se si es delta time pero ya lo saque de unity
        DeltaTime = t2.Millisecond-t1.Millisecond;
        t1 = t2;
        if (GetAsyncKeyState('A')!= 0)
        {
            playerAngle -= (0.1f)/DeltaTime;
        }
        if (GetAsyncKeyState('W') != 0)
        {
            playerX += Math.Sin(playerAngle) / DeltaTime;
            playerY += Math.Cos(playerAngle) / DeltaTime;
            if (map[(int)playerX][(int)playerY]!='.')
            {
                playerX -= Math.Sin(playerAngle) / DeltaTime;
                playerY -= Math.Cos(playerAngle) / DeltaTime;
            }
        }
        
        if (GetAsyncKeyState('S') != 0)
        {
            playerX -= Math.Sin(playerAngle) / DeltaTime;
            playerY -= Math.Cos(playerAngle) / DeltaTime;
            if (map[(int)playerX][(int)playerY] != '.')
            {
                playerX += Math.Sin(playerAngle) / DeltaTime;
                playerY += Math.Cos(playerAngle) / DeltaTime;
            }
        }
        if (GetAsyncKeyState('D') != 0)
        {
            playerAngle += (0.1f)/DeltaTime;
        }
    }

    public void RayCasting()
    {
        for (int i = 0; i < roomWidth; i++)
        {
            double rayAngle = (playerAngle - FOV / 2.0f) + ((double)i / (double)roomWidth);
            double distance = 0;

            bool hitwall = false;
            bool boundary=false;

            double eyeX = Math.Sin(rayAngle);
            double eyeY = Math.Cos(rayAngle);
            while (!hitwall && distance < depth)
            {
                distance += 0.1f;
                int testX = (int)(playerX + eyeX * distance);
                int testY = (int)(playerY + eyeY * distance);

                if (testX < 0 || testX > mapX || testY < 0 || testY > mapY)
                {
                    hitwall = true;
                    distance = depth;
                }
                else
                {
                    if (map[testY][testX] == '#')
                    {
                        hitwall = true;
                        //en esta parte ya no entendí nada pero traté de adaptarlo igual
                        var p = new List<Tuple<double, double>>();

                        for (int tx = 0; tx < 2; tx++)
                            for (int ty = 0; ty < 2; ty++)
                            {
                                double vy = (double)testY + ty - playerY;
                                double vx = (double)testX + tx - playerX;
                                double d = Math.Sqrt(vx * vx + vy * vy);
                                double dot = (eyeX * vx / d) + (eyeY * vy / d);
                                p.Add(Tuple.Create(d, dot));
                            }
                        p.Sort((a,b)=> a.Item1.CompareTo(b.Item1));
                        double bound = 0.009f;
                        if (Math.Acos(p[0].Item2) < bound) boundary = true;
                        if (Math.Acos(p[1].Item2) < bound) boundary = true;
                        if (Math.Acos(p[2].Item2) < bound) boundary = true;


                        //aca ya volvi a entender
                    }
                }

            }
            //de hecho no se cual es el piso y cual es el techo
            int ceiling = (int)((double)(roomHeight / 2.0) - roomHeight / ((double)distance));
            int floor = roomHeight - ceiling;
            char shade;

            if (distance < depth / 4)//cerca
            {
                shade = '\x2588';
            }
            else if(distance<depth/3)
            {
                shade = '\x2593';
            }
            else if (distance < depth / 2)
            {
                shade = '\x2592';
            }
            else if (distance < depth / 1)//lejos
            {
                shade = '\x2591';
            }
            else//muy lejos q no llega a renderizar
            {
                shade = ' ';
            }
            if (boundary) shade = ' ';
            for (int j = 0; j < roomHeight; j++)
            {
                if (j<=ceiling)
                {
                    screenBufferArray[i,j] = ' ';
                }
                else if (floor >= j && j > ceiling)
                {
                    screenBufferArray[i,j]= shade;
                }
                else 
                {
                    double floorDistance = 1.0f - (((double)j - roomHeight / 2.0f) / ((double)roomHeight / 2));
                    if (floorDistance< 0.25) shade = '#';
                    else if (floorDistance< 0.5) shade = 'x';
                    else if (floorDistance < 0.75) shade = '.';
                    else if (floorDistance < 0.9) shade = '-';
                    else shade = ' ';
                    screenBufferArray[i,j] = shade;
                }
            }
        }
    }

}

