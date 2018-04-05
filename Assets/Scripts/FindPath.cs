using System.Collections.Generic;
using UnityEngine;

public class FindPath : MonoBehaviour
{
    public Transform m_StartNode;
    public Transform m_EndNode;
    private GridBase m_Grid;
    private List<Node> openList = new List<Node>();
    private HashSet<Node> closeSet = new HashSet<Node>();

    void Start()
    {
        m_Grid = GetComponent<GridBase>();
    }

    void Update()
    {
        FindingPath(m_StartNode.position, m_EndNode.position);
    }

    /// <summary>
    /// A*算法，寻找最短路径
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    private void FindingPath(Vector3 start, Vector3 end)
    {
        Node startNode = m_Grid.GetFromPosition(start);
        Node endNode = m_Grid.GetFromPosition(end);
        openList.Clear();
        closeSet.Clear();
        openList.Add(startNode);
        int count = openList.Count;
        while (count > 0)
        {
            // 寻找开启列表中的F最小的节点，如果F相同，选取H最小的
            Node currentNode = openList[0];
            for (int i = 0; i < count; i++)
            {
                Node node = openList[i];
                if (node.FCost < currentNode.FCost || node.FCost == currentNode.FCost && node.m_hCost < currentNode.m_hCost)
                {
                    currentNode = node;
                }
            }
            // 把当前节点从开启列表中移除，并加入到关闭列表中
            openList.Remove(currentNode);
            closeSet.Add(currentNode);
            // 如果是目的节点，返回
            if (currentNode == endNode)
            {
                GeneratePath(startNode, endNode);
                return;
            }
            // 搜索当前节点的所有相邻节点
            foreach (var node in m_Grid.GetNeighor(currentNode))
            {
                // 如果节点不可通过或者已在关闭列表中，跳出
                if (!node.m_CanWalk || closeSet.Contains(node))
                {
                    continue;
                }
                int gCost = currentNode.m_gCost + GetDistanceNodes(currentNode, node);
                // 如果新路径到相邻点的距离更短 或者不在开启列表中
                if (gCost < node.m_gCost || !openList.Contains(node))
                {
                    // 更新相邻点的F，G，H
                    node.m_gCost = gCost;
                    node.m_hCost = GetDistanceNodes(node, endNode);
                    // 设置相邻点的父节点为当前节点
                    node.m_Parent = currentNode;
                    // 如果不在开启列表中，加入到开启列表中
                    if (!openList.Contains(node))
                    {
                        openList.Add(node);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 生成路径
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    private void GeneratePath(Node startNode, Node endNode)
    {
        Stack<Node> path = new Stack<Node>();
        Node node = endNode;
        while (node.m_Parent != startNode)
        {
            path.Push(node);
            node = node.m_Parent;
        }
        m_Grid.m_Path = path;
    }

    /// <summary>
    /// 获得两个节点的距离
    /// </summary>
    /// <param name="node1"></param>
    /// <param name="node2"></param>
    /// <returns></returns>
    private int GetDistanceNodes(Node node1, Node node2)
    {
        int deltaX = Mathf.Abs(node1.m_GridX - node2.m_GridX);
        int deltaY = Mathf.Abs(node1.m_GridY - node2.m_GridY);
        if (deltaX > deltaY)
        {
            return deltaY * 14 + 10 * (deltaX - deltaY);
        }
        else
        {
            return deltaX * 14 + 10 * (deltaY - deltaX);
        }
    }
}
