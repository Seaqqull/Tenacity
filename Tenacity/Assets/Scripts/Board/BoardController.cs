using System.Collections;
using System.Collections.Generic;
using Tenacity.Lands;
using TMPro;
using UnityEngine;

namespace Tenacity.Battle {

    public class BoardController : MonoBehaviour
    {
        [SerializeField] private Board board;
        [SerializeField] private Land emptyLandPrefab;
        [SerializeField] private int mapRadius = 4;

        [SerializeField] private TextMeshPro textPro;

        private void Start()
        {
            if (board == null) return;
            if (emptyLandPrefab == null || !emptyLandPrefab.GetComponent<Land>()) return;
            InitBoard();
        }

        private void InitBoard()
        {
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
                emptyLandPrefab.gameObject,
                new Vector3(x, 0, y),
                Quaternion.identity);
            landGO.transform.parent = transform;
            landGO.GetComponent<Land>().IsPlacedOnBoard = true;
            TextMeshPro text = Instantiate(textPro, landGO.transform, true);
            text.text = $"[{x}; {y}]";
            text.transform.localPosition = new Vector3(0, 1, 0);
            board.AddCell(landGO.GetComponent<Land>());
        }

    }
}