using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquilaService.Interfaces;

#region Core

/// <summary>
/// partial interface IClassManagerRepository
/// </summary>
public partial interface IClassManagerRepository
{
    /// <summary>
    /// get or create instantce with id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public object? GetOrCreateInstance(string id);

    /// <summary>
    /// Gets list name pattern in storage
    /// </summary>
    public IDictionary<string, string>? GetNameMap { get; }

    /// <summary>
    /// Insert pattern to list pattern
    /// </summary>
    /// <param name="type">typeof([class]).Namespace)</param>
    /// <param name="predicate">predicate condition</param>
    /// <param name="namespace">"Logic.Interfaces"</param>
    /// <returns>bool</returns>
    public bool InsertClasss(Type? type, Func<Type, bool> predicate, string? @namespace = null);

    /// <summary>
    /// Create instance with name and insert to list pattern storage
    /// </summary>
    /// <param name="name">name Pattern</param>
    /// <returns>type</returns>
    public (object?, Type?) CreateInstanceWithName(string name);

    /// <summary>
    /// Create instance with id
    /// </summary>
    /// <param name="id">id Pattern</param>
    /// <returns>type</returns>
    public (object?, Type?) CreateInstanceWithId(string id);

    /// <summary>
    /// Get id class
    /// </summary>
    /// <param name="type">type</param>
    /// <returns>string</returns>
    public string? GetId<T>(Type? type) where T : class, IIdentifier;
}
#endregion

#region Single

/// <summary>
/// partial interface IClassManagerRepository
/// </summary>
public partial interface IClassManagerRepository
{
    /// <summary>
    /// Invoke method with id execute and pattern Id
    /// </summary>
    /// <param name="methodName">Generate</param>
    /// <param name="id">pattern2</param>
    /// <param name="objects">"Diagram", ""</param>
    /// <returns>objects</returns>
    public Task<object?> InvokeWithIdAsync(string methodName, string id, object[] objects);

    /// <summary>
    /// Invoke method with name execute and pattern Id
    /// </summary>
    /// <param name="methodName">Generate</param>
    /// <param name="name">InterruptionEventCoveragePattern</param>
    /// <param name="objects">"Diagram", ""</param>
    /// <returns>object</returns>
    public Task<object?> InvokeWithNameAsync(string methodName, string name, object[] objects);

    /// <summary>
    /// Invoke method with instance class execute and pattern Id
    /// </summary>
    /// <param name="methodName">Generate</param>
    /// <param name="instance">class instance</param>
    /// <param name="objects">"Diagram", ""</param>
    /// <returns>object</returns>
    public Task<object?> InvokeWithInstanceAsync(string methodName, object? instance, object[] objects);

    /// <summary>
    /// Invoke method with generic class execute and pattern Id
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <param name="methodName"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<T?> InvokeWithGeneric<T>(string id, string methodName, params object[] parameters);
}

#endregion

#region Multi

/// <summary>
/// partial interface IClassManagerRepository
/// </summary>
public partial interface IClassManagerRepository
{
    /// <summary>
    /// For-each Invoke method with names class execute and pattern Id
    /// </summary>
    /// <param name="methodName">Generate</param>
    /// <param name="names">["InterruptionEventCoveragePattern", "EnumerationValueOption2Pattern"]</param>
    /// <param name="matrixObjects">[[],[]]</param>
    /// <returns>Status</returns>
    public IAsyncEnumerable<(string?, object?)> InvokeWithNamesYieldAsync(string methodName, IEnumerable<string> names, params object[][] matrixObjects);

    /// <summary>
    /// For-each Invoke method with ids class execute and pattern Id
    /// </summary>
    /// <param name="methodName">Generate</param>
    /// <param name="ids">["pattern2", "pattern3"]</param>
    /// <param name="matrixObjects">[[],[]]</param>
    /// <returns>Status</returns>
    public IAsyncEnumerable<(string?, object?)> InvokeWithIdsYieldAsync(string methodName, IEnumerable<string> ids, params object[][] matrixObjects);
}

#endregion