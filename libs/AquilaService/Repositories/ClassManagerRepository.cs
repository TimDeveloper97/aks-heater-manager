using AquilaService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AquilaService.Repositories;

/// <summary>
/// Write namespace of this folder have list patterns to import
/// Here is example: "Generic.Business.Patterns"
/// </summary>
public partial class ClassManagerRepository : IClassManagerRepository
{
    #region Properties
    private readonly IClassRepository _classRepository;
    private readonly IServiceProvider _serviceProvider;
    private IDictionary<string, Type>? _classs;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClassManagerRepository"/> class.
    /// </summary>
    /// <param name="classRepository">classRepository</param>
    /// <exception cref="ArgumentNullException">exception</exception>
    public ClassManagerRepository(IClassRepository classRepository, IServiceProvider serviceProvider)
    {
        _classRepository = classRepository
            ?? throw new ArgumentNullException(nameof(classRepository));
        this._serviceProvider = serviceProvider
            ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    private readonly Dictionary<string, object> _instanceCache = new();
    #endregion
}

/// <summary>
/// class PatternManagerRepositoryV2
/// </summary>
public partial class ClassManagerRepository
{
    public object? GetOrCreateInstance(string id)
    {
        if (_instanceCache.TryGetValue(id, out var instance))
            return instance;

        var (newInstance, _) = CreateInstanceWithId(id);
        if (newInstance is not null)
            _instanceCache[id] = newInstance;

        return newInstance;
    }

    /// <inheritdoc/>
    public string? GetId<T>(Type? type) where T : class, IIdentifier
    {
        if (type is not null)
        {
            var instance = _classRepository.CreateInstance(type, _serviceProvider) as T;
            return instance?.Id;
        }

        return null;
    }

    /// <summary>
    /// Gets name pattern
    /// </summary>
    public IDictionary<string, string>? GetNameMap => _classs?.ToDictionary(x => x.Key, x => x.Value.Name);

    /// <summary>
    /// Create instance with Id
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>type</returns>
    public (object?, Type?) CreateInstanceWithId(string id)
    {
        if (_classs is not null)
        {
            Type? pattern;
            pattern = _classs.FirstOrDefault(x => x.Key == id).Value;

            var instance = _classRepository.CreateInstance(pattern, _serviceProvider);
            return (instance, pattern);
        }

        return (null, null);
    }

    /// <summary>
    /// create instance with name
    /// </summary>
    /// <param name="name">name</param>
    /// <returns>type</returns>
    public (object?, Type?) CreateInstanceWithName(string name)
    {
        if (_classs is not null)
        {
            dynamic pattern = _classs.FirstOrDefault(x => x.Value.Name.Contains(name)).Value;

            var instance = _classRepository.CreateInstance(pattern, _serviceProvider);
            return (instance, pattern);
        }

        return (null, null);
    }

    /// <summary>
    /// insert pattern
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="namespace">namespace</param>
    /// <returns>bool</returns>
    public bool InsertClasss(Type? type, Func<Type, bool> predicate, string? @namespace = null)
    {
        var success = false;

        if (type is not null)
        {
            var pts = _classRepository.GetAll(Assembly.GetAssembly(type), @namespace ?? type.Namespace, true)?
                                    .Where(predicate)
                                    .ToArray();
            if (_classs is null)
                _classs = new Dictionary<string, Type>();

            if (pts is not null)
            {
                foreach (var pt in pts)
                {
                    var id = GetId<IIdentifier>(pt);
                    if (id is not null && !_classs.ContainsKey(id))
                    {
                        _classs.Add(id, pt);
                        success = true;
                    }
                }
            }
        }

        return success;
    }
}

#region Single

/// <summary>
/// class PatternManagerRepositoryV2
/// </summary>
public partial class ClassManagerRepository
{
    /// <summary>
    /// Invoke with Id
    /// </summary>
    /// <param name="methodName">methodName</param>
    /// <param name="id">id</param>
    /// <param name="objects">objects</param>
    /// <returns>object</returns>
    public async Task<object?> InvokeWithIdAsync(string methodName, string id, object[] objects)
    {
        if (_classs is not null)
        {
            Type? pattern;
            pattern = _classs.FirstOrDefault(x => x.Key == id).Value;

            var tcs = await _classRepository.InvokeSync(pattern, methodName, new object[] { objects });
            return tcs;
        }

        return null;
    }

