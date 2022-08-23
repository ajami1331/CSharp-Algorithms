// Matrix.cs
// Author: Araf Al Jami
// Last Updated: 23-08-2565 21:39

namespace Library.Matrix
{
    public abstract class Matrix<T>
    {
        private T[,] a;
        private readonly int size;

        protected Matrix(int size)
        {
            this.size = size;
            this.a = new T[this.size, this.size];
        }

        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b) => a.Add(b);

        public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b) => a.Multiply(b);

        public static Matrix<T> operator ^(Matrix<T> a, long power) => a.Power(power);

        public static Matrix<T> operator ^(Matrix<T> a, int power) => a.Power(power);

        public T this[int i, int j]
        {
            get => this.a[i, j];
            set => this.a[i, j] = value;
        }

        public void Reset()
        {
            for (var i = 0; i < this.size; i++)
            {
                for (var j = 0; j < this.size; j++)
                {
                    this[i, j] = default(T);
                }
            }
        }

        public abstract void Identity();

        protected abstract Matrix<T> Add(Matrix<T> right);

        protected abstract Matrix<T> Multiply(Matrix<T> right);

        protected abstract Matrix<T> Power(long power);

        public int Size => this.size;
    }
}
