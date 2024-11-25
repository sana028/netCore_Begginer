using Net_Beginner_web_app.Interfaces;

namespace Net_Beginner_web_app.Repositories
{
    public class DataStore:IDataStore
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public DataStore(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void StoreAuthenticationToken(string token)
        {
            _contextAccessor.HttpContext?.Session.SetString("authToken", token);
        }

        public void StoreUserDataInSession(string email)
        {
            _contextAccessor.HttpContext?.Session.SetString("email", email);
        }

        public string GetTheUserDataFromSession()
        {
            return _contextAccessor?.HttpContext?.Session?.GetString("email");
        }
    }
}
