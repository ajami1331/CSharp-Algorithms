namespace Library.IntegerMatrix
{
    using Library.Matrix;
/*
 * #import_Matrix.cs
 */
    public class IntegerMatrix: Matrix<int>
    {
        private readonly int mod;

        public IntegerMatrix(int size, int mod) : base(size)
        {
            this.mod = mod;
        }

        public override void Identity()
        {
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    this[i, j] = (i == j) ? 1 : 0;
                }
            }
        }

        protected override Matrix<int> Add(Matrix<int> right)
        {
            IntegerMatrix temp = new IntegerMatrix(this.Size, this.mod);
            temp.Reset();
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
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

        protected override Matrix<int> Multiply(Matrix<int> right)
        {
            IntegerMatrix temp = new IntegerMatrix(this.Size, this.mod);
            temp.Reset();
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    for (int k = 0; k < this.Size; k++)
                    {
                        temp[i, k] += (int)((1L * this[i, j] * right[j, k]) % this.mod);
                        while (temp[i, k] >= this.mod)
                        {
                            temp[i, k] -= this.mod;
                        }
                    }
                }
            }

            return temp;
        }

        protected override Matrix<int> Power(long power)
        {
            IntegerMatrix ans = new IntegerMatrix(this.Size, this.mod);
            ans.Identity();
            IntegerMatrix num = this;
            for (; power > 0; power >>= 1)
            {
                if (power % 2 == 1)
                {
                    ans = (IntegerMatrix)(ans * num);
                }

                num = (IntegerMatrix)(num * num);
            }

            return ans;
        }
    }
}
