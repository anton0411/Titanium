using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titanium.Utilities
{
    /// <summary>
    /// A static class full of math helper functions.
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// This function smoothly changes one value to another. This should be called every frame.
        /// A ratio of 10 is considered about average, 5 is fast, and 20 is slow.
        /// </summary>
        /// <param name="startVal">The value to change</param>
        /// <param name="endVal">The desired value</param>
        /// <param name="ratio">The ratio of change</param>
        /// <returns>The resulting value</returns>
        public static float smoothChange(float startVal, float endVal, int ratio)
        {
            float newVal = (startVal - endVal) / ratio;
            return -newVal;
        }
    }
}
