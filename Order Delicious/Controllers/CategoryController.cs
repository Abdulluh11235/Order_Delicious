using Application.Commands;
using Application.Services.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Order_Delicious.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
       private readonly ICategoryService _categoryService;
        readonly ILogger <CategoryController> _logger;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
    /// <summary>
    ///  API For Adding a category for items in the database
    /// </summary>
    /// <param name="createCmd"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>OK</returns>
  
      [HttpPost]
      [ProducesResponseType(StatusCodes.Status201Created)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(CreateCategory createCmd,CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _categoryService.Create(createCmd, cancellationToken);
            if(!result.IsSuccess) return BadRequest(result.ErrorMessage); 
            return Created();
        }
     /// <summary>
     /// API For Getting Categories In Pages 
     /// </summary>
     /// <param name="pageNumber"></param>
     /// <param name="pageSize"></param>
     /// <param name="cancellationToken"></param>
     /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public async Task<IActionResult> GetPage(int pageNumber,int pageSize,CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetCategoryPaged(pageNumber, pageSize, cancellationToken);
            if(!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Value);
        }
       /// <summary>
       /// Get Category By id
       /// </summary>
       /// <param name="id"></param>
       /// <param name="cancellationToken"></param>
       /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _categoryService.GetById( id, cancellationToken);
            if(!result.IsSuccess) return BadRequest(result.ErrorMessage);
            var val=result.Value;
            if (val==null) return NotFound($"Category not found Id = {id }");
            return Ok(val);
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="updateCategory"></param>
       /// <param name="cancellationToken"></param>
       /// <returns></returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id,UpdateCategory updateCategory,CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var result = await  _categoryService.Update(id,updateCategory,cancellationToken);
            if(result is { IsSuccess: false, ErrorMessage: not null } && result.ErrorMessage.Equals(Result<int>.NotFoundError)) 
                return NotFound();
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok();
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="ids"></param>
       /// <param name="cancellationToken"></param>
       /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveRange([FromBody] IEnumerable<int> ids,
            CancellationToken cancellationToken)
        {
             var result = await _categoryService.RemoveRange(ids, cancellationToken);
             if(!result.IsSuccess) return BadRequest(result.ErrorMessage);
             return NoContent();
        }
        
        /// <summary>
        /// Removing Category By id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveById(int id,
            CancellationToken cancellationToken)
        {
            var result = await _categoryService.RemoveById(id, cancellationToken);
            if(!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return NoContent();
        }
    }
}   
