# Sumo Logic .NET Appenders


[![Build status](https://ci.appveyor.com/api/projects/status/fnfnxdoanv5aux3f?svg=true)](https://ci.appveyor.com/project/bin3377/sumologic-net-appenders)
[![SumoLogic.Logging.Common](https://img.shields.io/nuget/v/SumoLogic.Logging.Common.svg?label=SumoLogic.Logging.Common&logo=nuget&logoColor=C0C0C0)](https://www.nuget.org/packages/SumoLogic.Logging.Common/)
[![SumoLogic.Logging.Log4Net](https://img.shields.io/nuget/v/SumoLogic.Logging.Log4Net.svg?label=SumoLogic.Logging.Log4Net&logo=nuget&logoColor=C0C0C0)](https://www.nuget.org/packages/SumoLogic.Logging.Log4Net/)
[![SumoLogic.Logging.NLog](https://img.shields.io/nuget/v/SumoLogic.Logging.NLog.svg?label=SumoLogic.Logging.NLog&logo=nuget&logoColor=C0C0C0)](https://www.nuget.org/packages/SumoLogic.Logging.NLog/)
[![SumoLogic.Logging.Serilog](https://img.shields.io/nuget/v/SumoLogic.Logging.Serilog.svg?label=SumoLogic.Logging.Serilog&logo=nuget&logoColor=C0C0C0)](https://www.nuget.org/packages/SumoLogic.Logging.Serilog/)
[![SumoLogic.Logging.AspNetCore](https://img.shields.io/nuget/v/SumoLogic.Logging.AspNetCore.svg?label=SumoLogic.Logging.AspNetCore&logo=nuget&logoColor=C0C0C0)](https://www.nuget.org/packages/SumoLogic.Logging.AspNetCore/)

Appenders for .NET logging frameworks which send data to Sumo Logic HTTP sources.

## Prerequisites

- .NET 4.5 or later or .NET Standard 1.5
- ASP.NET Core 2.0 or later for ASP.NET Core Logging provider 
- A Sumo Logic Account (trial can be started [here](https://www.sumologic.com/))

## Appenders

Appenders are provided for the following .NET logging frameworks

- NLog
- Log4Net
- Serilog
- ASP.NET Core Logging Provider (Beta)

All appenders have two implementations: a buffering and a non-buffering version.
The non-buffering implementations will send each log message to Sumo Logic in a distinct HTTP request. The buffering
implementations will queue messages until a size, count, or time threshold is met, then send in batch.

## NuGet Installation

### NLog

To install the NLog target, use the following steps

```ps
Install-Package SumoLogic.Logging.NLog
```

#### Example Appender Configuration for NLog

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog autoReload="true" internalLogToConsole="true">
  <extensions>
    <add assembly="SumoLogic.Logging.NLog"/>
  </extensions>
  <targets>
    <target name="sumoLogic" type="SumoLogicTarget"	layout="${date:format=yyyy-MM-dd HH\:mm\:ss.fff} ${level}, ${message}${exception:format=tostring}${newline}">
      <Url>https://collectors.us2.sumologic.com/receiver/v1/http/==your_endpoint_here==</Url>
      <ConnectionTimeout>30000</ConnectionTimeout>
      <SourceName>ExampleNameNLogTarget</SourceName>
      <SourceCategory>ExampleCategoryNLogTarget</SourceCategory>
      <SourceHost>ExampleHostNLogTarget</SourceHost>
      <UseConsoleLog>true</UseConsoleLog>
    </target>
    <target name="bufferedSumoLogic" type="BufferedSumoLogicTarget" layout="${date:format=yyyy-MM-dd HH\:mm\:ss.fff} ${level}, ${message}${exception:format=tostring}${newline}">
      <Url>https://collectors.us2.sumologic.com/receiver/v1/http/==your_endpoint_here==</Url>
      <SourceName>ExampleNameNLogBufferedTarget</SourceName>
      <SourceCategory>ExampleCategoryNLogBufferedTarget</SourceCategory>
      <SourceHost>ExampleHostNLogBufferedTarget</SourceHost>
      <ConnectionTimeout>30000</ConnectionTimeout>
      <RetryInterval>5000</RetryInterval>
      <MessagesPerRequest>10</MessagesPerRequest>
      <MaxFlushInterval>10000</MaxFlushInterval>
      <FlushingAccuracy>250</FlushingAccuracy>
      <MaxQueueSizeBytes>500000</MaxQueueSizeBytes>
      <UseConsoleLog>true</UseConsoleLog>
    </target>
  </targets>
  <rules>
    <logger name="*" minLevel="Debug" writeTo="sumoLogic"/>
    <logger name="*" minLevel="Debug" writeTo="bufferedSumoLogic"/>
  </rules>
</nlog>
```

#### Example Code For NLog

```csharp
public static class Program
{
    /// <summary>
    /// An example application that logs.
    /// </summary>
    public static void Main()
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        logger.Debug("Log message");
        Console.Read();
    }
}
```

More about NLog target configuration: [SumoLogic.Logging.NLog](docs/sumologic.logging.nlog.md)

### Log4Net

To install the Log4Net appender, use the following steps:

```ps
Install-Package SumoLogic.Logging.Log4Net
```

#### Example Appender Configuration for Log4Net

```xml
<configuration>
  <configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net"/>
  </configSections>
  <log4net debug="true">
    <appender name="SumoLogicAppender" type="SumoLogic.Logging.Log4Net.SumoLogicAppender, SumoLogic.Logging.Log4Net">
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger - %message%newline"/>
      </layout>
      <Url value="https://collectors.us2.sumologic.com/receiver/v1/http/==your_endpoint_here==" />
      <ConnectionTimeout value="30000" /> <!-- in milliseconds -->
      <SourceName value="ExampleNameLog4NetAppender" />
      <SourceCategory value="ExampleCategoryLog4NetAppender" />
      <SourceHost value="ExampleHostLog4NetAppender" />
      <UseConsoleLog value="true" />
    </appender>
    <appender name="BufferedSumoLogicAppender" type="SumoLogic.Logging.Log4Net.BufferedSumoLogicAppender, SumoLogic.Logging.Log4Net">
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger - %message%newline"/>
      </layout>
      <Url value="https://collectors.us2.sumologic.com/receiver/v1/http/==your_endpoint_here==" />
      <SourceName value="ExampleNameLog4NetBufferedAppender" />
      <SourceCategory value="ExampleCategoryLog4NetBufferedAppender" />
      <SourceHost value="ExampleHostLog4NetBufferedAppender" />
      <ConnectionTimeout value="30000" />
      <RetryInterval value="5000" />
      <MessagesPerRequest value="10" />
      <MaxFlushInterval value="10000" />
      <FlushingAccuracy value="250" />
      <MaxQueueSizeBytes value="500000" />
      <UseConsoleLog value="true" />
    </appender>
    <root>
      <priority value="ALL"/>
      <level value="ALL"/>
      <appender-ref ref="SumoLogicAppender"/>
      <appender-ref ref="BufferedSumoLogicAppender"/>
    </root>
    <logger name="SumoLogic.Logging.Log4Net.Example.Program">
      <level value="ALL"/>
    </logger>
  </log4net>

  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.0"/>
  </startup>
</configuration>
```

#### Example Code for Log4Net

```csharp
public static class Program
{
    /// <summary>
    /// The log4net log.
    /// </summary>
    private static ILog log4netLog = LogManager.GetLogger(typeof(Program));

    /// <summary>
    /// An example application that logs.
    /// </summary>
    public static void Main()
    {
        Console.WriteLine("Hello world!");
        log4netLog.Info("Hello world!");
        Console.ReadKey();
    }
}
```

More about Log4Net appender configuration: [SumoLogic.Logging.Log4Net](docs/sumologic.logging.log4net.md)

### Serilog

To install the Serilog sink, use the following steps:

```ps
Install-Package SumoLogic.Logging.Serilog
```

#### Example Code (instantiation and configuration)

```csharp
Logger log = new LoggerConfiguration()
    .WriteTo.BufferedSumoLogic(
        new Uri("https://collectors.us2.sumologic.com/receiver/v1/http/==your_endpoint_here=="),
        sourceName: "ExampleNameSerilogBufferedSink",
        sourceCategory: "ExampleCategorySerilogBufferedSink",
        sourceHost: "ExampleHostSerilogBufferedSink",
        connectionTimeout: 30000,
        retryInterval: 5000,
        messagesPerRequest: 10,
        maxFlushInterval: 10000,
        flushingAccuracy: 250,
        maxQueueSizeBytes: 500000)
    .CreateLogger();

log.Information("Hello world!");
```

More about Serilog sink configuration: [SumoLogic.Logging.Serilog](docs/sumologic.logging.serilog.md)

### ASP.NET Core Logging Provider

To install the logging provider, use the following steps:

```ps
Install-Package SumoLogic.Logging.AspNetCore
```

More about ASP.NET Core logging provider configuration: [SumoLogic.Logging.AspNetCore](docs/sumologic.logging.aspnetcore.md)

## TLS 1.2 Requirement

Sumo Logic only accepts connections from clients using TLS version 1.2 or greater. To utilize the content of this repo, ensure that it's running in an execution environment that is configured to use TLS 1.2 or greater.

## License

[Apache 2.0](LICENSE)
