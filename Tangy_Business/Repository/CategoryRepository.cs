using AutoMapper;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_Models;
using Tangy_DataAccess.Data;

namespace Tangy_Business.Repository;

public class CategoryRepository : ICategoryRepository
{
	private readonly ApplicationDbContext _db;
	private readonly IMapper _mapper;
	
	public CategoryRepository(ApplicationDbContext db, IMapper mapper)
	{
		_db = db;
		_mapper = mapper;
	}
	
	public CategoryDTO Create(CategoryDTO objDto)
	{
		var obj = _mapper.Map<CategoryDTO, Category>(objDto);
		obj.CreatedDate = DateTime.Now;
		var addedObj = _db.Categories.Add(obj);
		_db.SaveChanges();

		return _mapper.Map<Category, CategoryDTO>(addedObj.Entity);
	}

	public CategoryDTO Update(CategoryDTO objDto)
	{
		var objFromDb = _db.Categories.FirstOrDefault(u => u.Id == objDto.Id);
		if (objFromDb == null) return objDto;
		objFromDb.Name = objDto.Name;
		_db.Categories.Update(objFromDb);
		_db.SaveChanges();
		return _mapper.Map<Category, CategoryDTO>(objFromDb);

	}

	public int Delete(int id)
	{
		var obj = _db.Categories.FirstOrDefault(category => category.Id == id);
		if (obj == null) return 0;
		_db.Categories.Remove(obj);
		return _db.SaveChanges();

	}

	public CategoryDTO Get(int id)
	{
		var obj = _db.Categories.FirstOrDefault(category => category.Id == id);
		return obj == null ? new CategoryDTO() : _mapper.Map<Category, CategoryDTO>(obj);
	}

	public IEnumerable<CategoryDTO> GetAll()
	{
		return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(_db.Categories);
	}
}