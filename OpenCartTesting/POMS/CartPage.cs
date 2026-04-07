using Microsoft.Playwright;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CartPage
{
    private readonly IPage _page;
    private ILocator CartButton => _page.Locator("#cart-total");
    private ILocator CartItems => _page.Locator(".dropdown-menu .text-left a");
    private ILocator RemoveButtons => _page.Locator(".dropdown-menu button[title='Remove']");

    public CartPage(IPage page) 
    {
        _page = page;
    }
    public async Task<List<string>> GetAllProductNames() 
    // Get the names of all products in the cart
    {
        var names = new List<string>();
        int count = await CartItems.CountAsync();
        // Loop through each cart item and get its name 
        for (int i = 0; i < count; i++)
        {
            names.Add(await CartItems.Nth(i).InnerTextAsync());
        }
        return names;
    }

    public async Task RemoveFirstProduct()
    {
        await RemoveButtons.First.ClickAsync();
        await _page.WaitForTimeoutAsync(1000); // wait for removal
    }
}