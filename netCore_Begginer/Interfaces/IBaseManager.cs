namespace netCore_Begginer.Interfaces
{
    public interface IBaseManager<T,Type> where T : class
    {
        Task AddTheData(T data);
        Task EditTheData(T data, Type id);
        Task DeleteTheData(Type id);
        Task<T> GetTheData(Type id);

        Task<List<T>> GetAllTheData(string propertyName,Type email);
    }
}
