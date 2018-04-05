using System.Collections.Generic;
using UnityEngine;

public class GridBase : MonoBehaviour
{
    private Node[,] m_Grid;
    public Vector2 m_GridSize;
    public float m_NodeRadius;
    public LayerMask m_Layer;
    public Stack<Node> m_Path = new Stack<Node>();
    private float m_NodeDiameter;
    private int m_GridCountX;
    private int m_GridCountY;

    void Start()
    {
        m_NodeDiameter = m_NodeRadius * 2;
        m_GridCountX = Mathf.RoundToInt(m_GridSize.x / m_NodeDiameter);
        m_GridCountY = Mathf.RoundToInt(m_GridSize.y / m_NodeDiameter);
        m_Grid = new Node[m_GridCountX, m_GridCountY];
        CreateGrid();
    }

    /// <summary>
    /// 创建格子
    /// </summary>
    private void CreateGrid()
    {
        Vector3 startPos = transform.position;
        startPos.x = startPos.x - m_GridSize.x / 2;
        startPos.z = startPos.z - m_GridSize.y / 2;
        for (int i = 0; i < m_GridCountX; i++)
        {
            for (int j = 0; j < m_GridCountY; j++)
            {
                Vector3 worldPos = startPos;
                worldPos.x = worldPos.x + i * m_NodeDiameter + m_NodeRadius;
                worldPos.z = worldPos.z + j * m_NodeDiameter + m_NodeRadius;
                bool canWalk = !Physics.CheckSphere(worldPos, m_NodeRadius, m_Layer);
                m_Grid[i, j] = new Node(canWalk, worldPos, i, j);
            }
        }
    }

    /// <summary>
    /// 通过空间位置获得对应的节点
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Node GetFromPosition(Vector3 pos)
    {
        float percentX = (pos.x + m_GridSize.x / 2) / m_GridSize.x;
        float percentZ = (pos.z + m_GridSize.y / 2) / m_GridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);
        int x = Mathf.RoundToInt((m_GridCountX - 1) * percentX);
        int z = Mathf.RoundToInt((m_GridCountY - 1) * percentZ);
        return m_Grid[x, z];
    }

    /// <summary>
    /// 获得当前节点的相邻节点
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<Node> GetNeighor(Node node)
    {
        List<Node> neighborList = new List<Node>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                int tempX = node.m_GridX + i;
                int tempY = node.m_GridY + j;
                if (tempX < m_GridCountX && tempX > 0 && tempY > 0 && tempY < m_GridCountY)
                {
                    neighborList.Add(m_Grid[tempX, tempY]);
                }
            }
        }
        return neighborList;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(m_GridSize.x, 1, m_GridSize.y));
        if (m_Grid == null)
        {
            return;
        }
        foreach (var node in m_Grid)
        {
            Gizmos.color = node.m_CanWalk ? Color.white : Color.red;
            Gizmos.DrawCube(node.m_WorldPos, Vector3.one * (m_NodeDiameter - 0.1f));
        }
        if (m_Path != null)
        {
            foreach (var node in m_Path)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(node.m_WorldPos, Vector3.one * (m_NodeDiameter - 0.1f));
            }
        }
    }
}
