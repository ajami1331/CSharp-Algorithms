namespace Csharp_Contest
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

        public void Reset()
        {
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    this[i, j] = default(T);
                }
            }
        }

        public abstract void Identity();

        public static Matrix<T> operator+ (Matrix<T> a, Matrix<T> b)
        {
            return a.Add(b);
        }

        protected abstract Matrix<T> Add(Matrix<T> right);

        public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b)
        {
            return a.Multiply(b);
        }

        protected abstract Matrix<T> Multiply(Matrix<T> right);

        public static Matrix<T> operator ^(Matrix<T> a, long power)
        {
            return a.Power(power);
        }

        public static Matrix<T> operator ^(Matrix<T> a, int power)
        {
            return a.Power(power);
        }

        public T this[int i, int j]
        {
            get => this.a[i, j];
            set => this.a[i, j] = value;
        }

        protected abstract Matrix<T> Power(long power);

        public int Size => this.size;
    }
}
