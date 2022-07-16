namespace Tangy_Models.LearnBlazorModels;

public class DemoProduct
{
	public int Id { get; set; }
	public string Name { get; set; }
	public double Price { get; set; }
	public bool IsActive { get; set; }
	public IEnumerable<DemoProductProperties> ProductProperties { get; set; }
}