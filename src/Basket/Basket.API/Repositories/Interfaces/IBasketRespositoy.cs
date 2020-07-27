using Basket.API.Entities;
using System.Threading.Tasks;

namespace Basket.API.Repositories.Interfaces
{
    public interface IBasketRespositoy
    {
        Task<BasketCart> GetBasket(string userName);
        Task<BasketCart> UpdateBasket(BasketCart basket);
        Task<bool> DeleteBasket(string userName);
    }
}
