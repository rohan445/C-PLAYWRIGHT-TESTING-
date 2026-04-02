using System;
using System.Threading.Tasks;
using Microsoft.Playwright; 
class TestCaseProgram
{
    public static async Task Testcase()
    {
        using var playwright = await Playwright.CreateAsync();

        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false 
        });

        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        // Navigate to site
        await page.GotoAsync("https://www.saucedemo.com/");

        // Enter creditionals to Login
        await page.FillAsync("#user-name", "standard_user");
        await page.FillAsync("#password", "secret_sauce");
        await page.ClickAsync("#login-button");

        Console.WriteLine("Logged in successfully");
        // Assetion check if login successful by checking URL
        string currentUrl = page.Url;
        if (currentUrl != "https://www.saucedemo.com/inventory.html")
            throw new Exception("Login failed");

        //Assertion if login successful by checking inventory page is loaded
        await page.GotoAsync("https://www.saucedemo.com/inventory.html"); // Navigate to inventory page
        await page.WaitForSelectorAsync(".inventory_item"); // Wait for inventory items to load
        // Verify that inventory items are displayed
        var items = await page.QuerySelectorAllAsync(".inventory_item");
        if (items.Count == 0)
            throw new Exception("No inventory items found");

        Console.WriteLine("Inventory items displayed");


        // Add product to cart
        await page.ClickAsync("text=Add to cart"); 
        string cartCount = await page.InnerTextAsync(".shopping_cart_badge");

        if (cartCount != "1") 
            throw new Exception("Item not added to cart");

        Console.WriteLine("Item added to cart");

        //Go to cart to verify item is added
        await page.GotoAsync("");
        await page.ClickAsync(".shopping_cart_link");
        // Assertation if cart page is loaded
        string cartUrl = page.Url;
        if (cartUrl != "https://www.saucedemo.com/cart.html")
            throw new Exception("Cart page not loaded");

        await page.WaitForSelectorAsync(".cart_item");

        Console.WriteLine("Cart verified");

        // Checkout
        await page.ClickAsync("#checkout");

        // Fill details
        await page.FillAsync("#first-name", "John");
        await page.FillAsync("#last-name", "Doe");
        await page.FillAsync("#postal-code", "110001");

        await page.ClickAsync("#continue");

        // Assertion postal code should be of 5 digits
        await page.FillAsync("#postal-code", "1100"); // Invalid postal code
        if (await page.IsVisibleAsync(".error-message-container"))
            Console.WriteLine("Error message displayed for invalid postal code");
        else
            throw new Exception("Error message not displayed for invalid postal code");
        await page.ClickAsync("#continue");
 
       // Go to set step2 
        await page.GotoAsync("https://www.saucedemo.com/checkout-step-two.html");

        // Assertion 3 
        await page.WaitForSelectorAsync(".summary_info");

        Console.WriteLine("Checkout info entered");

        //Assertion if checkout info is correct
        string checkoutInfo = await page.InnerTextAsync(".summary_info");
        if (!checkoutInfo.Contains("John") || !checkoutInfo.Contains("Doe") || !checkoutInfo.Contains("110001"))
            throw new Exception("Checkout information is incorrect");

        // Finish order
        await page.GotoAsync("https://www.saucedemo.com/checkout-complete.html");
        await page.ClickAsync("#finish");

        // Assertion if order is completed by checking URL
        string orderUrl = page.Url;
        if (orderUrl != "https://www.saucedemo.com/checkout-complete.html")
            throw new Exception("Order not completed");
        
        // Assertion order completion
        string confirmation = await page.InnerTextAsync(".complete-header");
        if (!confirmation.Contains("Your order has been dispatched"))
            throw new Exception("Order not dispatched");

        if (!confirmation.Contains("Thank you"))
            throw new Exception("Order not completed");

        Console.WriteLine("Order completed successfully");

        await browser.CloseAsync();
    }
}

