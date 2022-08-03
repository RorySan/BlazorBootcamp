namespace Tangy_Models;

public class SignUpResponseDTO
{
	public bool IsRegistrationSuccessful { get; set; }
	public IEnumerable<string> Errors { get; set; }
}