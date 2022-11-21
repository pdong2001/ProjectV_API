using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Services.Interfaces;

namespace API.Controllers
{
    public abstract class PagedCRUDBaseController<TKey, TEntity, TDto, TUpSert, TLookUp> : ControllerBase
        where TEntity : Entity<TKey>
        where TLookUp : PageLookUpDto
    {
        private readonly IPagedCRUDService<TKey, TEntity, TDto, TUpSert, TLookUp> _service;

        public PagedCRUDBaseController(IPagedCRUDService<TKey, TEntity, TDto, TUpSert, TLookUp> service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetAsync([FromRoute] TKey id)
        {
            return Ok(await _service.GetAsync(id));
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreateAsync([FromBody] TUpSert request)
        {
            return Ok(await _service.CreateAsync(request));
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetListAsync()
        {
            return Ok(await _service.GetListAsync());
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> UpdateAsync([FromRoute] TKey id, [FromBody] TUpSert request)
        {
            return Ok(await _service.UpdateAsync(id, request));
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteAsync([FromRoute] TKey id)
        {
            return Ok(await _service.DeleteAsync(id));
        }

        [HttpGet("search")]
        public virtual async Task<IActionResult> SearchAsync([FromQuery] TLookUp request)
        {
            return Ok(await _service.SearchAsync(request));
        }
    }
}
