using System;
using System.Threading;

public static class Monade
{
    public static T Do<T>(this T subject, Action<T> action) where T : class
    {
        if(subject != null)
        {
            action(subject);
        }
        return subject;
    }

    public static T As<T>(this object subject) where T : class
    {
        return subject as T;
    }

    public static T If<T>(this T subject, Func<T, bool> condition) where T : class
    {
        return subject != null && condition(subject) ? subject : null;
    }

    public static T IfNot<T>(this T subject, Func<T, bool> condition) where T : class
    {
        return subject == null || condition(subject) ? null : subject;
    }

    public static R Return<T,R>(this T subject, Func<T, R> resultFunction, Func<R> failureFunction = null) where T : class
    {
        if(subject != null)
        {
            return resultFunction(subject);
        }
        return failureFunction == null ? default(R) : failureFunction();
    }

    public static bool ReturnSuccess<T>(this T subject) where T : class
    {
        return subject != null;
    }
}
