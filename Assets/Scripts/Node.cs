using UnityEngine;

public class Node
{
    //是否可以通过
    public bool m_CanWalk;
    //节点空间位置
    public Vector3 m_WorldPos;
    //节点在数组的位置
    public int m_GridX;
    public int m_GridY;
    //开始节点到当前节点的距离估值
    public int m_gCost;
    //当前节点到目标节点的距离估值
    public int m_hCost;

    public int FCost
    {
        get { return m_gCost + m_hCost; }
    }
    //当前节点的父节点
    public Node m_Parent;

    public Node(bool canWalk, Vector3 position, int gridX, int gridY)
    {
        m_CanWalk = canWalk;
        m_WorldPos = position;
        m_GridX = gridX;
        m_GridY = gridY;
    }
}
