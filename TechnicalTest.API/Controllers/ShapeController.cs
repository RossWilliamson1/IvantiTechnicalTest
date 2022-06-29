using Microsoft.AspNetCore.Mvc;
using TechnicalTest.API.DTOs;
using TechnicalTest.Core;
using TechnicalTest.Core.Interfaces;
using TechnicalTest.Core.Models;

namespace TechnicalTest.API.Controllers
{
    /// <summary>
    /// Shape Controller which is responsible for calculating coordinates and grid value.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ShapeController : ControllerBase
    {
        private readonly IShapeFactory _shapeFactory;

        /// <summary>
        /// Constructor of the Shape Controller.
        /// </summary>
        /// <param name="shapeFactory"></param>
        public ShapeController(IShapeFactory shapeFactory)
        {
            _shapeFactory = shapeFactory;
        }

        /// <summary>
        /// Calculates the Coordinates of a shape given the Grid Value.
        /// </summary>
        /// <param name="calculateCoordinatesRequest"></param>   
        /// <returns>A Coordinates response with a list of coordinates.</returns>
        /// <response code="200">Returns the Coordinates response model.</response>
        /// <response code="400">If an error occurred while calculating the Coordinates.</response>   
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Shape))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("CalculateCoordinates")]
        [HttpPost]
        public IActionResult CalculateCoordinates([FromBody]CalculateCoordinatesDTO calculateCoordinatesRequest)
        {
            // DONE: Get the ShapeEnum and if it is default (ShapeEnum.None) or not triangle, return BadRequest as only Triangle is implemented yet.
            ShapeEnum triangleEnum = ShapeEnum.Triangle;
            if(!(calculateCoordinatesRequest.ShapeType.Equals(triangleEnum))) return BadRequest();
           
            // DONE: Call the Calculate function in the shape factory.

            Grid g = new Grid(calculateCoordinatesRequest.Grid.Size);
            GridValue gv = new GridValue(calculateCoordinatesRequest.GridValue);
            Shape? calculateResult = _shapeFactory.CalculateCoordinates((ShapeEnum)calculateCoordinatesRequest.ShapeType, g, gv);

            // DONE: Return BadRequest with error message if the calculate result is null
            if(calculateResult is null)
            {
                return BadRequest();
            }
            // DONE: Create ResponseModel with Coordinates and return as OK with responseModel.
            CalculateCoordinatesResponseDTO responseModel = new CalculateCoordinatesResponseDTO();
            List<CalculateCoordinatesResponseDTO.Coordinate> responseCoords = 
                new List<CalculateCoordinatesResponseDTO.Coordinate>();

            foreach(Coordinate c in calculateResult.Coordinates) 
                responseCoords.Add(new CalculateCoordinatesResponseDTO.Coordinate(c.X, c.Y));

            responseModel.Coordinates = responseCoords;
            return Ok(responseModel);
        }

        /// <summary>
        /// Calculates the Grid Value of a shape given the Coordinates.
        /// </summary>
        /// <remarks>
        /// A Triangle Shape must have 3 vertices, in this order: Top Left Vertex, Outer Vertex, Bottom Right Vertex.
        /// </remarks>
        /// <param name="gridValueRequest"></param>   
        /// <returns>A Grid Value response with a Row and a Column.</returns>
        /// <response code="200">Returns the Grid Value response model.</response>
        /// <response code="400">If an error occurred while calculating the Grid Value.</response>   
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GridValue))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("CalculateGridValue")]
        [HttpPost]
        public IActionResult CalculateGridValue([FromBody]CalculateGridValueDTO gridValueRequest)
        {
            // DONE: Get the ShapeEnum and if it is default (ShapeEnum.None) or not triangle, return BadRequestas only Triangle is implemented yet.
            ShapeEnum triangleEnum = ShapeEnum.Triangle;
            if (!(gridValueRequest.ShapeType.Equals(triangleEnum))) return BadRequest();

            // DONE: Create new Shape with coordinates based on the parameters from the DTO.
            Grid grid = new Grid(gridValueRequest.Grid.Size);


            List<Coordinate> coords = new List<Coordinate>();

            foreach (Vertex v in gridValueRequest.Vertices)
            {
                Coordinate c = new Coordinate(v.x, v.y);
                coords.Add(c);
            }


            Shape DTOShape = new Shape(coords);


            // DONE: Call the function in the shape factory to calculate grid value.

            GridValue? gv = _shapeFactory.CalculateGridValue((ShapeEnum)gridValueRequest.ShapeType, grid, DTOShape);

            // DONE: If the GridValue result is null then return BadRequest with an error message.
            if (gv is null) return BadRequest("Could not calculate grid value");

            // DONE: Generate a ResponseModel based on the result and return it in Ok();

            return Ok(new CalculateGridValueResponseDTO(gv.Row, gv.Column));
        }
    }
}
