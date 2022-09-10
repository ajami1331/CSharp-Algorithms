// UnsignedMatrix.cs
// Author: Araf Al Jami
// Last Updated: 10-09-2565 20:57

namespace CLown1331.Library.UnsignedMatrix
{
    using CLown1331.Library.Matrix;

    public class UnsignedMatrix : Matrix<uint>
    {
        public UnsignedMatrix(int size)
            : base(size)
        {
        }

        public override void Identity()
        {
            for (var i = 0; i < this.Size; i++)
            {
                for (var j = 0; j < this.Size; j++)
                {
                    this[i, j] = i == j ? 1u : 0;
                }
            }
        }

        protected override Matrix<uint> Add(Matrix<uint> right)
        {
            var temp = new UnsignedMatrix(this.Size);
            temp.Reset();
            for (var i = 0; i < this.Size; i++)
            {
                for (var j = 0; j < this.Size; j++)
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
            var temp = new UnsignedMatrix(this.Size);
            temp.Reset();
            for (var i = 0; i < this.Size; i++)
            {
                for (var j = 0; j < this.Size; j++)
                {
                    for (var k = 0; k < this.Size; k++)
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
            var ans = new UnsignedMatrix(this.Size);
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
