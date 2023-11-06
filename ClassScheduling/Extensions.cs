using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassScheduling
{
    public static class Extensions
    {
        public static T RandomElement<T>(this IEnumerable<T> source,
                                 Random rng)
        {
            T current = default;
            int count = 0;
            foreach (T element in source)
            {
                count++;
                if (rng.Next(count) == 0)
                {
                    current = element;
                }
            }
            if (count == 0)
            {
                throw new InvalidOperationException("Sequence was empty");
            }
            return current;
        }

        /// <summary>
        ///     Softmax that doesn't choke on Lists larger than 200. See: http://stackoverflow.com/q/9906136
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IList<double> Softmax(this IEnumerable<double> input)
        {
            var inputAsArray = input as double[] ?? input.ToArray();
            var max = inputAsArray.Max();

            var result = new double[inputAsArray.Length];
            var sum = 0.0;
            for (var i = 0; i < inputAsArray.Length; ++i)
            {
                result[i] = Math.Exp(inputAsArray[i] - max);
                sum += result[i];
            }

            for (var i = 0; i < inputAsArray.Length; ++i)
                result[i] /= sum;

            return result;
        }
    }
}
