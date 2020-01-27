using System;
using System.Collections.Generic;
using Akka;
using Akka.Actor;
using Akka.Persistence;

namespace Shared.Actors
{
    /// <summary>
    /// The customer actor who manage all the buys of the customer ;D
    /// </summary>
    public class Customer : ReceivePersistentActor
    {
        #region Properties
        public const string TypeName = "customer";
        private static string ShardId => Context.Parent.Path.Name;
        private static string EntityId => Context.Self.Path.Name;
        public override string PersistenceId { get; } = $"{ShardId}/${EntityId}";
        public static readonly TimeSpan InactivityWindow = TimeSpan.FromSeconds(20);
        public Dictionary<string, CartItem> CartItems = new Dictionary<string, CartItem>();
        #endregion

        public Customer()
        {
            Recover<CartItem>(purchased =>
            {
                if (!string.IsNullOrEmpty(purchased.ProductId))
                {
                    CartItems.Add(purchased.ProductId, purchased);
                }
            });
            CommandAny(Handle);
        }

        private void Handle(object message)
        {
            SetReceiveTimeout(InactivityWindow);

            message
                .Match()
                .With<AddItem>(HandleAddItem)
                .With<ShowBasket>(HandleShowBasket)
                .With<EmptyBasket>(HandleEmptyBasket)
                .With<ReceiveTimeout>(HandleReceiveTimeOut)
                .Default(x => Print.Message("Unhandled message type.", ConsoleColor.Red));
        }

        private void HandleAddItem(AddItem purchased)
        {
            if (!CartItems.ContainsKey(purchased.ProductId))
            {
                Persist(new CartItem(purchased.Product, purchased.Quantity, purchased.ProductId), item =>
                {
                    CartItems.Add(item.ProductId, item);
                    Print.Message($"> {EntityId} added {item.Product} x {item.Quantity}");
                });
            }
        }
        private void HandleShowBasket()
        {
            Sender.Tell(new Basket(ShardId, EntityId, CartItems), Self);
            Print.Message($"> {EntityId} requested basket contents.");
        }
        private void HandleEmptyBasket()
        {
            DeleteMessages(long.MaxValue);
            CartItems = new Dictionary<string, CartItem>();
            Print.Message($"> {EntityId} emptied their cart");
        }
        private void HandleReceiveTimeOut()
        {
            Print.Message($"> {EntityId} is inactive. Shutting down..", ConsoleColor.Green);
            Context.Stop(Self);
        }

        protected override void PreStart()
        {
            Print.Message($"Waking up {EntityId} on shard {ShardId}.", ConsoleColor.Green);
            SetReceiveTimeout(InactivityWindow);
        }
    }
}