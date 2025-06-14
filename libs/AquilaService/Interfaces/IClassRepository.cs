using System.Reflection;

namespace AquilaService.Interfaces;

public partial interface IClassRepository
{
    /// <summary>
    /// checked exist class in namespace
    /// </summary>
    /// <param name="nameClass"></param>
    /// <returns></returns>
    public bool Exist(string? nameClass);
    /// <summary>
    /// checked exist method in class in namespace
    /// </summary>
    /// <param name="nameClass"></param>
    /// <param name="nameMethod"></param>
    /// <returns></returns>
    public bool Exist(string? nameClass, string? nameMethod);
    /// <summary>
    /// get class with nameClass
    /// </summary>
    /// <param name="nameClass"></param>
    /// <returns></returns>
    public Type? Get(string? nameClass);
    /// <summary>
    /// get all class in namespace
    /// </summary>
    /// <param name="nameSpace"></param>
    /// <returns></returns>
    public IEnumerable<Type>? GetAll(string? nameSpace, bool isContains = false);
    /// <summary>
    /// get all class with assembly and namespace
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="nameSpace"></param>
    /// <returns></returns>
    public IEnumerable<Type>? GetAll(Assembly? assembly, string? nameSpace, bool isContains = false);
}

public partial interface IClassRepository
{
    /// <summary>
    /// invoke method with name method, class, parameter
    /// </summary>
    /// <param name="nameClass"></param>
    /// <param name="nameMethod"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public object? Invoke(string? nameClass, string? nameMethod, object?[]? parameters = null);
    /// <summary>
    /// invoke method with name method, class, parameter
    /// </summary>
    /// <param name="classType"></param>
    /// <param name="nameMethod"></param>
    /// <param name="parameters"></param>
    public object? Invoke(Type? classType, string? nameMethod, object?[]? parameters = null);
    /// <summary>
    /// invoke method with name method, class, parameter
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="classType"></param>
    /// <param name="nameMethod"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public object? Invoke(object? instance, Type? classType, string? nameMethod, object?[]? parameters = null);
    /// <summary>
    /// task invoke method with name method, class, parameter, instance class
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="classType"></param>
    /// <param name="nameMethod"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<object?> InvokeSync(object? instance, Type? classType, string? nameMethod, object?[]? parameters = null);
    /// <summary>
    /// task invoke method with name method, class, parameter, instance class
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="nameClass"></param>
    /// <param name="nameMethod"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<object?> InvokeSync(object? instance, string? nameClass, string? nameMethod, object?[]? parameters = null);
    /// <summary>
    /// task invoke method with name method, class, parameter
    /// </summary>
    /// <param name="nameClass"></param>
    /// <param name="nameMethod"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<object?> InvokeSync(string? nameClass, string? nameMethod, object?[]? parameters = null);
    /// <summary>
    /// task invoke method with name method, class, parameter
    /// </summary>
    /// <param name="classType"></param>
    /// <param name="nameMethod"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<object?> InvokeSync(Type? classType, string? nameMethod, object?[]? parameters = null);
    /// <summary>
    /// task invoke method with name method, class, parameter, instance class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="classType"></param>
    /// <param name="nameMethod"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<T?> InvokeSync<T>(object? instance, Type? classType, string? nameMethod, object?[]? parameters = null) where T : class;
    /// <summary>
    /// task invoke method with name method, class, parameter, instance class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="nameClass"></param>
    /// <param name="nameMethod"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<T?> InvokeSync<T>(object? instance, string? nameClass, string? nameMethod, object?[]? parameters = null) where T : class;
    /// <summary>
    /// task invoke method with name method, class, parameter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="classType"></param>
    /// <param name="nameMethod"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<T?> InvokeSync<T>(Type? classType, string? nameMethod, object?[]? parameters = null) where T : class;
    /// <summary>
    /// polymorphism convert object task invoke method with name method, class, parameter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="nameClass"></param>
    /// <param name="nameMethod"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<T?> InvokeSync<T>(string? nameClass, string? nameMethod, object?[]? parameters = null) where T : class;
    /// <summary>
    /// polymorphism convert object invoke method with name method, class, parameter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="nameClass"></param>
    /// <param name="nameMethod"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public T? Invoke<T>(string? nameClass, string? nameMethod, object?[]? parameters = null) where T : class;
    /// <summary>
    /// polymorphism convert object task invoke method with name method, class, parameter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="classType"></param>
    /// <param name="nameMethod"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public T? Invoke<T>(Type? classType, string? nameMethod, object?[]? parameters = null) where T : class;
}

public partial interface IClassRepository
{
    /// <summary>
    /// Get current instance of this action invoke
    /// </summary>
    /// <returns></returns>
    public object? CurrentInstance();

    /// <summary>
    /// Create instance with type class
    /// </summary>
    /// <param name="classType"></param>
    /// <returns></returns>
    public object? CreateInstance(Type? classType, IServiceProvider? serviceProvider = null);
}