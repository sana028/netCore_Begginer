using Microsoft.AspNetCore.Mvc;
using Net_Beginner_web_app.Interfaces;
using Net_Beginner_web_app.Models;
using Net_Beginner_web_app.Repositories;
using System.Text.Json;

namespace Net_Beginner_web_app.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IDataStore _dataStore;
        private readonly ILogger<LoginController> _logger;   

        public LoginController(IHttpClientFactory httpFactory,IDataStore dataStore,ILogger<LoginController>logger)
        {
            _httpClient = httpFactory.CreateClient("ApiClient");
            _dataStore = dataStore;
            _logger = logger;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var response = await _httpClient.PostAsJsonAsync("/UserValidation/login",login);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Authenticating user with login credetials{login}");
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonDocument = JsonDocument.Parse(jsonResponse);
                    var token = jsonDocument.RootElement.GetProperty("token").GetString();
                    _dataStore.StoreAuthenticationToken(token);
                    _dataStore.StoreUserDataInSession(login.Email);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Service: Error retrieving tasks");
            }
            return View();
        }
    }
}
