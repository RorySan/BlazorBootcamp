using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository;

public class ProductPriceRepository: IProductPriceRepository
{
	private readonly ApplicationDbContext _db;
	private readonly IMapper _mapper;
	
	public ProductPriceRepository(ApplicationDbContext db, IMapper mapper)
	{
		_db = db;
		_mapper = mapper;
	}
	
	public async Task<ProductPriceDTO> Create(ProductPriceDTO objDto)
	{
		var obj = _mapper.Map<ProductPriceDTO, ProductPrice>(objDto);
		
		var addedObj = _db.ProductPrices.Add(obj);
		await _db.SaveChangesAsync();

		return _mapper.Map<ProductPrice, ProductPriceDTO>(addedObj.Entity);
	}

	public async Task<ProductPriceDTO> Update(ProductPriceDTO objDTO)
	{
		var objFromDb = await _db.ProductPrices.FirstOrDefaultAsync(u => u.Id==objDTO.Id);
		if (objFromDb!=null)
		{
			objFromDb.Price = objDTO.Price;
			objFromDb.Size = objDTO.Size;
			objFromDb.ProductId= objDTO.ProductId;
			_db.ProductPrices.Update(objFromDb);
			await _db.SaveChangesAsync();
			return _mapper.Map<ProductPrice, ProductPriceDTO>(objFromDb);
		}
		return objDTO;
	}

	public async Task<int> Delete(int id)
	{
		var objFromDb = await _db.ProductPrices.FirstOrDefaultAsync(prodPrice => prodPrice.Id == id);
		if (objFromDb == null) return 0;
		_db.ProductPrices.Remove(objFromDb);
		return await _db.SaveChangesAsync();

	}

	public async Task<ProductPriceDTO> Get(int id)
	{
		var objFromDb = await _db.ProductPrices.FirstOrDefaultAsync(prodPrice => prodPrice.Id == id);
		return objFromDb != null ? _mapper.Map<ProductPrice, ProductPriceDTO>(objFromDb) : new ProductPriceDTO();
	}

	public async Task<IEnumerable<ProductPriceDTO>> GetAll(int? id = null)
	{
		if (id is > 0)
		{
			return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>
				(_db.ProductPrices.Where(u => u.ProductId == id));
		}
		else
		{
			return _mapper.Map<IEnumerable<ProductPrice>, IEnumerable<ProductPriceDTO>>(_db.ProductPrices);
		}
	}
}