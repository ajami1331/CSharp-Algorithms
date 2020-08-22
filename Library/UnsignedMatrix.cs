namespace Csharp_Contest
{
/*
 * #import_Matrix.cs
 */
    public class UnsignedMatrix: Matrix<uint>
    {
        public UnsignedMatrix(int size) : base(size)
        {
        }

        public override void Identity()
        {
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    this[i, j] = (i == j) ? 1u : 0;
                }
            }
        }

        protected override Matrix<uint> Add(Matrix<uint> right)
        {
            UnsignedMatrix temp = new UnsignedMatrix(this.Size);
            temp.Reset();
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    unchecked
                    {
                        temp[i, j] = this[i, j] + right[i, j];
                    }
                }
            }

            return temp;
        }

        protected override Matrix<uint> Multiply(Matrix<uint> right)
        {
            UnsignedMatrix temp = new UnsignedMatrix(this.Size);
            temp.Reset();
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    for (int k = 0; k < this.Size; k++)
                    {
                        unchecked
                        {
                            temp[i, k] += this[i, j] * right[j, k];
                        }
                    }
                }
            }

            return temp;
        }

        protected override Matrix<uint> Power(long power)
        {
            UnsignedMatrix ans = new UnsignedMatrix(this.Size);
            ans.Identity();
            UnsignedMatrix num = this;
            for (; power > 0; power >>= 1)
            {
                if (power % 2 == 1)
                {
                    ans = (UnsignedMatrix)(ans * num);
                }

                num = (UnsignedMatrix)(num * num);
            }

            return ans;
        }
    }
}
