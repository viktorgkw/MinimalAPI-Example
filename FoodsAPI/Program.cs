using System.Net;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using FoodsAPI;
using FoodsAPI.Data;
using FoodsAPI.Models;
using FoodsAPI.Models.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingConfig));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/foods", (ILogger<Program> _logger) =>
{
    _logger.Log(LogLevel.Information, "Getting all foods.");

    APIResponse response = new()
    {
        Result = FoodData.Foods,
        IsSuccess = true,
        StatusCode = HttpStatusCode.OK
    };

    return Results.Ok(response);
})
.WithName("GetFoods")
.Produces<APIResponse>(200);

app.MapGet("/api/foods/{id:int}", (ILogger<Program> _logger, int id) =>
{
    _logger.Log(LogLevel.Information, $"Getting food with id: {id}.");

    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    Food? foundFood = FoodData.Foods.FirstOrDefault(food => food.Id == id);

    if (foundFood is null)
    {
        response.ErrorMessages.Add($"Food with id {id} doesn't exist!");
        return Results.BadRequest(response);
    }

    response.Result = foundFood;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    return Results.Ok(response);
})
.WithName("GetFoodById")
.Produces<APIResponse>(200)
.Produces(400);

app.MapGet("/api/foods/{name}", (ILogger<Program> _logger, string name) =>
{
    _logger.Log(LogLevel.Information, $"Getting food with name: {name}.");

    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    Food? foundFood = FoodData.Foods.FirstOrDefault(food => food.Name == name);

    if (foundFood is null)
    {
        response.ErrorMessages.Add($"Food with name {name} doesn't exist!");
        return Results.BadRequest(response);
    }

    response.Result = foundFood;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    return Results.Ok(response);
})
.WithName("GetFoodByName")
.Produces<APIResponse>(200)
.Produces(400);

app.MapPost("/api/foods", (IMapper _mapper, ILogger<Program> _logger,
    [FromBody] FoodCreateDTO foodDto) =>
{
    _logger.Log(LogLevel.Information, "Creating new food.");

    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    if (string.IsNullOrEmpty(foodDto.Name) || string.IsNullOrWhiteSpace(foodDto.Name))
    {
        response.ErrorMessages.Add("Invalid food name!");
        return Results.BadRequest(response);
    }

    if (FoodData.Foods.Any(f => f.Name.ToLower() == foodDto.Name.ToLower()))
    {
        response.ErrorMessages.Add($"Food with name {foodDto.Name} already exists!");
        return Results.BadRequest(response);
    }

    if (foodDto.Calories < 0 || foodDto.Calories > 600)
    {
        response.ErrorMessages.Add("Calories must be between 0 and 600!");
        return Results.BadRequest(response);
    }

    Food food = _mapper.Map<Food>(foodDto);
    food.Id = FoodData.Foods.Last().Id + 1;
    FoodData.Foods.Add(food);

    response.Result = foodDto;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.Created;

    //return Results.Created($"/api/foods/{food.Id}", food);
    //return Results.CreatedAtRoute("GetFoodById", new { id = food.Id }, food);
    return Results.Ok(response);
})
.WithName("CreateFood")
.Accepts<FoodCreateDTO>("application/json")
.Produces<APIResponse>(201)
.Produces(400);

app.MapPut("/api/foods", (IMapper _mapper, ILogger<Program> _logger,
    [FromBody] FoodUpdateDTO updatedFoodDto) =>
{
    _logger.Log(LogLevel.Information, $"Updating food with name {updatedFoodDto?.Name}.");

    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    if (string.IsNullOrEmpty(updatedFoodDto?.Name) || string.IsNullOrWhiteSpace(updatedFoodDto.Name))
    {
        response.ErrorMessages.Add("Invalid food name!");
        return Results.BadRequest(response);
    }

    if (!FoodData.Foods.Any(f => f.Name.ToLower() == updatedFoodDto.Name.ToLower()))
    {
        response.ErrorMessages.Add($"Food with name {updatedFoodDto.Name} doesn't exist!");
        return Results.BadRequest(response);
    }

    if (updatedFoodDto.Calories < 0 || updatedFoodDto.Calories > 600)
    {
        response.ErrorMessages.Add("Calories must be between 0 and 600!");
        return Results.BadRequest(response);
    }

    Food food = _mapper.Map<Food>(updatedFoodDto);
    Food otherFood = FoodData.Foods
        .First(f => f.Name.ToLower() == updatedFoodDto.Name.ToLower());
    otherFood.Name = updatedFoodDto.Name;
    otherFood.Description = updatedFoodDto.Description;
    otherFood.Calories = updatedFoodDto.Calories;

    response.Result = updatedFoodDto;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    return Results.Ok(response);
})
.WithName("UpdateFood")
.Accepts<FoodUpdateDTO>("application/json")
.Produces<APIResponse>(200)
.Produces(400);

app.MapDelete("/api/foods/{id:int}", (IMapper _mapper, ILogger<Program> _logger, int id) =>
{
    _logger.Log(LogLevel.Information, $"Deleting food with id {id}.");

    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    if (!FoodData.Foods.Any(f => f.Id == id))
    {
        response.ErrorMessages.Add($"Invalid food with id {id}!");
        return Results.BadRequest(response);
    }

    Food foodToDelete = FoodData.Foods.First(f => f.Id == id);

    FoodData.Foods.Remove(foodToDelete);

    response.Result = _mapper.Map<FoodDTO>(foodToDelete);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.NoContent;

    return Results.Ok(response);
})
.WithName("DeleteFood")
.Produces<APIResponse>(200)
.Produces(400);

app.UseHttpsRedirection();

app.Run();

// For the tests
public partial class Program { }