*************
Configuration
*************
uJet can be configured in `web.config`. The section `logikfabrik.umbraco.jet` allows for configuration.

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
uJet scans assemblies, looking for all types of type classes (document, media, member, and data types), by default. To limit the scan it's possible to combine the constants of the `SynchronizationMode` enumeration in `web.config`, e.g. `DocumentTypes, DataTypes` to scan and synchronize document, and data types only.

The following constants are declared in the `SynchronizationMode` enumeration.

+-----------------+---------------------------------------------------------+
| Constants                                                                 |
+=================+=========================================================+
| `None`          | Do not scan or synchronize type classes                 |
+-----------------+---------------------------------------------------------+
| `DocumentTypes` | Scan and synchronize document type classes              |
+-----------------+---------------------------------------------------------+
| `MediaTypes`    | Scan and synchronize media type classes                 |
+-----------------+---------------------------------------------------------+
| `MemberTypes`   | Scan and synchronize member type classes                |
+-----------------+---------------------------------------------------------+
| `DataTypes`     | Scan and synchronize data type classes                  |
+-----------------+---------------------------------------------------------+

.. note::
   Template synchronization and use of the built-in preview template, `PreviewTemplateAttribute`, requires document type synchronization to be enabled. Once uJet has synchronized all document types, document type synchronization can be disabled; it will still be possible to preview documents through the Umbraco back office. Template synchronization will, on the other hand, be disabled.

.. code-block:: xml

   <configuration>
     <configSections>
       <section name="logikfabrik.umbraco.jet" type="Logikfabrik.Umbraco.Jet.Configuration.JetSection, Logikfabrik.Umbraco.Jet" />
     </configSections>
     <logikfabrik.umbraco.jet synchronize="..." />
   </configuration>

Assemblies to Scan
------------------
uJet scans all assemblies in the app domain, looking for all types of type classes, by default. To limit the scan, it's possible to declare assemblies to scan in `web.config`. Assemblies are added by full name, case sensitive. No other assemblies in the app domain will be scanned.

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