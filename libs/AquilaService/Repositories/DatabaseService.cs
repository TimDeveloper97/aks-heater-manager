namespace BrothersBlog.Services.Repositories;

using AquilaService.Interfaces;
using AquilaService.Models;
using System.Diagnostics;

public class DatabaseService<T> : IDatabaseService<T> where T : BaseObject, new()
{
    private readonly string _folder;
    private readonly IJsonRepository<T> _jsonRepository;

    /// <summary>
    /// path table database
    /// </summary>
    public virtual string PathTable => AppDomain.CurrentDomain.BaseDirectory + "DB\\" + $"{_folder}\\";

    public DatabaseService(IJsonRepository<T> jsonRepository, string folder)
    {
        this._jsonRepository = jsonRepository ??
            throw new ArgumentNullException(nameof(jsonRepository));
        this._folder = folder ??
            throw new ArgumentNullException(nameof(folder));
    }

    public bool Add(T obj)
    {
        // Gene guid
        if (obj.Id is null)
            obj.Id = Guid.NewGuid().ToString();

        if (Directory.Exists(PathTable + obj.Id))
        {
            // New guid folder
            obj.Id = Guid.NewGuid().ToString();

            Directory.CreateDirectory(PathTable + obj.Id);
        }

        try
        {
            var newFolder = PathTable + obj.Id;

            // Create project common json
            var isOk = _jsonRepository.CreateById(newFolder, obj);
            if (!isOk)
                Debug.WriteLine("Create new object json fail.");

            return isOk;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }
    }

    public T? Get(string id)
    {
        if (!Directory.Exists(PathTable))
            return null;

        try
        {
            var lfolder = Directory.GetDirectories(PathTable);
            var folder = lfolder.FirstOrDefault(x => new DirectoryInfo(x).Name == id);
            if (folder is null) return null;

            var pathFile = folder + "\\" + id + ".json";
            var project = _jsonRepository.Read(pathFile);
            return project;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return null;
    }

    public IEnumerable<T>? GetAll()
    {
        if (!Directory.Exists(PathTable))
            return null;

        try
        {
            var projects = new List<T>();
            var lfolder = Directory.GetDirectories(PathTable);
            foreach (var folder in lfolder)
            {
                var nameFolder = new DirectoryInfo(folder).Name;
                var project = Get(nameFolder);

                if (project is not null)
                    projects.Add(project);
            }

            return projects;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return null;
    }

    public bool Update(T obj)
    {
        if (!Exist(obj.Id))
            return false;

        try
        {
            var pathFolder = PathTable + "\\" + obj.Id + "\\";

            return _jsonRepository.UpdateById(pathFolder, obj);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }
    }

    public bool Exist(string id)
    {
        if (!Directory.Exists(PathTable))
            return false;

        var pathFolder = PathTable + "\\" + id + "\\";
        if (!Directory.Exists(pathFolder))
            return false;

        var pathFile = pathFolder + "\\" + id + ".json";
        if (!File.Exists(pathFile))
            return false;

        return true;
    }

    public bool Delete(string id)
    {
        if (!Exist(id))
            return false;

        var pathFolder = PathTable + "\\" + id + "\\";
        Directory.Delete(pathFolder, true);
        return true;
    }

    public bool DeleteAll()
    {
        Directory.Delete(PathTable, true);
        return true;
    }
}


