using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;

namespace MinimalAPI.IntegrationTest
{
    public class Tests : IClassFixture<AppFactory>
    {
        private readonly HttpClient _client;

        public Tests(AppFactory factory)
        {
            _client = factory.CreateClient();
        }

       
        [Fact]
        public async Task Get_AllStudent()
        {
            // Act
            var response = await _client.GetAsync("api/Get_AllStudent?PageNumber=1&PageSize=10");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Save_Student()
        {
            //Arrange
            var jsonString = "{\r\n  \"studentId\": 0,\r\n  \"name\": \"Piyush\",\r\n  \"department\": \"Finance\",\r\n  \"address\": \"Mumbai\",\r\n  \"contactNo\": \"9876\"\r\n}";
            using var jsonContent = new StringContent(jsonString);
            jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            // Act
            using var response = await _client.PostAsync("api/Save_Student", jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}