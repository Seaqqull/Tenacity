using UnityEngine.Events;
using System;


namespace Tenacity.Utility.Methods
{
    /// <summary>
    /// Float extension class.
    /// </summary>
    public static class FloatHelper
    {
        /// <summary>
        /// Maps float value from interval (istart, istop) to (ostart, ostop).
        /// </summary>
        /// <param name="value">Value to be mapped.</param>
        /// <param name="istart">Original min value.</param>
        /// <param name="istop">Original max value.</param>
        /// <param name="ostart">Relative min value.</param>
        /// <param name="ostop">Relative max value.</param>
        /// <returns>Mapped float value.</returns>
        public static float Map(this float value, float istart, float istop, float ostart, float ostop)
        {
            return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
        }
    }

    public static class ButtonHelper
    {
        public static void SetAction(this UnityEvent evt, Action act) {
            evt.RemoveAllListeners();
            if (act != null)
            {
                evt.AddListener(new UnityAction(act));                
            }
        }
    }
}