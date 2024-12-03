namespace Net_Beginner_web_app.Interfaces
{
    public interface ISessionStore
    {
        void StoreUserDataInSession(string email);
        void StoreAuthenticationToken(string token);
        string GetTheUserDataFromSession();

    }
}
