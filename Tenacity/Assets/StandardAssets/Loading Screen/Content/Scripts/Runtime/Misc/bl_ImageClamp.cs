using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class bl_ImageClamp : MonoBehaviour
{
    public Direction m_Direction = Direction.Right;
    [Range(0.001f, 1)] public float Speed;

    private RawImage m_Image;

    private void Awake()
    {
        m_Image = GetComponent<RawImage>();
    }

    private void Update()
    {
        Rect r = m_Image.uvRect;
        if (m_Direction == Direction.Right)
        {
            r.x -= Time.deltaTime * Speed;
        }else if(m_Direction == Direction.Left)
        {
            r.x += Time.deltaTime * Speed;
        }
        else if (m_Direction == Direction.Up)
        {
            r.y -= Time.deltaTime * Speed;
        }
        else
        {
            r.y += Time.deltaTime * Speed;
        }
        m_Image.uvRect = r;
    }

    [System.Serializable]
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
    }
}