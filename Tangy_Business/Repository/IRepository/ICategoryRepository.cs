using System.Dynamic;
using Tangy_Models;

namespace Tangy_Business.Repository.IRepository;

public interface ICategoryRepository
{
	public CategoryDTO Create(CategoryDTO objDto);
	public CategoryDTO Update(CategoryDTO objDto);
	public int Delete(int id);

	public CategoryDTO Get(int id);
	public IEnumerable<CategoryDTO> GetAll();
}