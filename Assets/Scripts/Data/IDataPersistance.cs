public interface IDataPersistance<T>
{
    void LoadData(T data);
    void SaveData(T data);
}