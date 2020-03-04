using Fonlow.OpenApi.ClientTypes;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using System.IO;
using Xunit;
using Fonlow.OpenApiClientGen.Cs;

namespace SwagTests
{
    public class ToCsFunctions
    {
        static OpenApiDocument ReadJson(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return new OpenApiStreamReader().Read(stream, out var diagnostic);
            }
        }

        static string TranslateJsonToCode(string filePath)
        {
            OpenApiDocument doc = ReadJson(filePath);

            Settings settings = new Settings()
            {
                ClientNamespace = "MyNS",
                PathPrefixToRemove="/api",
                ForBothAsyncAndSync=true
            };
            var gen = new ControllersClientApiGen(settings);
            gen.CreateCodeDom(doc.Paths, doc.Components);
            return gen.WriteToText();
        }

        static string ReadFromResults(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        [Fact]
        public void TestValuesPaths()
        {
            var s = TranslateJsonToCode("SwagMock\\ValuesPaths.json");
            Assert.Equal(ReadFromResults("Results\\ValuesPaths.txt"), s);
        }

        [Fact]
        public void TestSimplePet()
        {
            string expected = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyNS
{
	
	
	public class Pet
	{
		
		/// <summary>
		/// The name given to a pet
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// Type of a pet
		/// </summary>
		public string PetType { get; set; }
	}
}
";
            var s = TranslateJsonToCode("SwagMock\\SimplePet.json");
            Assert.Equal(expected, s);
        }



    }
}