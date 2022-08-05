namespace TangyWeb_API.Helper;
/// <summary>
/// This class has the exact names of the configuration we want to access in appsettings.json
/// </summary>
public class APISettings
{
	public string SecretKey { get; set; }
	public string ValidAudience { get; set; }
	public string ValidIssuer { get; set; }
}