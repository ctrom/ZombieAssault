using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieAssault
{
    class Pathfinder
    {
        int[,] matrix;
        int columns = 44;
        int rows = 44;
        int end_column = 0;
        int end_row = 0;
        bool value_not_found = true;
        int value_at_end;

        private void check_cross(bool value_not_found, int row, int column, int current_value)
        {
            int new_value = current_value + 1;
            if (value_not_found && (matrix[row + 1, column] == 2 || matrix[row + 1, column] == 3))
            {
                matrix[row-1,column] = new_value;
                check_cross(value_not_found, row-1, column, new_value);
            }
            if (value_not_found && (matrix[row + 1, column] == 2 || matrix[row + 1, column] == 3))
            {
                matrix[row+1,column] = new_value;
                check_cross(value_not_found, row+1, column, new_value);
            }
            if (value_not_found && matrix[row+1,column] == -1)
            {
                matrix[row+1,column] = new_value;
                end_column = column;
                end_row = row+1;
                value_not_found = false;
            }
            if (value_not_found && (matrix[row + 1, column] == 2 || matrix[row + 1, column] == 3))
            {
                matrix[row,column-1] = new_value;
                check_cross(value_not_found, row, column-1, new_value);
            }
            if (value_not_found && (matrix[row + 1, column] == 2 || matrix[row + 1, column] == 3))
            {
                matrix[row,column+1] = new_value;
                check_cross(value_not_found, row, column+1, new_value);
            }
        }

        private void mark_path(int row, int column, int current_value)
        {
            try
            {
                if (matrix[row-1,column] < current_value)
                {
                    matrix[row-1,column] = '+';
                    mark_path(row-1, column, current_value-1);
                }
            }
            catch( Exception)
            {}
            try {
                if (matrix[row+1,column] < current_value) 
                {
                    matrix[row+1,column] = '+';
                    mark_path(row+1, column, current_value-1);
                }

            }catch( Exception){}
            try{
                if (matrix[row,column-1] < current_value) {

                
                    matrix[row,column-1] = '+';
                    mark_path(row, column-1, current_value-1);
                }
            }catch( Exception){}
            try{
                if (matrix[row,column+1] < current_value) 
                {
                    matrix[row,column+1] = '+';
                    mark_path(row, column+1, current_value-1);
                }
            }catch( Exception){}
            }

        public Pathfinder(Map map)
        {
            this.matrix = map.Matrix;
            value_at_end = matrix[end_row,end_column];
        }


        

        //private void calc()
        //{
        //    for( int i = 0; int < rows; int++)
        //    {
        //        string result = "";
        //        for(int j = 0; j < columns; j++)
        //        {
        //            if(matrix[i,j] != 0)
        //            {
        //                if( i == 0)
        //                    matrix[row,column] = 0;
        //                elif row == rows - 1:
        //                    matrix[row,column] = 'E'
        //                    end_column = column
        //                    end_row = row
        //                else:
        //                    matrix[row,column] = 'U'
        //            }
        //            result += str(matrix[row,column])
        //        }
        //        print(result)
        //    }
        //}

        private void incTile()
        {
           int current_value = 0;
           while (matrix[end_row,end_column] == 'E')
           {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0;j < columns; j++)
                    {
                        if (matrix[i,j] == current_value)
                            check_cross(value_not_found, i, j, matrix[i,j]);
                    }
                }
                current_value += 1;
           }
        }

        
        //matrix[end_row,end_column] = '+'
        //matrix[end_row-1,end_column] = '+'
        public List<Vector2> findPath(MapNode currPosition, MapNode destination)
        {
            mark_path(end_row-1, end_column, value_at_end-1);
            return null; //TODO Return value
        }

        private void resetMatrix()
        {
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    if (matrix[i,j] > 2 || matrix[i,j] == -1)
                        matrix[i,j] = 2;
                }
            }
        }
    }
}
