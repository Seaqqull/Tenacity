using UnityEngine;


namespace Tenacity.Navigation
{
    /// <summary>
    /// Sequential navigation.
    /// </summary>
    public class SimpleNavigation : NavigationContainer
    {
        /// <summary>
        /// Returns next or random (based on container settings) navigation point.
        /// </summary>
        /// <param name="destinationIndex">Relative index of navigation point, updates after choosing new point.</param>
        /// <returns>Position of target navigation  point.</returns>
        public override Vector3 GetDestination(ref int destinationIndex)
        {
            if (Points.Count == 0)
                throw new System.Exception("Can't get destination, navigation is empty.");

            if (_isRandom)
                return GetRandomPosition(ref destinationIndex);

            return GetNextPosition(ref destinationIndex);
        }
    }
}
