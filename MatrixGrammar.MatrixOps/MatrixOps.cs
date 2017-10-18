using System;
namespace MatrixGrammar.MatrixOps
{
    public static class MatrixOps
    {
		// Your semantics implementations here
        public static Int32 [][] matPlus(Int32[][] A, Int32[][] B){
            int m1 = A.GetLength(0);
            int n1 = A[0].GetLength(0);
            int m2 = B.GetLength(0);
            int n2 = B[0].GetLength(0);
            if(m1 != m2 || n1 != n2){
                return null;
            }
            int[][] result = new int[m1][];
            for (int i = 0; i < m1; i++)
            {
                result[i] = new int[n1];
                for (int j = 0; j < n1; j++)
                {
                    result[i][j] = A[i][j] + B[i][j];
                }
            }
            return result;
        }
	}
}
