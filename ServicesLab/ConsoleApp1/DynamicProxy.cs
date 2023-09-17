using System.Reflection;
using System.Xml.Serialization;

namespace ConsoleApp1;

public class EventEventArgs : EventArgs
{
    public readonly MethodInfo? TargetMethod;
    public readonly object?[]? Args;
    public EventEventArgs(MethodInfo? targetMethod, object?[]? args)
    {
        TargetMethod = targetMethod;
        Args = args;
    }
}

public class DynamicProxy<T> : DispatchProxy where T : class
{
    private T _target { get; set; }
    //private Predicate<MethodInfo>? _filter;
    //public event EventHandler<EventEventArgs>? BeforeExecute;
    //public event EventHandler<EventEventArgs>? AfterExecute;
    //public event EventHandler<EventEventArgs>? ErrorExecute;
    private MethodInfo? _targetMethod;
    private object?[]? _args;

    public static T? Decorate(T target = null)
    {
        var proxy = Create<T, DynamicProxy<T>>() as DynamicProxy<T>;
        proxy._target = target ?? Activator.CreateInstance<T>();
        //proxy.Filter = m => true;
        return proxy as T;
    }
    //public Predicate<MethodInfo>? Filter
    //{
    //    get { return _filter; }
    //    set
    //    {
    //        if (value == null)
    //            _filter = m => true;
    //        else
    //            _filter = value;
    //    }
    //}
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        try
        {
            _targetMethod = targetMethod;
            _args = args;
            var attrs = _target.GetType().GetMethod(targetMethod.Name).GetCustomAttributes();
            if (attrs.Any())
            {
                foreach (var a in attrs)
                {
                    if (a is LogAttribute attr)
                    {
                        Console.WriteLine(attr.Text);
                    }
                }
            }
            var result = targetMethod?.Invoke(_target, args);
            return result;
        }
        catch (TargetInvocationException exc)
        {
            //OnErrorExecute();
            throw exc.InnerException;
        }
    }

    //private void OnBeforeExecute()
    //{
    //    if (BeforeExecute != null)
    //    {
    //        var methodInfo = _targetMethod.GetBaseDefinition();
    //        if (_filter(methodInfo))
    //            BeforeExecute(this, new EventEventArgs(_targetMethod, _args));
    //    }
    //}
    //private void OnAfterExecute()
    //{
    //    if (AfterExecute != null)
    //    {
    //        var methodInfo = _targetMethod.GetBaseDefinition();
    //        if (_filter(methodInfo))
    //            AfterExecute(this, new EventEventArgs(_targetMethod, _args));
    //    }
    //}
    //private void OnErrorExecute()
    //{
    //    if (ErrorExecute != null)
    //    {
    //        var methodInfo = _targetMethod.GetBaseDefinition();
    //        if (_filter(methodInfo))
    //            ErrorExecute(this, new EventEventArgs(_targetMethod, _args));
    //    }
    //}
}