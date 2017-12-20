using System;

namespace WatchDog.Extensions
{
    public static class RandomExtension
    {
        public static T ArrayElement<T>(this Random random, T[] array)
        {
            return array[random.Next(array.Length) - 1];
        }
    }
}
