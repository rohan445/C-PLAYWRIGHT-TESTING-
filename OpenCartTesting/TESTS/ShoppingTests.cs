using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;

namespace PlaywrightTests;
public class ShoppingTests
{
    [Fact]
    public async Task Search_Add_To_Cart_Flow()
    {
        var fixture = new PlaywrightFixture();
        await fixture.Setup();

        var home = new HomePage(fixture.Page);
        var product = new ProductPage(fixture.Page);
        var cart = new CartPage(fixture.Page);

        await home.Navigate("https://demo.opencart.com/");
        await home.SearchProduct("iPhone");
        await product.SelectFirstProduct();
        await product.AddToCart();
        await cart.OpenCart();

        // Verify that the product is in the cart
        var productName = await cart.GetFirstProductName();
        Assert.Contains("iPhone", productName);

        await fixture.TearDown();
    }

    [Fact]
    public async Task Search_NonExistentProduct_ShowsNoResults()
    {
        var fixture = new PlaywrightFixture();
        await fixture.Setup();

        var home = new HomePage(fixture.Page);
        await home.Navigate("https://demo.opencart.com/");
        // Search for a product that does not exist
        await home.SearchProduct("NonExistentProduct123");

        // Verify that no products are found
        var searchResults = await home.GetSearchResults();
        Assert.Empty(searchResults);

       // Verify that the no results message is displayed
        var message = await home.GetNoResultsMessage();
        Assert.Contains("There is no product that matches the search criteria.", message);

        await fixture.TearDown();
    }

    [Fact]
    public async Task Add_Multiple_Products_To_Cart()
    {
        var fixture = new PlaywrightFixture();
        await fixture.Setup();

        var home = new HomePage(fixture.Page);
        var product = new ProductPage(fixture.Page);
        var cart = new CartPage(fixture.Page);

        await home.Navigate("https://demo.opencart.com/");

        await home.SearchProduct("iPhone");
        await product.SelectFirstProduct();
        await product.AddToCart();

        await home.SearchProduct("MacBook");
        await product.SelectFirstProduct();
        await product.AddToCart();

        // Open the cart to verify that both products are added
        await cart.OpenCart();
        // Verify that both products are in the cart
        var products = await cart.GetAllProductNames();
        Assert.Contains("iPhone", products);
        Assert.Contains("MacBook", products);
        // Verify that the product count is correct
        var productCount = await cart.GetProductCount();
        Assert.Equal(2, await cart.GetProductCount());

        await fixture.TearDown();
    }

    [Fact]
    public async Task Remove_Product_From_Cart()
    {
        var fixture = new PlaywrightFixture();
        await fixture.Setup();

        var home = new HomePage(fixture.Page);
        var product = new ProductPage(fixture.Page);
        var cart = new CartPage(fixture.Page);

        await home.Navigate("https://demo.opencart.com/");
        await home.SearchProduct("iPhone");
        // verify that the product is found before trying to add it to the cart
        var searchResults = await home.GetSearchResults();
        Assert.Contains("iPhone", searchResults);
        await product.SelectFirstProduct();
        await product.AddToCart();

        await cart.OpenCart();
        await cart.RemoveFirstProduct();
        var products = await cart.GetAllProductNames();
        // Verify that the product has been removed from the cart
        Assert.DoesNotContain("iPhone", products);

        await fixture.TearDown();
    }

    [Fact]
    public async Task Checkout_BasicFlow()
    {
        var fixture = new PlaywrightFixture();
        await fixture.Setup();

        var home = new HomePage(fixture.Page);
        var product = new ProductPage(fixture.Page);
        var cart = new CartPage(fixture.Page);
        var checkout = new CheckoutPage(fixture.Page);

        await home.Navigate("https://demo.opencart.com/");
        await home.SearchProduct("iPhone");
        await product.SelectFirstProduct();
        await product.AddToCart();

        await cart.OpenCart();
        await checkout.ProceedToCheckout();

        var pageTitle = await fixture.Page.TitleAsync();
        // Verify that we are on the checkout page
        Assert.Contains("Checkout", pageTitle);

        await fixture.TearDown();
    }
}