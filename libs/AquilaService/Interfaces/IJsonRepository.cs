namespace AquilaService.Interfaces;

using System;
using System.Collections.Generic;

public interface IJsonRepository<T>
{
    /// <summary>
    /// read json file
    /// </summary>
    /// <param name="pathFile"></param>
    /// <returns></returns>
    public T? Read(string pathFile);
    /// <summary>
    /// read list object with save format json
    /// </summary>
    /// <param name="pathFolder"></param>
    /// <returns></returns>
    public IEnumerable<T>? ReadAll(string pathFolder);
    /// <summary>
    /// create file with object
    /// </summary>
    /// <param name="pathFolder"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool CreateById(string pathFolder, T obj);
    /// <summary>
    /// update json file with specification name in path
    /// </summary>
    /// <param name="pathFile"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool CreateByName(string pathFile, T obj);
    /// <summary>
    /// create all file with object
    /// </summary>
    /// <param name="pathFolder"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public bool CreateAll(string pathFolder, IEnumerable<T> objs);
    /// <summary>
    /// replace .content file with object
    /// </summary>
    /// <param name="pathFolder"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool UpdateById(string pathFolder, T obj);
    /// <summary>
    /// create json file with specification name in path
    /// </summary>
    /// <param name="pathFile"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool UpdateByName(string pathFile, T obj);
    /// <summary>
    /// delete json file
    /// </summary>
    /// <param name="pathFolder"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool Delete(string pathFolder, string id);
}