    /// <summary>
    /// Invoke with name
    /// </summary>
    /// <param name="methodName">methodName</param>
    /// <param name="name">name</param>
    /// <param name="objects">objects</param>
    /// <returns>object</returns>
    public async Task<object?> InvokeWithNameAsync(string methodName, string name, object[] objects)
    {
        if (_classs is not null)
        {
            Type? pattern;
            pattern = _classs.FirstOrDefault(x => x.Value.Name.Contains(name)).Value;

            var tcs = await _classRepository.InvokeSync(pattern, methodName, new object[] { objects });
            return tcs;
        }

        return null;
    }

    /// <summary>
    /// invoke with instance
    /// </summary>
    /// <param name="methodName">methodName</param>
    /// <param name="instance">instance</param>
    /// <param name="objects">objects</param>
    /// <returns>object</returns>
    public async Task<object?> InvokeWithInstanceAsync(string methodName, object? instance, object[] objects)
    {
        if (_classs is not null
            && instance is not null)
        {
            var tcs = await _classRepository.InvokeSync(instance, instance.GetType(), methodName, new object[] { objects });
            return tcs;
        }

        return null;
    }

    public async Task<T?> InvokeWithGeneric<T>(string id, string methodName, params object[] parameters)
    {
        var (instance, type) = CreateInstanceWithId(id);
        if (instance is not null && type is not null)
        {
            var result = await _classRepository.InvokeSync(instance, type, methodName, parameters);
            return (T?)result;
        }
        return default;
    }

}

#endregion

#region Multi

/// <summary>
///  partial class PatternManagerRepositoryV2
/// </summary>
public partial class ClassManagerRepository
{
    /// <summary>
    /// Invoke with name
    /// </summary>
    /// <param name="methodName">methodName</param>
    /// <param name="names">names</param>
    /// <param name="matrixObjects">matrixObjects</param>
    /// <returns>Status</returns>
    public async IAsyncEnumerable<(string?, object?)> InvokeWithNamesYieldAsync(string methodName, IEnumerable<string> names, params object[][] matrixObjects)
    {
        if (_classs is not null)
        {
            for (int i = 0; i < names.Count(); i++)
            {
                var element = names.ElementAtOrDefault(i);
                var objects = matrixObjects[i];

                if (element is not null)
                {
                    var pattern = _classs.FirstOrDefault(t => t.Value.Name.ToLower().Contains(element.ToLower())).Value;
                    object? status = null;

                    if (pattern is not null)
                    {
                        status = await _classRepository.InvokeSync(pattern, methodName, new object[] { objects });
                    }

                    yield return (element, status);
                }
            }
        }
        else
        {
            yield return (null, null);
        }
    }

    /// <summary>
    /// Invoke with Ids
    /// </summary>
    /// <param name="methodName">methodName</param>
    /// <param name="ids">ids</param>
    /// <param name="matrixObjects">matrixObjects</param>
    /// <returns>Status</returns>
    public async IAsyncEnumerable<(string?, object?)> InvokeWithIdsYieldAsync(string methodName, IEnumerable<string> ids, params object[][] matrixObjects)
    {
        if (_classs is not null)
        {
            for (int i = 0; i < ids.Count(); i++)
            {
                var element = ids.ElementAtOrDefault(i);
                var objects = matrixObjects[i];

                if (element is not null)
                {
                    var pattern = _classs.FirstOrDefault(t => t.Key.Contains(element)).Value;
                    object? status = null;

                    if (pattern is not null)
                    {
                        status = await _classRepository.InvokeSync(pattern, methodName, new object[] { objects });
                    }

                    yield return (element, status);
                }
            }
        }
        else
        {
            yield return (null, null);
        }
    }
}

#endregion