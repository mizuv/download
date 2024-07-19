
using System.Linq;

namespace Mizuvt.Common {
    public partial class Utils {
        public static float[] GenerateZeroMeanArray(int arrayLength, float interval) {
            if (arrayLength == 0) {
                return new float[0];
            }

            // Create an array with n elements where each element is k apart
            var arr = Enumerable.Range(0, arrayLength).Select(i => i * interval).ToArray();

            // Calculate the mean of the array
            float mean = (arrayLength % 2 == 0)
            ? (arr[arrayLength / 2 - 1] + arr[arrayLength / 2]) / 2
            : arr[arrayLength / 2];


            // Adjust each element so that the new mean is 0
            var zeroMeanArr = arr.Select(x => x - mean).ToArray();

            return zeroMeanArr;
        }
    }
}