namespace SalonCRM.Cache
{
    public interface ICache
    {
        object Get(string key);
        void Add(string key,object obj);
        void Remove(string key);
    }
}