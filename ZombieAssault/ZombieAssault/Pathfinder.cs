using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieAssault
{
    public class Pathfinder
    {

        private SearchNode[,] searchNodes;

        private int levelWidth;

        private int levelHeight;

        private List<int> passableTiles;

        public Pathfinder(Map map, List<int> passableTiles)
        {

            levelWidth = map.Width;

            levelHeight = map.Height;

            this.passableTiles = passableTiles;

            InitializeSearchNodes(map);
        }

        private void InitializeSearchNodes(Map map)
        {
            searchNodes = new SearchNode[levelWidth, levelHeight];

            for (int x = 0; x < levelWidth; x++)
            {

                for (int y = 0; y < levelHeight; y++)
                {

                    SearchNode node = new SearchNode();



                    node.Position = new Point(x, y);

                    node.Walkable = passableTiles.Contains(Map.getNode(new Vector2(x, y)).Type);

                    if (node.Walkable == true)
                    {

                        node.Neighbors = new SearchNode[4];

                        searchNodes[x, y] = node;

                    }

                }

            }

            for (int x = 0; x < levelWidth; x++)
            {

                for (int y = 0; y < levelHeight; y++)
                {

                    SearchNode node = searchNodes[x, y];

                    if (node == null || node.Walkable == false)
                    {

                        continue;

                    }

                    Point[] neighbors = new Point[]
                    {

                        new Point (x, y - 1), // The node above the current node

                        new Point (x, y + 1), // The node below the current node.

                        new Point (x - 1, y), // The node left of the current node.

                        new Point (x + 1, y), // The node right of the current node

                    };

                    for (int i = 0; i < neighbors.Length; i++)
                    {

                        Point position = neighbors[i];

                        if (position.X < 0 || position.X > levelWidth - 1 ||

                            position.Y < 0 || position.Y > levelHeight - 1)
                        {

                            continue;

                        }



                        SearchNode neighbor = searchNodes[position.X, position.Y];

                        if (neighbor == null || neighbor.Walkable == false)
                        {

                            continue;

                        }

                        node.Neighbors[i] = neighbor;

                    }

                }

            }
        }
        private List<SearchNode> openList = new List<SearchNode>();
        private List<SearchNode> closedList = new List<SearchNode>();

        private float Heuristic(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) +
                   Math.Abs(point1.Y - point2.Y);
        }

        private void ResetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();

            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    SearchNode node = searchNodes[x, y];

                    if (node == null)
                    {
                        continue;
                    }

                    node.InOpenList = false;
                    node.InClosedList = false;

                    node.DistanceTraveled = float.MaxValue;
                    node.DistanceToGoal = float.MaxValue;
                }
            }
        }

        private SearchNode FindBestNode()
        {
            SearchNode currentTile = openList[0];

            float smallestDistanceToGoal = float.MaxValue;
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].DistanceToGoal < smallestDistanceToGoal)
                {
                    currentTile = openList[i];
                    smallestDistanceToGoal = currentTile.DistanceToGoal;
                }
            }
            return currentTile;
        }

        private List<Vector2> FindFinalPath(SearchNode startNode, SearchNode endNode)
        {
            closedList.Add(endNode);

            SearchNode parentTile = endNode.Parent;
            while (parentTile != startNode)
            {
                closedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }

            List<Vector2> finalPath = new List<Vector2>();
            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(new Vector2(closedList[i].Position.X * SpriteManager.tileSize + Game1.resOffset - SpriteManager.gridOffset - SpriteManager.tileSize,
                                          closedList[i].Position.Y * SpriteManager.tileSize - 2 * SpriteManager.tileSize));
            }

            return finalPath;
        }

        public List<Vector2> FindPath(Point startPoint, Point endPoint)
        {
            //try
            //{
                if (startPoint == endPoint)
                {
                    return new List<Vector2>();
                }
                ResetSearchNodes();
                SearchNode startNode = searchNodes[startPoint.X, startPoint.Y];
                SearchNode endNode = searchNodes[endPoint.X, endPoint.Y];
                if (startNode != null)
                {
                    startNode.InOpenList = true;

                    startNode.DistanceToGoal = Heuristic(startPoint, endPoint);
                    startNode.DistanceTraveled = 0;

                    openList.Add(startNode);
                }
                while (openList.Count > 0)
                {
                    SearchNode currentNode = FindBestNode();
                    if (currentNode == null)
                    {
                        break;
                    }
                    if (currentNode == endNode)
                    {
                        return FindFinalPath(startNode, endNode);
                    }
                    for (int i = 0; i < currentNode.Neighbors.Length; i++)
                    {
                        SearchNode neighbor = currentNode.Neighbors[i];
                        if (neighbor == null || neighbor.Walkable == false)
                        {
                            continue;
                        }
                        float distanceTraveled = currentNode.DistanceTraveled + 1;
                        float heuristic = Heuristic(neighbor.Position, endPoint);
                        if (neighbor.InOpenList == false && neighbor.InClosedList == false)
                        {
                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + heuristic;
                            neighbor.Parent = currentNode;
                            neighbor.InOpenList = true;
                            openList.Add(neighbor);
                        }
                        else if (neighbor.InOpenList || neighbor.InClosedList)
                        {
                            if (neighbor.DistanceTraveled > distanceTraveled)
                            {
                                neighbor.DistanceTraveled = distanceTraveled;
                                neighbor.DistanceToGoal = distanceTraveled + heuristic;

                                neighbor.Parent = currentNode;
                            }
                        }
                    }
                    openList.Remove(currentNode);
                    currentNode.InClosedList = true;
                }
                return new List<Vector2>();
            //}
            //catch (Exception)
            //{ Console.WriteLine(startPoint.X+":"+ startPoint.Y);
            //return new List<Vector2>();
            //}
        }

        private class SearchNode
        {
            public Point Position;

            public bool Walkable;

            public SearchNode[] Neighbors;
            public SearchNode Parent;
            public bool InOpenList;
            public bool InClosedList;
            public float DistanceToGoal;
            public float DistanceTraveled;
        }





        //    int[,] matrix;
        //    int columns = 44;
        //    int rows = 44;
        //    int end_column;
        //    int end_row;
        //    bool value_not_found = true;
        //    int value_at_end;
        //    List<MapNode> path;

        //    private void check_cross(bool value_not_found, int row, int column, int current_value)
        //    {
        //        int new_value = current_value + 1;
        //        try
        //        {
        //            if (value_not_found && matrix[row - 1, column] == -1)
        //            {
        //                Console.WriteLine("* ");
        //                matrix[row - 1, column] = new_value;
        //                end_column = column;
        //                end_row = row - 1;
        //                value_not_found = false;
        //            }
        //        }
        //        catch (Exception) { Console.WriteLine(1); }
        //        try
        //        {
        //            if (value_not_found && matrix[row - 1, column] == 2)
        //            {
        //                matrix[row - 1, column] = new_value;
        //                check_cross(value_not_found, row - 1, column, new_value);
        //            }
        //        }
        //        catch (Exception) { Console.WriteLine(2); }
        //        try
        //        {
        //            if (value_not_found && matrix[row + 1, column] == -1)
        //            {
        //                Console.WriteLine("** ");
        //                matrix[row + 1, column] = new_value;
        //                end_column = column;
        //                end_row = row + 1;
        //                value_not_found = false;
        //            }
        //        }
        //        catch (Exception) { Console.WriteLine(3); }
        //        try
        //        {
        //            if (value_not_found && matrix[row + 1, column] == 2)
        //            {
        //                matrix[row + 1, column] = new_value;
        //                check_cross(value_not_found, row + 1, column, new_value);
        //            }
        //        }
        //        catch (Exception) { Console.WriteLine(4); }
        //        try
        //        {
        //            if (value_not_found && matrix[row, column - 1] == -1)
        //            {
        //                Console.WriteLine("*** ");
        //                matrix[row, column - 1] = new_value;
        //                end_column = column - 1;
        //                end_row = row;
        //                value_not_found = false;
        //            }
        //        }
        //        catch (Exception) { Console.WriteLine(5); }
        //        try
        //        {
        //            if (value_not_found && matrix[row, column - 1] == 2)
        //            {
        //                matrix[row, column - 1] = new_value;
        //                check_cross(value_not_found, row, column - 1, new_value);
        //            }
        //        }
        //        catch (Exception) { Console.WriteLine(6); }
        //        try
        //        {
        //            if (value_not_found && matrix[row, column + 1] == -1)
        //            {
        //                Console.WriteLine("**** ");
        //                matrix[row, column + 1] = new_value;
        //                end_column = column + 1;
        //                end_row = row;
        //                value_not_found = false;
        //            }
        //        }
        //        catch (Exception) { Console.WriteLine(7); }
        //        try
        //        {
        //            if (value_not_found && matrix[row, column + 1] == 2)
        //            {
        //                matrix[row, column + 1] = new_value;
        //                check_cross(value_not_found, row, column + 1, new_value);
        //            }
        //        }
        //        catch (Exception) { Console.WriteLine(8); }

        //    }

        //    private void mark_path(int row, int column, int current_value)
        //    {
        //        try
        //        {
        //            if (matrix[row-1,column] < current_value)
        //            {
        //                path.Add(Map.getNode(new Vector2(row-1,column)));
        //                mark_path(row-1, column, current_value-1);
        //            }
        //        }
        //        catch( Exception){}
        //        try {
        //            if (matrix[row+1,column] < current_value) 
        //            {
        //                path.Add(Map.getNode(new Vector2(row - 1, column)));
        //                //matrix[row+1,column] = '+';
        //                mark_path(row+1, column, current_value-1);
        //            }

        //        }catch( Exception){}
        //        try
        //        {
        //            if (matrix[row,column-1] < current_value)
        //            {
        //                path.Add(Map.getNode(new Vector2(row - 1, column)));
        //                //matrix[row,column-1] = '+';
        //                mark_path(row, column-1, current_value-1);
        //            }
        //        }catch( Exception){}
        //        try
        //        {
        //            if (matrix[row,column+1] < current_value) 
        //            {
        //                path.Add(Map.getNode(new Vector2(row - 1, column)));
        //                //matrix[row,column+1] = '+';
        //                mark_path(row, column+1, current_value-1);
        //            }
        //        }catch( Exception){}
        //        }

        //    public Pathfinder(int[,] matrix)
        //    {
        //        this.matrix = matrix;
        //        path = new List<MapNode>();
        //    }

        //    //private void calc()
        //    //{
        //    //    for( int i = 0; int < rows; int++)
        //    //    {
        //    //        string result = "";
        //    //        for(int j = 0; j < columns; j++)
        //    //        {
        //    //            if(matrix[i,j] != 0)
        //    //            {
        //    //                if( i == 0)
        //    //                    matrix[row,column] = 0;
        //    //                elif row == rows - 1:
        //    //                    matrix[row,column] = 'E'
        //    //                    end_column = column
        //    //                    end_row = row
        //    //                else:
        //    //                    matrix[row,column] = 'U'
        //    //            }
        //    //            result += str(matrix[row,column])
        //    //        }
        //    //        print(result)
        //    //    }
        //    //}

        //    private void incTiles()
        //    {
        //       int current_value = -2;
        //       while (matrix[end_row,end_column] == -1)
        //       {
        //            for (int i = 0; i < rows; i++)
        //            {
        //                for (int j = 0;j < columns; j++)
        //                {
        //                    if (matrix[i,j] == current_value)
        //                        check_cross(value_not_found, i, j, matrix[i,j]+ 4);
        //                }
        //            }
        //            current_value += 1;
        //       }
        //    }


        //    //matrix[end_row,end_column] = '+'
        //    //matrix[end_row-1,end_column] = '+'
        //    public List<MapNode> findPath(MapNode currPosition, MapNode destination)
        //    {
        //        path.Clear();
        //        resetMatrix();
        //        matrix[(int)currPosition.Index.Y,(int)currPosition.Index.X] = -2;
        //        end_row = (int)destination.Index.Y;
        //        end_column = (int)destination.Index.X;
        //        matrix[end_row, end_column] = -1;
        //        incTiles();
        //        for (int i = 0; i < rows; i++ )
        //        {
        //            for( int j = 0; j < columns; j++)
        //            {
        //                Console.Write(matrix[i, j]+"\t");
        //            }
        //            Console.WriteLine();
        //        }
        //        end_row = (int)destination.Index.X;
        //        end_column = (int)destination.Index.Y;
        //        value_at_end = matrix[end_row, end_column];
        //        mark_path(end_row/*-1*/, end_column, value_at_end/*-1*/);
        //        Console.WriteLine(path.Count);
        //        return path;
        //    }

        //    private void resetMatrix()
        //    {
        //        for(int i = 0; i < rows; i++)
        //        {
        //            for(int j = 0; j < columns; j++)
        //            {
        //                if (matrix[i,j] > 2 || matrix[i,j] == -1 || matrix[i,j] == -2)
        //                    matrix[i,j] = 2;
        //            }
        //        }
        //    }
        //}
    }
}
