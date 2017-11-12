using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
namespace MatrixGrammar.MatrixOps
{
    public static class MatrixOps
    {
		// Your semantics implementations here
        public static double[] matPlus(double[] A, double[] B){
            if(A[0]!=B[0] || A[1]!=B[1] || A.Length!=B.Length){
                return null;
            }
            double[] result = new double[A.Length];
            result[0] = A[0];
            result[1] = A[1];
            for (int i = 2; i < A.Length; i++)
            {
                    result[i] = A[i] + B[i];
            }
            return result;
        }

        public static double[] matMultScalar(double n, double[] factor)
        {
            int m1 = factor.Length;
            double[] result = new double[m1];
            result[0] = factor[0];
            result[1] = factor[1];
            for (int i = 2; i < m1; i++)
            {
                result[i] = n * factor[i];
            }
            return result;
        }

        public static Int32[,] convertTo2dArray(Int32[] A)
        {
            int rows = A[0];
            int cols = A[1];
            int[,] result2d = new int[rows, cols];
            int index = 2;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result2d[i, j] = A[index];
                    index++;
                }
            }

            return result2d;
        }

        public static double[] removeDim(double[] A)
        {
            double rows = A[0];
            double cols = A[1];
            double[] result = new double[A.Length - 2];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = A[i + 2];
            }

            return result;
        }


        public static double[] matMult(double[] factor1, double[] factor2)
        {
            int m1rows = (int)factor1[0];
            int m2rows = (int)factor2[0];
            int m1cols = (int)factor1[1];
            int m2cols = (int)factor2[1];

            if (m1cols != m2rows)
            {
                return null; // is this necessary?
            }
            double[,] f12d = new double[m1rows, m1cols];
            double[,] f22d = new double[m2rows, m2cols];
            int index = 2;

            for (int i = 0; i < m1rows; i++)
            {
                for (int j = 0; j < m1cols; j++)
                {
                    f12d[i, j] = factor1[index];
                    index++;
                }
            }
            //Console.WriteLine("f1");
            //Console.WriteLine("f1rows:" + m1rows);
            //Console.WriteLine("f1cols:" + m1cols);
            //for (int i = 0; i < m1rows; i++)
            //{
            //    for (int j = 0; j < m1cols; j++)
            //    {
            //        Console.Write(f12d[i, j] + " ");


            //    }
            //    Console.WriteLine();
            //}


            index = 2;
            for (int i = 0; i < m2rows; i++)
            {
                for (int j = 0; j < m2cols; j++)
                {
                    f22d[i, j] = factor2[index];
                    index++;
                }
            }


            //for (int i = 0; i < m2rows; i++)
            //{
            //    for (int j = 0; j < m2cols; j++)
            //    {
            //        Console.Write(f22d[i, j] + " ");


            //    }
            //    Console.WriteLine();
            //}


            double[] result = new double[m1rows * m2cols + 2];
            double[,] result2d = new double[m1rows, m2cols];
            for (int i = 0; i < m1rows; i++)
            {
                for (int j = 0; j < m2cols; j++)
                {
                    for (int k = 0; k < m1cols; k++) // OR k<b.GetLength(0)
                        result2d[i, j] = result2d[i, j] + f12d[i, k] * f22d[k, j];
                }
            }
            //Console.WriteLine("result");
            index = 2;
            result[0] = m1rows;
            result[1] = m2cols;
            for (int i = 0; i < m1rows; i++)
            {
                for (int j = 0; j < m2cols; j++)
                {
                    //Console.Write(result2d[i, j] + " ");
                    result[index] = result2d[i, j];
                    index++;
                }
                //Console.WriteLine();
            }

            return result;
        }

        public static void test()
        {
            double[] a = new double[] { 1, 2, 3, 4 };
            Matrix<double> someMatrix = Matrix<double>.Build.Dense(2, 2, a);
            Matrix<double> inverseM = someMatrix.Inverse();
            Console.WriteLine();

        }
	}
}
