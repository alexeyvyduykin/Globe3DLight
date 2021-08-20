namespace TimeDataViewer.Core
{
    public static class ArrayBuilder
    {
        /// <summary>
        /// Fills the two-dimensional array with the specified value.
        /// </summary>
        /// <param name="array">The two-dimensional array.</param>
        /// <param name="value">The value.</param>
        public static void Fill2D(this double[,] array, double value)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = value;
                }
            }
        }
    }
}
