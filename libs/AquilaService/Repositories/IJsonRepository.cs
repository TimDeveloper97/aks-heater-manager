namespace AquilaService.Repositories;

using AquilaService.Interfaces;
using AquilaService.Models;
using Newtonsoft.Json;
using System.Diagnostics;

// CRUD file json
public class JsonRepository<T> : IJsonRepository<T> where T : BaseObject
{
    public T? Read(string pathFile)
    {
        T? file = null;
        try
        {
            if (!File.Exists(pathFile)) return null;

            string contents = File.ReadAllText(pathFile);
            file = JsonConvert.DeserializeObject<T>(contents);
        }
        catch (System.Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        return file;
    }

    public IEnumerable<T>? ReadAll(string pathFolder)
    {
        List<T>? objs = new List<T>();

        // Checked and create folder
        if (!Directory.Exists(pathFolder)) Directory.CreateDirectory(pathFolder);

        // Checked file exist
        var listFilePath = Directory.EnumerateFiles(pathFolder, "*.json");
        if (listFilePath is null || listFilePath.Count() == 0)
            return null;

        foreach (string filePath in listFilePath)
        {
            try
            {
                string contents = File.ReadAllText(filePath);
                var jsons = JsonConvert.DeserializeObject<T>(contents);
                if (jsons != null)
                    objs.Add(jsons);

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                objs = null;
            }
        }
        return objs;
    }

    public bool CreateById(string pathFolder, T obj)
    {
        try
        {
            string json = JsonConvert.SerializeObject(obj);

            if (!Directory.Exists(pathFolder))
                Directory.CreateDirectory(pathFolder);

            var pathFile = pathFolder + "/" + obj.Id + ".json";
            if (File.Exists(pathFile))
                File.Delete(pathFile);

            File.WriteAllText(pathFolder + "/" + obj.Id + ".json", json);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return false;
    }

    public bool CreateAll(string pathFolder, IEnumerable<T> objs)
    {
        foreach (var obj in objs)
        {
            var isOk = CreateById(pathFolder, obj);
            if (!isOk)
                return false;
        }
        return true;
    }

    public bool UpdateById(string pathFolder, T obj)
    {
        try
        {
            var pathFile = pathFolder + "/" + obj.Id + ".json";
            var o = Read(pathFile);
            if (o is not null)
            {
                string json = JsonConvert.SerializeObject(obj);
                File.WriteAllText(pathFile, json);
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return false;
    }

    public bool Delete(string pathFolder, string id)
    {
        try
        {
            var pathFile = pathFolder + "/" + id + ".json";
            if (File.Exists(pathFile))
            {
                File.Delete(pathFile);
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return false;
    }

    public bool CreateByName(string pathFile, T obj)
    {
        try
        {
            string json = JsonConvert.SerializeObject(obj);

            if (File.Exists(pathFile))
                File.Delete(pathFile);

            File.WriteAllText(pathFile, json);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return false;
    }

    public bool UpdateByName(string pathFile, T obj)
    {
        try
        {
            var o = Read(pathFile);
            if (o is not null)
            {
                string json = JsonConvert.SerializeObject(obj);
                File.WriteAllText(pathFile, json);
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return false;
    }
}

