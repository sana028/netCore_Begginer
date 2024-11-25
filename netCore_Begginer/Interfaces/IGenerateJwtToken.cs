namespace netCore_Begginer.Interfaces
{
    public interface IGenerateJwtToken
    {
        string GenerateToken(string role, string email);
    }
}
