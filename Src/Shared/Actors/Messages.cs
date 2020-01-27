using System.Collections.Generic;

namespace Shared.Actors
{
    public sealed class AddItem
    {
        public readonly string Product;
        public readonly string ProductId;
        public readonly int Quantity;

        public AddItem(string product, int quantity, string productId)
        {
            Product = product;
            Quantity = quantity;
            ProductId = productId;
        }
    }

    public sealed class CartItem
    {
        public readonly string Product;
        public readonly string ProductId;
        public readonly int Quantity;
        public CartItem(string product, int quantity, string productId)
        {
            Product = product;
            Quantity = quantity;
            ProductId = productId;
        }
    }

    public sealed class ShowBasket
    {
        public static ShowBasket Instance = new ShowBasket();
        private ShowBasket() { }
    }

    public sealed class Basket
    {
        public readonly string ShardId;
        public readonly string EntityId;
        public readonly Dictionary<string, CartItem> Items;

        public Basket(string shardId, string entityId, Dictionary<string, CartItem> items)
        {
            ShardId = shardId;
            EntityId = entityId;
            Items = items;
        }
    }

    public sealed class EmptyBasket
    {
        public static EmptyBasket Instance = new EmptyBasket();
        private EmptyBasket() { }
    }
}