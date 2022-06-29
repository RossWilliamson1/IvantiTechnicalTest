using TechnicalTest.Core.Interfaces;
using TechnicalTest.Core.Models;

namespace TechnicalTest.Core.Services
{
    public class ShapeService : IShapeService
    {
        public Shape ProcessTriangle(Grid grid, GridValue gridValue)
        {
            // DONE: Calculate the coordinates.
            char letter = gridValue.Row[0];
            int number = gridValue.Column;
            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Dictionary<char, int> lettersToValues = new Dictionary<char, int>();
            for(int i=0; i<alpha.Length; i++) lettersToValues.Add(alpha[i], i*10);
            if((number%2)==0)
            {
                return new Shape(new List<Coordinate> {
                new((number*5)-10, lettersToValues[letter]),
                new(number*5, lettersToValues[letter]),
                new(number*5, lettersToValues[letter]+10)
                });
            }
            else
            {
                return new Shape(new List<Coordinate> {
                new((number*5)-5, lettersToValues[letter]),
                new((number*5)-5, lettersToValues[letter]+10),
                new((number*5)+5, lettersToValues[letter]+10)
                });
            }
        }

        public GridValue ProcessGridValueFromTriangularShape(Grid grid, Triangle triangle)
        {
            // TODO: Calculate the grid value.
            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Dictionary<int, char> valuesToLetters = new Dictionary<int, char>();
            for (int i = 0; i < alpha.Length; i++) valuesToLetters.Add(i*10, alpha[i]);
            if (triangle.TopLeftVertex.Y == triangle.OuterVertex.Y) 
            {
                //number is even
                return new GridValue((char)valuesToLetters[triangle.OuterVertex.Y]-64, 
                    (triangle.OuterVertex.X) / 5);
            }
            else
            {
                //number is odd
                return new GridValue((char)valuesToLetters[triangle.BottomRightVertex.Y - 10]-64, 
                    (triangle.BottomRightVertex.X - 5)/5);
            }
        }
    }
}