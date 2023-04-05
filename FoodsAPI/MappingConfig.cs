namespace FoodsAPI
{
    using AutoMapper;

    using FoodsAPI.Models;
    using FoodsAPI.Models.DTOs;

    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            this.CreateMap<FoodCreateDTO, Food>()
                .ReverseMap();

            this.CreateMap<FoodUpdateDTO, Food>()
                .ReverseMap();

            this.CreateMap<FoodDTO, Food>()
                .ReverseMap();
        }
    }
}
