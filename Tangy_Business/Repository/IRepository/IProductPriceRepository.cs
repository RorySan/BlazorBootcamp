using Tangy_Models;

namespace Tangy_Business.Repository.IRepository;

public interface IProductPriceRepository
{
	public Task<ProductPriceDTO> Create(ProductPriceDTO objDto);
	public Task<ProductPriceDTO> Update(ProductPriceDTO objDto);
	public Task<int> Delete(int id);
	public Task<ProductPriceDTO> Get(int id);
	public Task<IEnumerable<ProductPriceDTO>> GetAll(int? id = null);
}