*******
Logging
*******
uJet logs using the Umbraco wrapper for Apache log4net. uJet logging can easily be configured by editing the logging configuration in `Config\\log4net.config`. For more information on how to configure log4net, please refer to `the online manual <https://logging.apache.org/log4net/release/manual/introduction.html>`_.

The following configuration will write uJet debug logs to a separate file.

.. code-block:: xml

   <log4net>
     
     <root>
       <priority value="Info"/>
       <appender-ref ref="AsynchronousLog4NetAppender" />
     </root>
   
     <appender name="rollingFile" type="log4net.Appender.RollingFileAppender">
       <file type="log4net.Util.PatternString" value="App_Data\Logs\UmbracoTraceLog.%property{log4net:HostName}.txt" />
       <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
       <appendToFile value="true" />
       <rollingStyle value="Date" />
       <maximumFileSize value="5MB" />
       <layout type="log4net.Layout.PatternLayout">
         <conversionPattern value=" %date [P%property{processId}/D%property{appDomainId}/T%thread] %-5level %logger - %message%newline" />
       </layout>
       <encoding value="utf-8" />
     </appender>
   
     <appender name="AsynchronousLog4NetAppender" type="Umbraco.Core.Logging.ParallelForwardingAppender,Umbraco.Core">
       <appender-ref ref="rollingFile" />
       <filter type="log4net.Filter.LoggerMatchFilter">
         <loggerToMatch value="Logikfabrik.Umbraco.Jet" />
         <acceptOnMatch value="false" />
       </filter>
     </appender>
    
     <logger name="Logikfabrik.Umbraco.Jet">
       <level value="DEBUG" />
       <appender-ref ref="uJetAsynchronousLog4NetAppender" />
     </logger>
   
     <appender name="uJetAsynchronousLog4NetAppender" type="Umbraco.Core.Logging.ParallelForwardingAppender,Umbraco.Core">
       <appender-ref ref="uJetRollingFile" />
     </appender>
   
     <appender name="uJetRollingFile" type="log4net.Appender.RollingFileAppender">
       <file type="log4net.Util.PatternString" value="App_Data\Logs\uJetTraceLog.%property{log4net:HostName}.txt" />
       <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
       <appendToFile value="true" />
       <rollingStyle value="Date" />
       <maximumFileSize value="5MB" />
       <layout type="log4net.Layout.PatternLayout">
         <conversionPattern value=" %date [P%property{processId}/D%property{appDomainId}/T%thread] %-5level %logger - %message%newline" />
       </layout>
       <encoding value="utf-8" />
     </appender>
     
   </log4net>