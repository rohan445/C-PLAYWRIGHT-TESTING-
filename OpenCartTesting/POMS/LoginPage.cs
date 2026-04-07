using Microsoft.Playwright;
using System.Threading.Tasks;

public class LoginPage
{
    private readonly IPage _page;
    private ILocator SearchInput => _page.Locator("input[name='search']");
    private ILocator SearchButton => _page.Locator("button[class*='btn-default']");
    private ILocator NoResultsMessage => _page.Locator("p:has-text('There is no product that matches the search criteria.')");

    public HomePage(IPage page)
    {
        _page = page;
    }

    public async Task Navigate(string url) 
    {
        await _page.GotoAsync(url);
    }

    public async Task SearchProduct(string productName)
    {
        await SearchInput.FillAsync(productName);
        await SearchButton.ClickAsync();
    }

    public async Task<string> GetNoResultsMessage() {
        return await NoResultsMessage.InnerTextAsync();
    }
}