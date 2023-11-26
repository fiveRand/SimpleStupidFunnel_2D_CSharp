using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Funnel : MonoBehaviour
{
    /// <summary>
    /// http://jceipek.com/Olin-Coding-Tutorials/pathing.html#funnel-algorithm
    /// </summary>
    public List<Vector2> SimpleStupidFunnel(Vector2 start, Vector2 end, List<Edge> pathEdges, float radius)
    {

        List<Vector2> result = new List<Vector2>(pathEdges.Count);

        int lastTailIndex = 0;
        int lastHeadIndex = 0;
        Vector2 apexPoint = start;
        int i = 1;
        while (true)
        {
            Debug.Log(i);
            Vector2 lastHead = pathEdges[lastHeadIndex].next.position;
            Vector2 lastTail = pathEdges[lastTailIndex].position;

            Vector2 curHead = pathEdges[i].next.position;
            Vector2 curTail = pathEdges[i].position;

            if (isSame(lastHead, curHead) || WherePointLocatedOnTheLine(lastHead, apexPoint, curHead) > 0) // right
            {

                // If is funnel getting narrower or same

                if (WherePointLocatedOnTheLine(lastTail, apexPoint, curHead) > 0) // is curHead crossed the lastTail?
                {
                    apexPoint = lastTail;
                    result.Add(apexPoint);

                    i = UpdateFunnelTail(apexPoint, lastTailIndex, pathEdges);
                    lastTailIndex = i;
                    lastHeadIndex = i;

                    continue;
                }
                else
                {
                    lastHeadIndex = i;
                }

            }

            if (isSame(lastTail, curTail) || WherePointLocatedOnTheLine(lastTail, apexPoint, curTail) < 0) // left
            {


                if (WherePointLocatedOnTheLine(lastHead, apexPoint, curTail) < 0)
                {
                    apexPoint = lastHead;
                    result.Add(apexPoint);
                    i = UpdateFunnelHead(apexPoint, lastHeadIndex, pathEdges);
                    lastTailIndex = i;
                    lastHeadIndex = i;

                    continue;
                }
                else
                {
                    lastTailIndex = i;
                }
            }

            if (i == pathEdges.Count - 1)
            {
                var tailValue = WherePointLocatedOnTheLine(lastTail, apexPoint, end);
                var headValue = WherePointLocatedOnTheLine(lastHead, apexPoint, end);

                if ((tailValue < 0 && headValue > 0) || isSame(apexPoint, lastHead) || isSame(apexPoint, lastTail))
                {
                    break;
                }
                else if (tailValue > 0 && headValue > 0)
                {

                    apexPoint = lastTail;
                    result.Add(apexPoint);
                    i = UpdateFunnelTail(apexPoint, lastTailIndex, pathEdges);
                    lastTailIndex = i;
                    lastHeadIndex = i;

                    if (i == pathEdges.Count - 1)
                    {
                        break;
                    }
                    continue;

                }
                else if (headValue < 0 && tailValue < 0)
                {
                    apexPoint = lastHead;
                    result.Add(apexPoint);

                    i = UpdateFunnelHead(apexPoint, lastHeadIndex, pathEdges);
                    lastTailIndex = i;
                    lastHeadIndex = i;

                    if (i == pathEdges.Count - 1)
                    {
                        break;
                    }
                    continue;
                }
            }
            i++;
        }
        result.Add(end);


        return result;
    }
    public int UpdateFunnelHead(Vector2 apexPoint, int lastHeadIndex, List<Edge> pathEdges)
    {
        int i = 0;
        for (int j = lastHeadIndex; j < pathEdges.Count; j++)
        {
            i = j;
            var candidateHead = pathEdges[j].next.position;

            if (!isSame(apexPoint, candidateHead))
            {
                break;
            }
        }
        return i;
    }
    public int UpdateFunnelTail(Vector2 apexPoint, int lastTailIndex, List<Edge> pathEdges)
    {
        int i = 0;
        for (int j = lastTailIndex; j < pathEdges.Count; j++)
        {
            i = j;
            var candidateTail = pathEdges[j].position;

            if (!isSame(apexPoint, candidateTail))
            {
                break;
            }
        }
        return i;
    }

    public float WherePointLocatedOnTheLine(Vector2 point, Vector2 tail, Vector2 head)
    {
        var tailX = tail.x - point.x;
        var tailY = tail.y - point.y;
        var headX = head.x - point.x;
        var headY = head.y - point.y;
        return headX * tailY - tailX * headY;
    }
    public bool isSame(Vector2 aVec, Vector2 bVec, float toleranceRange = 0.0000001f)
    {
        float dist = Vector2.SqrMagnitude(aVec - bVec);
        return dist < toleranceRange;
    }

    public class Edge
    {
        // edge tail position
        public Vector3 position;
        // edge head position = next.position
        public Edge next;

        public Edge(Vector3 position)
        {
            this.position = position;
        }

    }
}