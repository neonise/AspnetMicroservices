using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.Events;

namespace Basket.API.Mapper
{
    public class BaseketProfile : Profile
    {
        public BaseketProfile()
        {
            CreateMap<BasketCheckout, BasketCheckouEvent>().ReverseMap();
        }
    }
}
