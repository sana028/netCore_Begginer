using Net_Beginner_web_app.Interfaces;

namespace Net_Beginner_web_app.Repositories
{
    public class SessionStoreRepository:ISessionStore
    {
        private readonly IHttpContextAccessor ContextAccessor;

        public SessionStoreRepository(IHttpContextAccessor contextAccessor)
        {
            ContextAccessor = contextAccessor;
        }
        public void StoreAuthenticationToken(string token)
        {
            ContextAccessor.HttpContext?.Session.SetString("authToken", token);
        }

        public void StoreUserDataInSession(string email)
        {
            ContextAccessor.HttpContext?.Session.SetString("email", email);
        }

        public string GetTheUserDataFromSession()
        {
            return ContextAccessor?.HttpContext?.Session?.GetString("email");
        }
    }
}
