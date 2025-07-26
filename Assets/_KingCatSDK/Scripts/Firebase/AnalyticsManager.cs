using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnalyticsManager : MonoSingleton<AnalyticsManager>
{
#if ANALYTICS
    public void LogAppOpen()
    {
        LogEvent(Firebase.Analytics.FirebaseAnalytics.EventAppOpen);
    }

    public void LogEvent(string eventName)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);

        Debug.Log("Analytics: " + eventName);
    }

    public void LogEvent(string eventName, string eventParam, object eventValue)
    {
        if (eventValue is int intValue) Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, eventParam, intValue);
        else if (eventValue is long longValue) Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, eventParam, longValue);
        else if (eventValue is float floatValue) Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, eventParam, floatValue);
        else if (eventValue is string stringValue) Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, eventParam, stringValue);
        else if (eventValue is bool boolValue) Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, eventParam, boolValue ? "true" : "false");

        Debug.Log($"Analytics: {eventName}: {eventParam}:{eventValue}");
    }

    public void LogEventWithParams(string eventName, params (string, object)[] args)
    {
        List<Firebase.Analytics.Parameter> param = new List<Firebase.Analytics.Parameter>();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].Item2 is int intValue) param.Add(new Firebase.Analytics.Parameter(args[i].Item1, intValue));
            else if (args[i].Item2 is long longValue) param.Add(new Firebase.Analytics.Parameter(args[i].Item1, longValue));
            else if (args[i].Item2 is float floatValue) param.Add(new Firebase.Analytics.Parameter(args[i].Item1, floatValue));
            else if (args[i].Item2 is string stringValue) param.Add(new Firebase.Analytics.Parameter(args[i].Item1, stringValue));
            else if (args[i].Item2 is bool boolValue) param.Add(new Firebase.Analytics.Parameter(args[i].Item1, boolValue ? "true" : "false"));

        }

        // Convert the list back to an array before passing it to FirebaseAnalytics.LogEvent
        Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, param.ToArray());

        // Create a string representation of the parameters for logging
        string paramLog = string.Join(", ", args.Select(a => $"{a.Item1}: {a.Item2}"));
        Debug.Log($"Analytics: {eventName}: params: {paramLog}");
    }
#endif
}