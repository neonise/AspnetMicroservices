﻿using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<CatalogController> logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), ((int)HttpStatusCode.OK))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(typeof(Product), ((int)HttpStatusCode.OK))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await productRepository.GetProduct(id);
            if (product == null)
            {
                logger.LogError("not found");
                return NotFound();
            }
            return Ok(product);
        }

        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), ((int)HttpStatusCode.OK))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await productRepository.GetProductByCategory(category);
            return Ok(products);
        }

        [Route("[action]/{name}", Name = "GetProductByName")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), ((int)HttpStatusCode.OK))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
        {
            var products = await productRepository.GetProductByName(name);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), ((int)HttpStatusCode.OK))]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await productRepository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), ((int)HttpStatusCode.OK))]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await productRepository.UpdateProduct(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), ((int)HttpStatusCode.OK))]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            return Ok(await productRepository.DeleteProduct(id));
        }
    }
}
