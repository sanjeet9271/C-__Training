namespace SphoneApp.Interfaces;

public interface IRepository<T> where T : class
{
    List<T> GetAll();
    void Add(T entity);
    void SaveChanges();
    void LoadData();
}

