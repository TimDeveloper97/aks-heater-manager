using AquilaService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;

namespace AquilaService.Repositories;

public partial class ClassRepository : IClassRepository
{
    public bool Exist(string? nameClass)
    {
        try
        {
            if (nameClass is not null)
            {
                // Get the Type object for the class
                Type? classType = Type.GetType(nameClass);

                return classType is not null;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return false;
    }

    public bool Exist(string? nameClass, string? nameMethod)
    {
        try
        {
            if (nameClass is not null
                && nameMethod is not null)
            {
                // Get the Type object for the class
                Type? classType = Type.GetType(nameClass);

                if (classType is not null)
                {
                    // Call the method on the instance
                    MethodInfo? methodInfo = classType.GetMethod(nameMethod);
                    return methodInfo is not null;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return false;
    }

    public Type? Get(string? nameClass)
    {
        try
        {
            if (nameClass is not null)
            {
                // Get the Type object for the class
                Type? classType = Type.GetType(nameClass);

                return classType;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return null;
    }

    public IEnumerable<Type>? GetAll(string? nameSpace, bool isContains = false)
    {
        try
        {
            if (nameSpace is not null)
            {
                // Get the assembly where the namespace is defined
                var assembly = Assembly.GetExecutingAssembly();

                // Get all types in the assembly
                var types = assembly.GetTypes();

                // Filter the types by the target namespace
                Type[] classesInNamespace;
                if (!isContains)
                    classesInNamespace = types.Where(t => t.Namespace == nameSpace && t.IsClass).ToArray();
                else
                    classesInNamespace = types.Where(t => t.Namespace?.Contains(nameSpace) ?? false && t.IsClass).ToArray();
                return classesInNamespace;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return null;
    }

    public IEnumerable<Type>? GetAll(Assembly? assembly, string? nameSpace, bool isContains = false)
    {
        try
        {
            if (nameSpace is not null
                && assembly is not null)
            {
                // Get all types in the assembly
                var types = assembly.GetTypes();

                // Filter the types by the target namespace
                Type[] classesInNamespace;
                if (!isContains)
                    classesInNamespace = types.Where(t => t.Namespace == nameSpace && t.IsClass).ToArray();
                else
                    classesInNamespace = types.Where(t => t.Namespace?.Contains(nameSpace) ?? false && t.IsClass).ToArray();
                return classesInNamespace;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return null;
    }
}

public partial class ClassRepository
{
    public object? Invoke(string? nameClass, string? nameMethod, object?[]? parameters = null)
    {
        try
        {
            if (nameClass is not null
            && nameMethod is not null)
            {
                // Get the Type object for the class
                Type? classType = Type.GetType(nameClass);

                // Check if the class type is not null
                if (classType is not null)
                {
                    // Create an instance of the class
                    object? instance = Activator.CreateInstance(classType);

                    _instance = instance;
                    // Call the method on the instance
                    MethodInfo? methodInfo = classType.GetMethod(nameMethod);
                    return methodInfo?.Invoke(instance, parameters);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw;
        }
        return null;
    }

    public object? Invoke(Type? classType, string? nameMethod, object?[]? parameters = null)
    {
        try
        {
            if (classType is not null
                && nameMethod is not null)
            {
                // Check if the class type is not null
                if (classType is not null)
                {
                    // Create an instance of the class
                    object? instance = Activator.CreateInstance(classType);

                    _instance = instance;
                    // Call the method on the instance
                    MethodInfo? methodInfo = classType.GetMethod(nameMethod);
                    return methodInfo?.Invoke(instance, parameters);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw;
        }
        return null;
    }

    public object? Invoke(object? instance, Type? classType, string? nameMethod, object?[]? parameters = null)
    {
        try
        {
            if (classType is not null
                && nameMethod is not null
                && instance is not null)
            {
                // Check if the class type is not null
                if (classType is not null)
                {
                    // Create an instance of the class
                    _instance = instance;

                    // Call the method on the instance
                    MethodInfo? methodInfo = classType.GetMethod(nameMethod);

                    return methodInfo?.Invoke(instance, parameters);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw;
        }
        return null;
    }

    public async Task<object?> InvokeSync(object? instance, string? nameClass, string? nameMethod, object?[]? parameters = null)
    {
        try
        {
            if (nameClass is not null
                && nameMethod is not null
                && instance is not null)
            {
                // Get the Type object for the class
                Type? classType = Type.GetType(nameClass);

                // Check if the class type is not null
                if (classType is not null)
                {
                    // Create an instance of the class
                    _instance = instance;

                    // Call the method on the instance
                    MethodInfo? methodInfo = classType.GetMethod(nameMethod);

                    // Convert task
                    dynamic? dynamicTask = methodInfo?.Invoke(instance, parameters);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var result = await dynamicTask.ConfigureAwait(false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    return result;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw;
        }
        return null;
    }

    public async Task<object?> InvokeSync(object? instance, Type? classType, string? nameMethod, object?[]? parameters = null)
    {
        try
        {
            if (classType is not null
                && nameMethod is not null
                && instance is not null)
            {
                // Check if the class type is not null
                if (classType is not null)
                {
                    // Create an instance of the class
                    _instance = instance;

                    // Call the method on the instance
                    MethodInfo? methodInfo = classType.GetMethod(nameMethod);

                    // Convert task
                    dynamic? dynamicTask = methodInfo?.Invoke(instance, parameters);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var result = await dynamicTask.ConfigureAwait(false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    return result;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw;
        }
        return null;
    }

    public async Task<object?> InvokeSync(string? nameClass, string? nameMethod, object?[]? parameters = null)
    {
        try
        {
            if (nameClass is not null
                && nameMethod is not null)
            {
                // Get the Type object for the class
                Type? classType = Type.GetType(nameClass);

                // Check if the class type is not null
                if (classType is not null)
                {
                    // Create an instance of the class
                    object? instance = Activator.CreateInstance(classType);

                    _instance = instance;
                    // Call the method on the instance
                    MethodInfo? methodInfo = classType.GetMethod(nameMethod);

                    // Convert task
                    dynamic? dynamicTask = methodInfo?.Invoke(instance, parameters);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var result = await dynamicTask.ConfigureAwait(false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    return result;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw;
        }
        return null;
    }

    public async Task<object?> InvokeSync(Type? classType, string? nameMethod, object?[]? parameters = null)
    {
        try
        {
            if (classType is not null
                && nameMethod is not null)
            {
                // Check if the class type is not null
                if (classType is not null)
                {
                    // Create an instance of the class
                    object? instance = Activator.CreateInstance(classType);

                    _instance = instance;
                    // Call the method on the instance
                    MethodInfo? methodInfo = classType.GetMethod(nameMethod);

                    // Convert task
                    dynamic? dynamicTask = methodInfo?.Invoke(instance, parameters);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var result = await dynamicTask.ConfigureAwait(false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    return result;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            throw;
        }
        return null;
    }

    public async Task<T?> InvokeSync<T>(object? instance, Type? classType, string? nameMethod, object?[]? parameters = null) where T : class
        => (await InvokeSync(instance, classType, nameMethod, parameters)) as T;

    public async Task<T?> InvokeSync<T>(object? instance, string? nameClass, string? nameMethod, object?[]? parameters = null) where T : class
        => (await InvokeSync(instance, nameClass, nameMethod, parameters)) as T;

    public async Task<T?> InvokeSync<T>(Type? classType, string? nameMethod, object?[]? parameters = null) where T : class
        => (await InvokeSync(classType, nameMethod, parameters)) as T;

    public async Task<T?> InvokeSync<T>(string? nameClass, string? nameMethod, object?[]? parameters = null) where T : class
        => (await InvokeSync(nameClass, nameMethod, parameters)) as T;

    public T? Invoke<T>(string? nameClass, string? nameMethod, object?[]? parameters = null) where T : class
            => Invoke(nameClass, nameMethod, parameters) as T;

    public T? Invoke<T>(Type? classType, string? nameMethod, object?[]? parameters = null) where T : class
            => Invoke(classType, nameMethod, parameters) as T;
}

public partial class ClassRepository
{
    private object? _instance;
    public object? CurrentInstance() => _instance;

    public object? CreateInstance(Type? classType, IServiceProvider? serviceProvider = null)
    {
        try
        {
            if (classType is not null)
            {
                object? instance = null;
                var contructor = classType.GetConstructors().FirstOrDefault();
                var parameters = contructor?.GetParameters();

                if (parameters?.Count() == 0)
                {
                    instance = Activator.CreateInstance(classType);
                }
                else if (parameters?.Count() > 0 && serviceProvider is not null)
                {
                    instance = ActivatorUtilities.CreateInstance(serviceProvider, classType);
                }

                _instance = instance;
                return instance;
            }
        }
        catch (Exception)
        {
            throw;
        }

        return null;
    }
}