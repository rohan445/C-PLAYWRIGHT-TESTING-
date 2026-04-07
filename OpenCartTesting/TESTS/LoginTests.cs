using Microsoft.Playwright;
using System.Threading.Tasks;
using Xunit;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests;

public class LoginTests
{
    private IPage _page;

    public LoginPage(IPage page)
    {
        _page = page;
    }

    private async Task<(IPage page, IBrowser browser)> Setup()
    {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = false,
            SlowMo = 1000
        });

        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        return (page, browser);
    }

    [Fact]
    public async Task Login_With_Valid_Credentials()
    {
        var (page, browser) = await Setup();

        var loginPage = new LoginPage(page);
        await loginPage.Navigate();
        await loginPage.Login("test@test.com", "password123");

        await Expect(page).ToHaveURLAsync("**/account/account");

        await browser.CloseAsync();
    }

    [Fact]
    public async Task Login_With_Invalid_Credentials()
    {
        var (page, browser) = await Setup();

        var loginPage = new LoginPage(page);
        await loginPage.Navigate();
        await loginPage.Login("wrong@test.com", "wrongpass");

        var error = page.GetByText("Warning: No match for E-Mail Address and/or Password.");
        await Expect(error).ToBeVisibleAsync();

        await browser.CloseAsync();
    }

    [Fact]
    public async Task Login_With_Empty_Credentials()
    {
        var (page, browser) = await Setup();

        var loginPage = new LoginPage(page);
        await loginPage.Navigate();
        await loginPage.Login("", "");

        var error = page.GetByText("Warning: No match for E-Mail Address and/or Password.");
        await Expect(error).ToBeVisibleAsync();

        await browser.CloseAsync();
    }

    [Fact]
    public async Task Login_With_Valid_Email_And_Invalid_Password()
    {
        var (page, browser) = await Setup();

        var loginPage = new LoginPage(page);
        await loginPage.Navigate();
        await loginPage.Login("test@test.com", "wrongpassword");

        var error = page.GetByText("Warning: No match for E-Mail Address and/or Password.");
        await Expect(error).ToBeVisibleAsync();

        await browser.CloseAsync();
    }
}