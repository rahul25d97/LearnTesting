using MinimalAPI.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;


namespace MinimalAPI.E2ETesting
{
    public class E2ETests : IClassFixture<AppFactory>
    {
        private readonly HttpClient _client;

        public E2ETests(AppFactory factory)
        {
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task E2E_StudentManagerTest()
        {
            #region Save Student
            //Arrange
            var jsonString = "{\r\n  \"studentId\": 0,\r\n  \"name\": \"Piyush\",\r\n  \"department\": \"Finance\",\r\n  \"address\": \"Mumbai\",\r\n  \"contactNo\": \"9876\"\r\n}";
            using var jsonContent = new StringContent(jsonString);
            jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            // Act
            using var saveResponse = await _client.PostAsync("api/Save_Student", jsonContent);

            // Assert
            var saveResponseBody = await saveResponse.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<PostResult>(saveResponseBody);

            saveResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, saveResponse.StatusCode);

            #endregion

            #region Get Student By Id

            // Act
            var getResponse = await _client.GetAsync("api/Get_StudentById?Id=" + obj.Id);

            // Assert
            getResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            #endregion

            #region Remove Student
            // Act
            using var removeResponse = await _client.DeleteAsync("api/Remove_Student/" + obj.Id);

            // Assert
            removeResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, removeResponse.StatusCode);

            #endregion
        }
    }
}