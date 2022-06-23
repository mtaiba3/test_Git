using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository itemRepository;

        public ItemsController(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<itemDto>> Get()
        {
            var items = (await itemRepository.GetAllAsync()).Select(i => i.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<itemDto>> GetById(Guid id)
        {
            var item = (await itemRepository.GetAsync(id)).AsDto();
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<itemDto>> Post(CreateItemDto itemCreator)
        {
            var item = new Item()
            {
                Name = itemCreator.Name,
                Description = itemCreator.Description,
                Price = itemCreator.Price,
                CreatedDate = DateTime.UtcNow
            };

            await itemRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetById), new {id = item.Id}, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateItemDto itemUpdator)
        {
            var existingItem = await itemRepository.GetAsync(id);

            if (existingItem != null)
            {
                existingItem.Name = itemUpdator.Name;
                existingItem.Description = itemUpdator.Description;
                existingItem.Price = itemUpdator.Price;

                await itemRepository.UpdateAsync(existingItem);
            }
            else
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await itemRepository.GetAsync(id);

            if (item != null)
            {
                await itemRepository.RemoveAsync(id);
            }
            else
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

