namespace Stopify
{
    internal class ArrayManager
    {
        const int DEFAULT_SIZE = 100;
        private int[] vector;
        public int[] Vector { get { return vector; } }

        public ArrayManager(int size)
        {
            vector = new int[size];
        }

        public ArrayManager()
        {
            vector = new int[DEFAULT_SIZE];
        }

        public void FillArray()
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = i + 1;
            }
        }

        public void ShuffleArray()
        {
            Random random = new Random();
            for (int i = vector.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                int temp = vector[i];
                vector[i] = vector[j];
                vector[j] = temp;
            }
        }
    }
}