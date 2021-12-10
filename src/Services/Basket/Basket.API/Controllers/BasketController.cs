using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        [HttpGet("{username}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), ((int)HttpStatusCode.OK))]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
        {
            var basket = await basketRepository.GetBasket(username);
            return Ok(basket ?? new ShoppingCart(username));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), ((int)HttpStatusCode.OK))]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody]ShoppingCart basket)
        {
            return Ok(await basketRepository.UpdateBasket(basket));
        }

        [HttpDelete("{username}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), ((int)HttpStatusCode.OK))]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await basketRepository.DeleteBasket(username);
            return Ok();
        }
    }
}
