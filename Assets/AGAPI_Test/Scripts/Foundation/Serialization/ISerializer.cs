
namespace AGAPI.Foundation
{
    public interface ISerializer<TResult>
    {
        TResult Serialize<T>(T obj);

        T Deserialize<T>(TResult data);
    }
}