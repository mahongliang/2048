using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048Console
{
    class Program
    {
        public static int mySorce;
        static void Main(string[] args)
        {
            int[,] a = new int[4, 4];
            a[0, 0] = 2;
            a[0, 1] = 2;
            a[0, 3] = 2;
            mySorce = 0;

            RePaint(a);
            while(true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                switch(key.Key)
                {
                    case ConsoleKey.UpArrow:
                        a = SquareRot90(a, 3);
                        a = Merge(a);
                        a = SquareRot90(a, -3);
                        break;

                    case ConsoleKey.DownArrow:
                        a = SquareRot90(a, 1);
                        a = Merge(a);
                        a = SquareRot90(a, -1);
                        break;

                    case ConsoleKey.LeftArrow:
                        a = Merge(a);
                        break;

                    case ConsoleKey.RightArrow:
                        a = SquareRot90(a, 2);
                        a = Merge(a);
                        a = SquareRot90(a, -2);
                        break;
                }

                Point cp = RandomPoint(a);
                if(cp != null)
                {
                    a[cp.X, cp.Y] = 2;
                    RePaint(a);
                }

                if(cp ==null && !CanMove(a))
                {
                    RePaint(a, "game over");
                }
            }
        }
        //向左移动并合并相同的元素
        public  static int[,] Merge(int[,] a)
        {           
            for(int i=0; i< a.GetLength(0); i++)
            {
                int lastNum = 0;
                int last_j = 0;
                for(int j = 0 ;j<a.GetLength(1);j++)
                {
                    if( lastNum != a[i, j] && a[i, j] !=0)
                    {
                        lastNum = a[i, j];
                        last_j = j;
                    }
                    else if(lastNum == a[i, j])
                    {
                        a[i, last_j] = 0;
                        a[i, j] = lastNum + a[i, j];
                        mySorce += 2 * lastNum;
                        lastNum = a[i, j];//改了一个bug                      
                    }
                }

                last_j = 0;
                for(int j = 0; j<a.GetLength(1); j++)
                {
                    if(a[i,j] != 0)
                    {
                        a[i, last_j] = a[i, j];
                        if (last_j != j)
                            a[i, j] = 0;
                        last_j++;
                    }
                }
            }
            return a;      
        }

        //矩阵顺时针旋转90度
        public static int[,] SquareRot90(int[,] a, int rotNum)
        {
            while(rotNum < 0)
            {
                rotNum += 4;
            }

            for(int rot_i= 0; rot_i<rotNum;rot_i++)
            {
                int[,] b = new int[a.GetLength(0), a.GetLength(1)];
                for(int i =0; i<a.GetLength(0);i++)
                {
                    for(int j =0 ;j <a.GetLength(1);j++)
                    {
                        b[j, a.GetLength(0) - i - 1] = a[i, j];
                    }
                }
                a = b;
            }
            return a;      
        }

        //随机生成点，从矩阵的空位子中随机生成一个点，若不存在空位子返回null；
        public static Point RandomPoint(int[,] a)
        {
            List<Point> lstP = new List<Point>();
            for(int i =0;i< a.GetLength(0);i++)
            {
                for(int j =0;j<a.GetLength(1);j++)
                {
                    if(a[i,j] == 0)
                    {
                        lstP.Add(new Point(i, j));
                    }
                }
            }

            if(lstP.Count == 0)
            {
                return null;
            }
            int rnd = new Random().Next(lstP.Count);

            return lstP[rnd];
        }

        //是否可以继续移动
        public static bool CanMove(int[,] a)
        {
            bool res = false;
            int[,] b= CopyToB(a);
            b = Merge(b);
            if(!IsEquals(a,b))
                res =true;

            b = CopyToB(a);
            b = SquareRot90(b, 1);
            b = Merge(b);
            b = SquareRot90(b, -1);
            if (!IsEquals(a, b))
                res = true;

            b = CopyToB(a);
            b = SquareRot90(b, 2);
            b = Merge(b);
            b = SquareRot90(b, -2);
            if (!IsEquals(a, b))
                res = true;

            b = CopyToB(a);
            b = SquareRot90(b, 3);
            b = Merge(b);
            b = SquareRot90(b, -3);
            if (!IsEquals(a, b))
                res = true;

            return res;              
        } 

        public static int[,] CopyToB(int[,] a)
        {
            int[,] b = new int[a.GetLength(0), a.GetLength(1)];
            for(int i = 0; i<a.GetLength(0);i++)
            {
                for(int j =0; j<a.GetLength(1);j++)
                {
                    b[i, j] = a[i, j];
                }
            }
            return b;
        }

        public static bool IsEquals(int[,] a, int[,] b)
        {
            bool res =true;
            for(int i = 0; i<a.GetLength(0);i++)
            {
                for(int j =0; j<a.GetLength(1);j++)
                {
                    if(b[i, j] != a[i, j])
                    {
                        res =false;
                        break;
                    }
                }
                if (!res)
                    break;
            }
            return res;
        }

        public static void RePaint(int[,] a, string s)
        {
            while(true)
            {
                Console.Clear();
                RePaint(a);
                Console.WriteLine("\n\n\n\n\t\t" + s + "\n\n");
                Console.ReadKey();
            }       
        }

        //刷新界面
        public static void RePaint(int[,] a)
        {
            Console.Clear();
            Console.WriteLine("My Sorce is{0}", mySorce);
            for(int j=0; j< a.GetLength(1);j++)
            {
                Console.Write("----");
            }

            Console.Write("\n");
            for(int i=0;i<a.GetLength(0);i++)
            {
                Console.Write("|");
                for(int j= 0;j<a.GetLength(1);j++)
                {
                    string s = "";
                    if (a[i, j] == 0)
                        s = "   ";
                    else if (a[i, j] < 10)
                        s = " " + a[i, j] + " ";
                    else if (a[i, j] < 100)
                        s = "" + a[i, j] + " ";
                    else if (a[i, j] < 1000)
                        s = "" + a[i, j];

                    Console.Write(s + "|");
                }
                Console.Write("\n");
                for(int j = 0; j<a.GetLength(1);j++)
                {
                    Console.Write("----");
                }
                Console.Write("\n");
            }
        }

    }

    class Point
    {
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }
    }
}
