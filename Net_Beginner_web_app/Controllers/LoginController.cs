using Microsoft.AspNetCore.Mvc;
using Net_Beginner_web_app.Interfaces;
using Net_Beginner_web_app.Models;
using Net_Beginner_web_app.Repositories;
using System.Text.Json;

namespace Net_Beginner_web_app.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient HttpClient;
        private readonly ISessionStore DataStore;
        private readonly ILogger<LoginController> Logger;   

        public LoginController(IHttpClientFactory httpFactory,ISessionStore dataStore,ILogger<LoginController>logger)
        {
            HttpClient = httpFactory.CreateClient("ApiClient");
            DataStore = dataStore;
            Logger = logger;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var response = await HttpClient.PostAsJsonAsync("/UserValidation/login",login);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    Logger.LogInformation($"Authenticating user with login credetials{login}");
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonDocument = JsonDocument.Parse(jsonResponse);
                    var token = jsonDocument.RootElement.GetProperty("token").GetString();
                    DataStore.StoreAuthenticationToken(token);
                    DataStore.StoreUserDataInSession(login.Email);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex,"Service: Error retrieving tasks");
            }
            return View();
        }
    }
}
