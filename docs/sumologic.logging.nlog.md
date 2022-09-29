# SumoLogic.Logging.NLog

## Description

SumoLogic provides an interface between NLog and remote service Logging. You can logging its application as the usual way.
Once you have downloaded the NuGet package, you can watch the example project for configuration details.

## Minimum requirements

- .NET 4.5 or later or .NET Standard 1.3

## Targets

- SumoLogicTarget: It instantly pushes the log message to SumoLogic.
- BufferedSumoLogicTarget: It makes a log messages queue. It pushes the queue if the maximum quantity of messages was reached or if the maximum flush interval has passed.

## Configuration

- The configuration is in NLog.config. There are two target, BufferedSumoLogicTarget and SumoLogicTarget.
- We recommend to use the BufferedSumoLogicTarget because SumoLogicTarget might make the application runs slower.
- The output url is which you get from SumoLogic http collector.

## Properties

Like all NLog targets then formatting of message payload happens with the standard Layout-property:

- Default Layout = `${longdate}|${level:uppercase=true}|${logger}${exception:format=tostring}${newline}`

!NOTE! Older versions (<= 1.0.0.8) also reacted to the now obsolete `AppendException` property.
That controlled whether exception details was added independent of the configured NLog Target Layout.
Instead of using `AppendException` property, then one should just configure the NLog Target Layout as wanted.

### SumoLogicTarget

| Argument                  | Description                                                                           | Default value         |
|---------------------------|---------------------------------------------------------------------------------------|----------------------:|
| Url                       | The http collector URL from SumoLogic.                                                | __mandatory__         |
| SourceName                | The named used for messages sent to SumoLogic.                                        | `Nlog-SumoObject`     |
| SourceCategory            | The category used for messages sent to SumoLogic.                                     | `null`                |
| SourceHost                | The host used for messages sent to SumoLogic. Ex. ${machinename}                      | `null`                |
| ConnectionTimeout         | The connection timeout in milliseconds.                                               | `60000`               |

### BufferedSumoLogicTarget

| Argument                  | Description                                                                           | Default value             |
|---------------------------|---------------------------------------------------------------------------------------|--------------------------:|
| Url                       | The http collector URL from SumoLogic.                                                | __mandatory__             |
| SourceName                | The named used for messages sent to SumoLogic.                                        | `Nlog-SumoObject-Buffered`|
| SourceCategory            | The category used for messages sent to SumoLogic.                                     | `null`                    |
| SourceHost                | The host used for messages sent to SumoLogic. Ex. ${machinename}                      | `null`                    |
| ConnectionTimeout         | The connection timeout in milliseconds.                                               | `60000`                   |
| RetryInterval             | The send message retry interval, in milliseconds                                      | `10000`                   |
| FlushingAccuracy          | How often the messages queue is checked for messages to send, in milliseconds.        | `250`                     |
| MaxFlushInterval          | The maximum interval between flushes, in milliseconds.                                | `10000`                   |
| MessagePerRequest         | How many messages need to be in the queue before flushing.                            | `100`                     |
| MaxQueueSizeBytes         | The messages queue capacity in bytes. Messages will be dropped when it is exceeded.   | `1000000`                 |


## Internal Logging

The Sumo Logic NLog target can log internal status information and error messages for diagnostic purposes if needed.
It will automatically route these message to [NLog Internal Loggger](https://github.com/NLog/NLog/wiki/Internal-Logging/).

It is also possible to route the Sumo Logic debug messages to other destinations. The simplest way to enable this is
to set `UseConsoleLog = true` through the configuration XML. Internal logging will then be printed to the console.

If an alternative internal logging method is required, you can optionally specify a custom logger. Implement the interface 
`SumoLogic.Logging.Common.Log.ILog` and reconfigure targets at runtime per below:

```csharp
static void ReconfigureSumoTargets()
{
    foreach (var target in LogManager.Configuration.AllTargets)
    {
        if (!(target is SumoLogicTarget))
            continue;

        var originalTarget = target as SumoLogicTarget;

        var customTargetLogger = new ILogImpl();  // custom implementation of ILog goes here

        var newTarget = new SumoLogicTarget(customTargetLogger, null)
        {
            ConnectionTimeout = originalTarget.ConnectionTimeout,
            Layout = originalTarget.Layout,
            Name = originalTarget.Name,
            SourceName = originalTarget.SourceName,
            SourceCategory = originalTarget.SourceCategory,
            SourceHost = originalTarget.SourceHost,
            Url = originalTarget.Url,
            UseConsoleLog = false
        };

        if (originalTarget.Name != null)
        {
            LogManager.Configuration.RemoveTarget(originalTarget.Name);
            LogManager.Configuration.AddTarget(newTarget.Name, newTarget);
        }

        foreach (var rule in LogManager.Configuration.LoggingRules)
        {
            if (rule.Targets.Remove(originalTarget))
            {
                rule.Targets.Add(newTarget);
            }
        }
    }

    LogManager.ReconfigExistingLoggers();
}
```