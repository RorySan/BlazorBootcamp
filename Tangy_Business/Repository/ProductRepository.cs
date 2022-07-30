using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository;

public class ProductRepository : IProductRepository
{
	private readonly ApplicationDbContext _db;
	private readonly IMapper _mapper;

	public ProductRepository(ApplicationDbContext db, IMapper mapper)
	{
		_db = db;
		_mapper = mapper;
	}

	public async Task<ProductDTO> Create(ProductDTO objDto)
	{
		var obj = _mapper.Map<ProductDTO, Product>(objDto);
		var addedObj = _db.Products.Add(obj);
		await _db.SaveChangesAsync();
		return _mapper.Map<Product, ProductDTO>(addedObj.Entity);
	}

	public async Task<ProductDTO> Update(ProductDTO objDto)
	{
		var objFromDb = await _db.Products.FirstOrDefaultAsync(u => u.Id == objDto.Id);
		if (objFromDb == null) return objDto;
		objFromDb.Name = objDto.Name;
		objFromDb.Description = objDto.Description;
		objFromDb.ImageUrl = objDto.ImageUrl;
		objFromDb.CategoryId = objDto.CategoryId;
		objFromDb.Color = objDto.Color;
		objFromDb.CustomerFavorite = objDto.CustomerFavorite;
		objFromDb.ShopFavorite = objFromDb.ShopFavorite;
		_db.Products.Update(objFromDb);
		await _db.SaveChangesAsync();
		return _mapper.Map<Product, ProductDTO>(objFromDb);
	}

	public async Task<int> Delete(int id)
	{
		var obj = await _db.Products.FirstOrDefaultAsync(product => product.Id == id);
		if (obj == null) return 0;
		_db.Products.Remove(obj);
		return await _db.SaveChangesAsync();
	}

	public async Task<ProductDTO> Get(int id)
	{
		var obj = await _db.Products.Include(product => product.Category)
			.FirstOrDefaultAsync(product => product.Id == id);
		return obj != null ? _mapper.Map<Product, ProductDTO>(obj) : new ProductDTO();
	}

	public async Task<IEnumerable<ProductDTO>> GetAll()
	{
		return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(_db.Products
			.Include(product => product.Category));

	}
}