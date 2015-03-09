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

The following constants are declared in the `SyncronizationMode` enumeration.

+-----------------+---------------------------------------------------------+
| Constants                                                                 |
+=================+=========================================================+
| `None`          | Scan and synchronize no type classes                    |
+-----------------+---------------------------------------------------------+
| `DocumentTypes` | Scan and synchronize document type classes              |
+-----------------+---------------------------------------------------------+
| `MediaTypes`    | Scan and synchronize media type classes                 |
+-----------------+---------------------------------------------------------+
| `DataTypes`     | Scan and synchronize data type classes                  |
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