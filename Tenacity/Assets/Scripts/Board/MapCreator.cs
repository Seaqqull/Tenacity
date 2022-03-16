using System.Collections;
using System.Collections.Generic;
using Tenacity.Lands;
using UnityEngine;

namespace Tenacity.Board {
    public class MapCreator : MonoBehaviour
    {
        [SerializeField] private GameObject emptyLandPrefab;
        [SerializeField] [Range(4, 10)] private int mapRadius = 4;

        private void Start()
        {
            if (emptyLandPrefab == null) return;
            if (!emptyLandPrefab.GetComponent<Land>()) return;

            float colLimit = 2 * (mapRadius - 1);

            for (float row = -mapRadius + 1; row < mapRadius; row++)
            {
                for (float col = -(colLimit - Mathf.Abs(row)) / 2; col <= (colLimit - Mathf.Abs(row)) / 2; col++)
                {
                    CreateLandCell(row, col);
                }
            }
        }

        private void CreateLandCell(float y, float x)
        {

            var landGO = Instantiate(
                emptyLandPrefab,
                new Vector3(x, 0, y),
                Quaternion.identity);
            landGO.transform.parent = transform;
            Land land = landGO.GetComponent<Land>();
            land.IsPlacedOnBoard = true;
        }

    }
}