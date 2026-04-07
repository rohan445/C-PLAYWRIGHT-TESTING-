using Microsoft.Playwright;
using System.Threading.Tasks;

public class ProductPage
{
    private readonly IPage _page;
    private ILocator FirstProduct => _page.Locator(".product-layout .product-thumb").First;
    private ILocator AddToCartButton => _page.Locator("#button-cart");

    public ProductPage(IPage page)
    {
        _page = page;
    }
    public async Task SelectFirstProduct() => await FirstProduct.ClickAsync();
    public async Task AddToCart() => await AddToCartButton.ClickAsync();
}