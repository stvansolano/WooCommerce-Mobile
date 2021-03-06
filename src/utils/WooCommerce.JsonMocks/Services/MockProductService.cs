﻿using System.Net;
using System.Threading.Tasks;
using Core.Logic.Http;
using Core.Logic.Services;
using WooCommerceNET.WooCommerce.v3;

namespace WooCommerce.Mocks
{
	public class MockProductService : MockHttpFactory<Product>, IProductService
	{
		public async Task<HttpResponse<Product[]>> GetByCategoryId(int categoryId)
		{
			var mockedCall = await base.GetAsync($"/products?categoryId={categoryId}");

			return mockedCall;
		}

		public async Task<HttpResponse<Variation[]>> GetVariations(int productId)
		{
			await Task.Delay(500);

			//return new HttpResponse<Variation[]>(new Variation[0], HttpStatusCode.OK);
			var mockedCall = await base.GetSubTypeAsync<Variation>($"/products/{productId}/variations");
			return mockedCall;
		}

		public async Task<HttpResponse<Product[]>> Search(string criteria)
		{
			var mockedCall = await base.GetAsync($"/products?search={criteria}");

			return mockedCall;
		}
	}
}