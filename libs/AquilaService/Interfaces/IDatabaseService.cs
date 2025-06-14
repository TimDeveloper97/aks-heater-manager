using AquilaService.Models;

namespace AquilaService.Interfaces;

public interface IDatabaseService<T> where T : BaseObject, new()
{
    /// <summary>
    /// create api in folder projects\projectid\{folder}\
    /// </summary>
    /// <param name="obj"></param>
    public bool Add(T obj);
    /// <summary>
    /// get api in folder projects\projectid\{folder}\
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public T? Get(string id);
    /// <summary>
    /// get all api in folder projects\projectid\{folder}\
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<T>? GetAll();
    /// <summary>
    /// checked exist api in folder projects\projectid\{folder}\
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Exist(string id);
    /// <summary>
    /// update api in folder projects\projectid\{folder}\
    /// </summary>
    /// <param name="api"></param>
    /// <exception cref="NotImplementedException"></exception>
    public bool Update(T obj);
    /// <summary>
    /// delete api in folder projects\projectid\{folder}\
    /// </summary>
    /// <param name="id"></param>
    public bool Delete(string id);
    /// <summary>
    /// delete all api in folder projects\projectid\
    /// </summary>
    public bool DeleteAll();
}

