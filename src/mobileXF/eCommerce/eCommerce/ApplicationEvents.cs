using System;
using WooCommerceNET.WooCommerce.v3;

namespace eCommerce
{
	public class AddToCartEventArgs : EventArgs
	{
		public AddToCartEventArgs(Product product, int quantity)
		{
			Product = product;
			Quantity = quantity;
		}

		public int Quantity { get; }
		public Product Product { get; }
	}

	public class AddToCartEvent : Prism.Events.PubSubEvent<AddToCartEventArgs>
	{
		public Product Product { get; set; }
		public int Quantity { get; set; }
	}
}
