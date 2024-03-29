﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
	
	public async Task<CategoryDTO> Create(CategoryDTO objDto)
	{
		var obj = _mapper.Map<CategoryDTO, Category>(objDto);
		obj.CreatedDate = DateTime.Now;
		var addedObj = _db.Categories.Add(obj);
		await _db.SaveChangesAsync();

		return _mapper.Map<Category, CategoryDTO>(addedObj.Entity);
	}

	public async Task<CategoryDTO> Update(CategoryDTO objDto)
	{
		var objFromDb = await _db.Categories.FirstOrDefaultAsync(u => u.Id == objDto.Id);
		if (objFromDb == null) return objDto;
		objFromDb.Name = objDto.Name;
		_db.Categories.Update(objFromDb);
		await _db.SaveChangesAsync();
		return _mapper.Map<Category, CategoryDTO>(objFromDb);

	}

	public async Task<int> Delete(int id)
	{
		var obj = await _db.Categories.FirstOrDefaultAsync(category => category.Id == id);
		if (obj == null) return 0;
		_db.Categories.Remove(obj);
		return await _db.SaveChangesAsync();

	}

	public async Task<CategoryDTO> Get(int id)
	{
		var obj = await _db.Categories.FirstOrDefaultAsync(category => category.Id == id);
		return obj != null ? _mapper.Map<Category, CategoryDTO>(obj) : new CategoryDTO();
	}

	public async Task<IEnumerable<CategoryDTO>> GetAll()
	{
		return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(_db.Categories);
	}
}