// LongMatrix.cs
// Author: Araf Al Jami
// Last Updated: 07-09-2565 03:03

namespace Library.LongMatrix
{
    using Library.Matrix;

    public class LongMatrix : Matrix<long>
    {
        private readonly long mod;

        public LongMatrix(int size, long mod)
            : base(size)
        {
            this.mod = mod;
        }

        public override void Identity()
        {
            for (var i = 0; i < this.Size; i++)
            {
                for (var j = 0; j < this.Size; j++)
                {
                    this[i, j] = i == j ? 1 : 0;
                }
            }
        }

        protected override Matrix<long> Add(Matrix<long> right)
        {
            var temp = new LongMatrix(this.Size, this.mod);
            temp.Reset();
            for (var i = 0; i < this.Size; i++)
            {
                for (var j = 0; j < this.Size; j++)
                {
                    temp[i, j] = (this[i, j] + right[i, j]) % this.mod;
                    while (temp[i, j] >= this.mod)
                    {
                        temp[i, j] -= this.mod;
                    }
                }
            }

            return temp;
        }

        protected override Matrix<long> Multiply(Matrix<long> right)
        {
            var temp = new LongMatrix(this.Size, this.mod);
            temp.Reset();
            for (var i = 0; i < this.Size; i++)
            {
                for (var j = 0; j < this.Size; j++)
                {
                    for (var k = 0; k < this.Size; k++)
                    {
                        temp[i, k] += this[i, j] * right[j, k] % this.mod;
                        while (temp[i, k] >= this.mod)
                        {
                            temp[i, k] -= this.mod;
                        }
                    }
                }
            }

            return temp;
        }

        protected override Matrix<long> Power(long power)
        {
            var ans = new LongMatrix(this.Size, this.mod);
            ans.Identity();
            LongMatrix num = this;
            for (; power > 0; power >>= 1)
            {
                if (power % 2 == 1)
                {
                    ans = (LongMatrix)(ans * num);
                }

                num = (LongMatrix)(num * num);
            }

            return ans;
        }
    }
}
