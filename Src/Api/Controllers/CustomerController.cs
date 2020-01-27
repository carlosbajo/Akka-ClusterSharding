using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Actors;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var message = new ShardEnvelope(id, ShowBasket.Instance);
            var timeout = TimeSpan.FromSeconds(10);
            
            var basket = await Actors.Instance.CustomerProxy.Ask<Basket>(message, timeout);
            var items = basket.Items.Select(x => new CartItemDto(
                product: x.Value.Product,
                productId: x.Value.ProductId,
                quantity: x.Value.Quantity
            )).ToList();
            
            return Ok(items);
        }
        
        [HttpPost, Route("{name}")]
        public ActionResult Post([FromBody] AddItemRequest request, string name)
        {
            
            var message = new ShardEnvelope(name, new AddItem(request.Product, request.Quantity, request.ProductId));
            Actors.Instance.CustomerProxy.Tell(message, ActorRefs.NoSender);

            return Ok();
        }

        [HttpDelete, Route("{name}")]
        public ActionResult Delete(string name)
        {
            var message = new ShardEnvelope(name, EmptyBasket.Instance);
            Actors.Instance.CustomerProxy.Tell(message, ActorRefs.NoSender);
            return Ok();
        }

        public class CartItemDto
        {
            public string Product { get; }
            public string ProductId { get; }
            public int Quantity { get; }

            public CartItemDto()
            {
                
            }
            public CartItemDto(string product, int quantity, string productId)
            {
                Product = product;
                Quantity = quantity;
                ProductId = productId;
            }
        }
        public class AddItemRequest
        {
            public string Product { get; set; }
            public string ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}