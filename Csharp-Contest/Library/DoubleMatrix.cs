// DoubleMatrix.cs
// Author: Araf Al Jami
// Last Updated: 23-08-2565 21:39

namespace Library.DoubleMatrix
{
    using Library.Matrix;

    public class DoubleMatrix : Matrix<double>
    {
        public DoubleMatrix(int size)
            : base(size)
        {
        }

        public override void Identity()
        {
            for (var i = 0; i < this.Size; i++)
            {
                for (var j = 0; j < this.Size; j++)
                {
                    this[i, j] = i == j ? 1.0 : 0;
                }
            }
        }

        protected override Matrix<double> Add(Matrix<double> right)
        {
            var temp = new DoubleMatrix(this.Size);
            temp.Reset();
            for (var i = 0; i < this.Size; i++)
            {
                for (var j = 0; j < this.Size; j++)
                {
                    temp[i, j] = this[i, j] + right[i, j];
                }
            }
            return temp;
        }

        protected override Matrix<double> Multiply(Matrix<double> right)
        {
            var temp = new DoubleMatrix(this.Size);
            temp.Reset();
            for (var i = 0; i < this.Size; i++)
            {
                for (var j = 0; j < this.Size; j++)
                {
                    for (var k = 0; k < this.Size; k++)
                    {
                        temp[i, k] += this[i, j] * right[j, k];
                    }
                }
            }
            return temp;
        }

        protected override Matrix<double> Power(long power)
        {
            var ans = new DoubleMatrix(this.Size);
            ans.Identity();
            DoubleMatrix num = this;
            for (; power > 0; power >>= 1)
            {
                if (power % 2 == 1)
                {
                    ans = (DoubleMatrix)(ans * num);
                }
                num = (DoubleMatrix)(num * num);
            }
            return ans;
        }
    }
}
