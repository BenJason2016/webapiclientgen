﻿using System;
using Xunit;

namespace IntegrationTests
{
	public partial class DotNetCoreSpecial : IClassFixture<SuperDemoFixture>
	{
		public DotNetCoreSpecial(SuperDemoFixture fixture)
		{
			api = fixture.Api;
		}

		DemoWebApi.Controllers.Client.SuperDemo api;

		[Fact]
		public void TestGetDictionary()
		{
			var dic = api.GetDictionary();
			Assert.Equal("number", dic["System.Int64"]);
		}

		[Fact]
		public void TestGetDictionaryOfPeople()
		{
			var dic = api.GetDictionaryOfPeople();
			Assert.Equal("Tony Stark", dic["Iron Man"].Name);
			Assert.Equal("New York", dic["Spider Man"].Addresses[0].City);
			Assert.Throws< System.Collections.Generic.KeyNotFoundException >(() => dic["iron Man"].Name); //the camelCase filter is in play in the web api
		}

		[Fact]
		public void TestGetEmptyString()
		{
			Assert.Equal("\"\"", api.GetEmptyString());
		}

		[Fact]
		public void TestGetNullString()
		{
			Assert.Equal(String.Empty, api.GetNullString());
		}

		[Fact]
		public void TestGetTextStream()
		{
			var response = api.GetTextStream();
			var stream = response.Content.ReadAsStreamAsync().Result;
			using (var reader = new System.IO.StreamReader(stream))
			{
				var s = reader.ReadToEnd();
				Assert.Equal("abcdefg", s);
			}

		}

		[Fact]
		public void TestZeroWithFloatDoubleAndDecimal()
		{
			// Assert.NotEqual(0f, 0.1f + 0.2f - 0.3f);//In VS 2015 update 2, compiler makes it zeror.
			Assert.NotEqual(0d, 0.1d + 0.2d - 0.3d);
			Assert.Equal(0m, 0.1m + 0.2m - 0.3m);

			Assert.Equal(0, api.GetFloatZero());
			Assert.Equal(0, api.GetDoubleZero());
			Assert.Equal(0, api.GetDecimalZero());

		}

	}
}