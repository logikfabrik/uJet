*************
Configuration
*************
uJet can be configured in `web.config`.

.. code-block:: xml

   <configuration>
     <configSections>
       <section name="logikfabrik.umbraco.jet" type="Logikfabrik.Umbraco.Jet.Configuration.JetSection, Logikfabrik.Umbraco.Jet" />
     </configSections>
     <logikfabrik.umbraco.jet synchronize="...">
       <assemblies>
         <add name="..." />
       </assemblies>
     </logikfabrik.umbraco.jet>
   </configuration>

Types of Type Classes to Scan
-----------------------------
uJet scans assemblies, looking for all types of type classes (document, media, and data types), by default. To limit the scan it's possible to combine the constants of the `SyncronizationMode` enumeration in `web.config`, e.g. `DocumentTypes, DataTypes` to scan and synchronize document, and data types.

The following constants of the `SyncronizationMode` enumeration can be set.

+-----------------+---------------------------------------------------------+
| Constants                                                                 |
+=================+=========================================================+
| `None`          | No type classes will be scanned and synchronized        |
+-----------------+---------------------------------------------------------+
| `DocumentTypes` | Document type classes will be scanned and synchronized  |
+-----------------+---------------------------------------------------------+
| `MediaTypes`    | Media type classes will be scanned and synchronized     |
+-----------------+---------------------------------------------------------+
| `DataTypes`     | Data type classes will be scanned and synchronized      |
+-----------------+---------------------------------------------------------+

.. code-block:: xml

   <configuration>
     <configSections>
       <section name="logikfabrik.umbraco.jet" type="Logikfabrik.Umbraco.Jet.Configuration.JetSection, Logikfabrik.Umbraco.Jet" />
     </configSections>
     <logikfabrik.umbraco.jet synchronize="..." />
   </configuration>

Assemblies to Scan
------------------
uJet scans all assemblies in the app domain, looking for all types of type classes, by default. To limit the scan it's possible to declare assemblies to scan in `web.config`. Assemblies are added by full name.

.. code-block:: xml

   <configuration>
     <configSections>
       <section name="logikfabrik.umbraco.jet" type="Logikfabrik.Umbraco.Jet.Configuration.JetSection, Logikfabrik.Umbraco.Jet" />
     </configSections>
     <logikfabrik.umbraco.jet>
       <assemblies>
         <add name="..." />
       </assemblies>
     </logikfabrik.umbraco.jet>
   </configuration>