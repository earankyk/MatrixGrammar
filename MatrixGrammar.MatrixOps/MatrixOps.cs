using System;
namespace MatrixGrammar.MatrixOps
{
    public static class MatrixOps
    {
		// Your semantics implementations here
        public static Int32 [] matPlus(Int32[] A, Int32[] B){
            int m1 = A.Length;
            int m2 = B.Length;
            if(m1 != m2){
                return null;
            }
            int[] result = new int[m1];
            for (int i = 0; i < m1; i++)
            {
                    result[i] = A[i] + B[i];
            }
            return result;
        }
	}
}
