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
        

        public LoginController(IHttpClientFactory httpFactory,IDataStore dataStore)
        {
            _httpClient = httpFactory.CreateClient("ApiClient");
            _dataStore = dataStore;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var response = await _httpClient.PostAsJsonAsync("/UserValidation/login",login);
            if(response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(jsonResponse);
                var token = jsonDocument.RootElement.GetProperty("token").GetString();
                _dataStore.StoreAuthenticationToken(token);
                _dataStore.StoreUserDataInSession(login.Email);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
