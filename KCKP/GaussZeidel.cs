using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCKP_UI
{
	public class GaussZeidel
	{
		private static double epsilon = 0.01; //точность вычисления
		private int n, k; //n - размерность квадратной матрицы коэффицентов, k-количество итераций
        private static int N = 500; //N -допустимое число итераций
		private double s, Xi, diff = 1; //s - сумма, величина погрешности
		private double[,] matrix; //матрица коэффицентов
		private double[] value; //матрица значений
		private double[] roots; //матрица корней
		private bool diagonal;

        public double[,] Matrix { get => matrix; set => matrix = value; }
        public double[] Value { get => value; set => this.value = value; }
        public double[] Roots { get => roots; set => roots = value; }

        public GaussZeidel(double[,] matrix, double[] value, int n)
		{
			this.Matrix = matrix;
			
			this.Value = value;
			this.n = n;
			Roots = new double[value.Length];
		}

		public bool DiagonallyDominant()
		{
			for (int i = 0; i < n; i++)
			{
				double sum = 0;
				for (int j = 0; j < n; j++)
				{
					if (i != j)
					{
						sum += Math.Abs(Matrix[i, j]);
					}
				}
				if (Math.Abs(Matrix[i, i]) >= sum)
				{
					diagonal = true;
					break;
				}
				else
				{
					diagonal = false;
				}
			}
			return diagonal;
		}

		public void Algoritm()
		{
			k = 0;
			while ((k <= N) && (diff >= epsilon))
			{
				k = k + 1;
				for (int i = 0; i < n; i++)
				{
					s = 0;
					for (int j = 0; j < n; j++)
					{
						if (i != j)
						{
							s += Matrix[i, j] * Roots[j];
						}
					}
					Xi = (Value[i] - s) / Matrix[i, i];
					diff = Math.Abs(Xi - Roots[i]);
					Roots[i] = Xi;
				}
			}
		}
	}
}
