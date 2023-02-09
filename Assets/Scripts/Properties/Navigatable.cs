using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigatable 
{
    //#region search
    //public List<Vertex> getFoundPath(Vertex goalVertex, Vertex locatedVertex)
    //{
    //    List<SearchVertex> fringe = new List<SearchVertex>();
    //    List<SearchVertex> candidate = new List<SearchVertex>();
    //    List<Vertex> pathFound = new List<Vertex>();

    //    SearchVertex v = makeInitialVertex(locatedVertex);
    //    setInitialVertex(v, fringe);

    //    while (true)
    //    {
    //        SearchVertex currentVertex = getNextVertex(fringe, candidate);
    //        if (goalTest(currentVertex, goalVertex))
    //        {
    //            pathFound = makeOptimalPath(currentVertex, pathFound);
    //            break;
    //        }

    //        SearchVertex[] children = expandVertex(currentVertex);
    //        putFringe(children, goalVertex, fringe, candidate);
    //        sortFringe(fringe);
    //    }
    //    pathFound.RemoveAt(0);
    //    return pathFound;
    //}
    
    //private SearchVertex[] expandVertex(SearchVertex v)
    //{
    //    List<SearchVertex> expand = new List<SearchVertex>();
    //    Vertex vertex = v.getVertex();

    //    foreach (string name in vertex.edges)
    //    {
    //        Vertex childVertex = EnvironmentManager.instance.getVertex(name);
    //        expand.Add(new SearchVertex(v, childVertex));
    //    }

    //    v.setChildren(expand.ToArray());
    //    SearchVertex[] children = expand.ToArray();

    //    return children;
    //}

    //private bool goalTest(SearchVertex currentVertex, Vertex goalVertex)
    //{
    //    Vertex v = currentVertex.getVertex();
    //    if (v.name.Equals(goalVertex.name))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    
    //private SearchVertex makeInitialVertex(Vertex initialVertex)
    //{
    //    return new SearchVertex(null, initialVertex);
    //}


    //private void setInitialVertex(SearchVertex initialVertex, List<SearchVertex> fringe)
    //{
    //    fringe.Add(initialVertex);
    //}

    //private SearchVertex getNextVertex(List<SearchVertex> fringe, List<SearchVertex> candidate)
    //{
    //    if (fringe.Count == 0)
    //    {
    //        Debug.Log("PathNotValid");
    //        throw new System.Exception("PathNotValid");
    //    }
    //    SearchVertex vertex = fringe[0];
    //    fringe.RemoveAt(0);
    //    candidate.Add(vertex);
    //    return vertex;
    //}

    //private void putFringe(SearchVertex[] children, Vertex goalVertex, List<SearchVertex> fringe, List<SearchVertex> candidate)
    //{

    //    SearchVertex currentVertex = children[0].getParent();
    //    float currentVertexPosX = currentVertex.getVertex().posX;
    //    float currentVertexPosY = currentVertex.getVertex().posY;

    //    foreach (SearchVertex childVertex in children)
    //    {
    //        //현재까지 이동한 거리 계산
    //        childVertex.setDistanceTraveledFromStartPosition(
    //            currentVertex.getDistanceTraveledFromStartPosition() +
    //            calcEuclideanDistance(currentVertexPosX, childVertex.getVertex().posX, currentVertexPosY, childVertex.getVertex().posY)
    //        );
    //        //현재까지 이동거리 + 휴리스틱 거리 계산(유클리드 거리)
    //        childVertex.setOptimalDistance(
    //            childVertex.getDistanceTraveledFromStartPosition() +
    //            calcEuclideanDistance(goalVertex.posX, childVertex.getVertex().posX, goalVertex.posY, childVertex.getVertex().posY)
    //        );

    //        if (isVisited(childVertex, candidate))
    //        {
    //            continue;
    //        }
    //        fringe.Add(childVertex);
    //    }
    //}

    //private void sortFringe(List<SearchVertex> fringe)
    //{
    //    fringe.Sort((x1, x2) => x1.getOptimalDistance().CompareTo(x2.getOptimalDistance()));
    //}

    //private List<Vertex> makeOptimalPath(SearchVertex goalVertex, List<Vertex> pathFound)
    //{
    //    SearchVertex searchVertex = goalVertex;
    //    while (searchVertex.getParent() != null)
    //    {
    //        pathFound.Add(searchVertex.getVertex());
    //        SearchVertex parentVertex = searchVertex.getParent();
    //        searchVertex = parentVertex;
    //    }
    //    pathFound.Add(searchVertex.getVertex());
    //    pathFound.Reverse();
    //    return pathFound;
    //}

    //private bool isVisited(SearchVertex vertex, List<SearchVertex> candidate)
    //{
    //    foreach (SearchVertex v in candidate)
    //    {
    //        if (v.getVertex().name == vertex.getVertex().name)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    //private float calcEuclideanDistance(float x1, float x2, float y1, float y2)
    //{
    //    return (float)System.Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    //}
    //#endregion
    
  



}
