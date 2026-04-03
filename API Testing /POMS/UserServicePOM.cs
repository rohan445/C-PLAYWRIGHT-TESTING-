using Microsoft.Playwright;
using System.Threading.Tasks;

public class UserService
{
    private readonly IAPIRequestContext request;

    public UserService(IAPIRequestContext request) 
    {
        this.request = request;
    }
    // Create a new user Post request
    public async Task<IAPIResponse> CreateUser(UserRequest user)
    {
        return await request.PostAsync(
            "https://reqres.in/api/users",
            new APIRequestContextOptions
            {
                DataObject = user
            });
    }
    // get a response of all users
    public async Task<IAPIResponse> GetUsers()
    {
        return await request.GetAsync("https://reqres.in/api/users?page=2");
    }
}