namespace FoodsAPI.Tests
{
    using System.Net;
    using System.Net.Http.Json;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Testing;
    
    using FoodsAPI.Models.DTOs;
    using FoodsAPI.Models;

    public class APITests
    {

        [Fact]
        public async Task GetFoodsShouldBeSuccessful()
        {
            // Arrange
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            // Act
            var response = await client.GetFromJsonAsync<APIResponse>($"/api/foods");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task GetFoodByNameShouldBeSuccessful()
        {
            // Arrange
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();
            string searchName = "Apple";

            // Act
            var response = await client.GetFromJsonAsync<APIResponse>($"/api/foods/{searchName}");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
        }

        [Fact]
        public async Task PostFoodShouldBeSuccessful()
        {
            // Arrange
            await using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            // Act
            var result = await client.PostAsJsonAsync("/api/foods", new FoodCreateDTO
            {
                Name = "Test",
                Description = "Testing.",
                Calories = 11.1,
            });

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task PutFoodShouldBeSuccessful()
        {
            await using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var result = await client.PutAsJsonAsync("/api/foods", new FoodUpdateDTO
            {
                Name = "Apple",
                Description = "Apple falls from the apple tree.",
                Calories = 36.36,
            });

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("http://localhost/api/foods", result.RequestMessage!.RequestUri!.ToString());
        }

        [Fact]
        public async Task PutInvalidFoodShouldNotBeSuccessful()
        {
            await using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var result = await client.PutAsJsonAsync("/api/foods", new FoodUpdateDTO
            {
                Name = "Vodka",
                Description = "Invalid drink!",
                Calories = 800.008,
            });

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task DeleteFoodShouldBeSuccessful()
        {
            // Arrange
            await using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            int idToDelete = 1;

            // Act
            var result = await client.DeleteFromJsonAsync<APIResponse>($"/api/foods/{idToDelete}");

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ErrorMessages.Count == 0);
            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.Contains("\"name\":\"Bread\"", result.Result!.ToString());
        }
    }
}