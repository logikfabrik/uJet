# Media Types

A media type is created by decorating a public non-abstract class, with a constructor that takes no parameters, using the `MediaTypeAttribute` attribute.

When your Umbraco application is started, uJet will scan all assemblies in the app domain, looking for media type classes. Found classes will be used as blueprints to synchronize your database.

    Assemblies to scan can be configured. Having uJet scan all app domain assemblies will have an impact on performance. Configuring assemblies is recommended if synchronization is enabled in your production environment.

    Synchronization can be configured. uJet can be configured to synchronize document types, media types and/or data types. Synchronization can also be disabled. Disabling synchronization is recommended in your production environment, once your production database has been synchronized.