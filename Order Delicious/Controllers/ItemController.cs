using Application.Commands;
using Application.Services.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Order_Delicious.Controllers
{
    [Route("api/v1/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService  _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }
         /// <summary>
         ///  Creates food item 
         /// </summary>
         /// <param name="createItem"></param>
         /// <param name="cancellationToken"></param>
         /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(CreateItem createItem,CancellationToken cancellationToken)
        {
          var res= await _itemService.Create(createItem,cancellationToken);
          if (!res.IsSuccess) return BadRequest();
           return CreatedAtAction(nameof(GetById),new  { id = res.Value }, res.Value);
        }
        /// <summary>
        /// API For Getting Items In Pages 
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
            var result = await _itemService.GetItemPaged(pageNumber, pageSize, cancellationToken);
            if(!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Value);
        }
         
         
        /// <summary>
        /// Get Item By id
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
            var res = await _itemService.GetById(id,cancellationToken);
            if(!res.IsSuccess && res.ErrorMessage == Result<int>.NotFoundError) return NotFound();
            if(!res.IsSuccess) return BadRequest();
            return Ok(res.Value);
        }
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id,UpdateItem updateItem,CancellationToken cancellationToken)
        {
            var result = await  _itemService.Update(id,updateItem, cancellationToken);
            if(!result.IsSuccess && result.ErrorMessage==Result<int>.NotFoundError) 
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
            var result = await _itemService.RemoveRange(ids, cancellationToken);
            if(!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return NoContent();
        }
        
        /// <summary>
        /// Removing Item By id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveById( int id,
            CancellationToken cancellationToken)
        {
            var result = await _itemService.RemoveById(id, cancellationToken);
            if(!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return NoContent();
        }

        
    }
}
