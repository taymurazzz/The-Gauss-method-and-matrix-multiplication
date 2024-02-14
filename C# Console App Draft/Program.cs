using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void PrintMatrix(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write(matrix[i, j]+ " ");
            }
            Console.WriteLine();
        }
    }
    static int[,] InputMatrix()
    {
        int m, n;
        Console.WriteLine("Введите размерность матрицы в формате \"3 3\" где первое значение кол-во строк второе - колво столбцов: ");
        string line = Console.ReadLine();
        m = int.Parse(line.Split(' ')[0]);
        n = int.Parse(line.Split(' ')[1]);
        Console.WriteLine("Введите матрицу разделяя столбцы пробелом, пример: \n 1 2 3 \n 4 5 6 \n 7 8 9\nНачните вводить матрицу построчно: \n");
        int[,] matrix = new int[m, n];
        for (int i = 0; i < m; i++)
        {
            string[] s = Console.ReadLine().Split(' ');
            for (int j = 0; j < n; j++)
            {
                matrix[i, j] = int.Parse(s[j]);
            }
        }
        return matrix;
    }
    static double[][] InputSLAY()
    {
        int m, n;
        Console.WriteLine("Введите размерность СЛАУ учитывая результирующий столбец в формате \"3 3\" где первое значение кол-во строк второе - колво столбцов: ");
        string line = Console.ReadLine();
        m = int.Parse(line.Split(' ')[0]);
        n = int.Parse(line.Split(' ')[1]);
        Console.WriteLine("Введите СЛАУ разделяя столбцы пробелом, пример: \n 1 2 3 | 1 \n 4 5 6 | 1 \n 7 8 9 | 1\nНачните вводить СЛАУ построчно: \n");
        double[][] SLAY = new double[m][];
        for(int i =0; i< m; i++)
        {
            SLAY[i] = new double[n];
        }
        for (int i = 0; i < m; i++)
        {
            string[] s = Console.ReadLine().Split(' ');
            for (int j = 0; j < n; j++)
            {
                SLAY[i][j] = double.Parse(s[j]);
            }
        }
        return SLAY;
    }
    static int[,] MultiplyMatrix(int[,] first, int[,] second)
    {
        int[,] result = new int[first.GetLength(0), second.GetLength(1)];
        for (int i = 0; i < first.GetLength(0); i++)
        {
            for (int j=0 ; j < second.GetLength(1); j++)
            {
                int sum = 0;
                for (int firJ=0; firJ<first.GetLength(1); firJ++)
                {
                    sum += first[i, firJ] * second[firJ, j];
                }
                result[i,j] = sum;
            }
        }
        return result;
    }

    static double[][] MethodGauss(double[][] slay)
    {
        string answerX = "";
        for(int j=0; j < slay[0].Length-1; j++) // верхняя граница выбора строки которую можно взять за основную
        {
            double[] osnova= null;  // будет хранить ссылку на выбранную строку
            for (int i = 0; i < slay.Length; i++) // выбор основной строки и уничтожение лишних в j-ом столбце
            {
                if (slay[i][j] == 0)
                {
                    continue;
                }
                else if (osnova==null)
                {
                    bool flag = false;
                    for (int z = 0; z<j; z++) // проверяем чтобы слева небыло нулей иначе строка испортит нам уже зануленые элементы 
                    {
                        if (slay[i][z] != 0)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        continue;
                    }
                    osnova = slay[i]; // тут не создается копия, они указывают на один и тот же адрес памяти
                }
                else
                {
                    double multOs = slay[i][j];
                    double multSl = osnova[j];

                    for ( int z =j; z< slay[0].Length; z++)
                    {
                        slay[i][z] = slay[i][z]*multSl - osnova[z]*multOs;
                    }

                }

            }


            for (int i = 0; i < slay.Length; i++)
            {
                if (slay[i][j] == 0)
                {
                    continue;
                }
                else if (osnova == slay[i])
                {
                    continue;
                }
                else
                {
                    double multOs = slay[i][j];
                    double multSl = osnova[j];

                    for (int z = 0; z < slay[0].Length; z++)
                    {
                        slay[i][z] = slay[i][z] * multSl;
                    }

                    for (int z = j; z < slay[0].Length; z++)
                    {
                        slay[i][z] = slay[i][z] - osnova[z] * multOs;
                    }

                }

            }
        }
        
        return slay;
    }

    static void Main()
    {
        Console.WriteLine("Выберите операциию 1 - умножение матриц 2 - решение слау методом гаусса: ");
        int Operation = int.Parse(Console.ReadLine());

        if (Operation == 1) {
            Console.WriteLine("Ввод первой матрицы");
            int[,] first = InputMatrix();
            Console.WriteLine("Ввод второй матрицы");
            int[,] second = InputMatrix();

            if (first.GetLength(1) != second.GetLength(0))
            {
                Console.WriteLine("Данные матрицы нельзя перемножить");
                return ;
            }
            Console.WriteLine();
            PrintMatrix(MultiplyMatrix(first, second));

        }

        if (Operation == 2)
        {
            double[][] slay = InputSLAY();
            slay = MethodGauss(slay);
            Console.WriteLine();
            Console.WriteLine("Матрица после преобразований Гаусса:");
            for ( int i = 0; i < slay.Length; i++)
            {
                for ( int j = 0; j < slay[i].Length; j++)
                {
                    Console.Write(slay[i][j]+" ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Корни:");
            for (int i=0; i < slay.Length; i++)
            {
                int j = slay[i].Length - 1;
                Console.WriteLine($"x{i} = {slay[i][j] / slay[i][i]}");
            }
        }


    }
}
