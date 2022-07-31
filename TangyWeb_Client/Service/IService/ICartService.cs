using TangyWeb_Client.ViewModels;

namespace TangyWeb_Client.Service.IService;

public interface ICartService
{
	Task IncrementCart(ShoppingCart shoppingCart);
	Task DecrementCart(ShoppingCart shoppingCart);
	public event Action OnChange;
}