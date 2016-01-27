******************
Working with Types
******************
uJet supports document types and media types (including inheritance and composition), member types and data types.

Document Types
==============
A document type is created by decorating a public non-abstract class, with a default constructor that takes no parameters, using the `DocumentTypeAttribute` attribute.

.. code-block:: csharp
   
   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.DocumentTypes
   {
       [DocumentType("My Page")]
       public class MyPage
       {
       }
   }

.. tip::
   Your document type classes can be concidered models. Following ASP.NET MVC conventions, models are placed in the `Models\\` folder. It's recommended to place all document type classes in `Models\\DocumentTypes\\`.

When your Umbraco application is started, uJet will scan all assemblies in the app domain, looking for document type classes. Found classes will be used as blueprints to synchronize your database.

.. note::
   Assemblies to scan can be configured. Having uJet scan all app domain assemblies will have an impact on performance. Configuring assemblies is recommended if synchronization is enabled in your production environment.

DocumentTypeAttribute Properties
--------------------------------
The following document type properties can be set using the `DocumentTypeAttribute` attribute.

Id
^^
The document type identifier. Specifying a document type identifier will enable document type tracking. Tracked document types can be renamed; uJet will keep your Umbraco database synchronized.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.DocumentTypes
   {
       [DocumentType("F3D4B9F1-711D-40A8-9091-FF5104CE0ACE", "My Page")]
       public class MyPage
       {
       }
   }

Name
^^^^
**Required**
The name of the document type. The document type name is displayed in the Umbraco back office.

Description
^^^^^^^^^^^
A description of the document type. The document type description is displayed in the Umbraco back office.

Icon
^^^^
The icon for the document type. The document type icon is displayed in the Umbraco back office.

Thumbnail
^^^^^^^^^
The thumbnail for the document type. The document type thumbnail is displayed in the Umbraco back office.

AllowedAsRoot
^^^^^^^^^^^^^
Whether or not documents of this type can be created at the root of the content tree.

AllowedChildNodeTypes
^^^^^^^^^^^^^^^^^^^^^
Which other types are allowed as child nodes to documents of this type in the content tree.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.DocumentTypes
   {
       [DocumentType("My Page", AllowedChildNodeTypes = new[] {typeof(OurPage), typeof(TheirPage)})]
       public class MyPage
       {
       }
   }

CompositionNodeTypes
^^^^^^^^^^^^^^^^^^^^
The composition document types of the document type.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.DocumentTypes
   {
       [DocumentType("My Page", CompositionNodeTypes = new[] {typeof(OurPage), typeof(TheirPage)})]
       public class MyPage
       {
       }
   }

Templates
^^^^^^^^^
The available templates (aliases) of the document type.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.DocumentTypes
   {
       [DocumentType("My Page", Templates = new []{"ourTemplate", "theirTemplate"})]
       public class MyPage
       {
       }
   }

.. seealso:: For more information on the topic of templates see :doc:`working_with_templates`.

DefaultTemplate
^^^^^^^^^^^^^^^
The default template (alias) of the document type.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.DocumentTypes
   {
       [DocumentType("My Page", DefaultTemplate = "myTemplate")]
       public class MyPage
       {
       }
   }

.. seealso:: For more information on the topic of templates see :doc:`working_with_templates`.

Media Types
===========
A media type is created by decorating a public non-abstract class, with a default constructor that takes no parameters, using the `MediaTypeAttribute` attribute.

.. code-block:: csharp
   
   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.MediaTypes
   {
       [MediaType("My Media")]
       public class MyMedia
       {
       }
   }

.. tip::
   Your media type classes can be concidered models. Following ASP.NET MVC conventions, models are placed in the `Models\\` folder. It's recommended to place all media type classes in `Models\\MediaTypes\\`.

When your Umbraco application is started, uJet will scan all assemblies in the app domain, looking for media type classes. Found classes will be used as blueprints to synchronize your database.

.. note::
   Assemblies to scan can be configured. Having uJet scan all app domain assemblies will have an impact on performance. Configuring assemblies is recommended if synchronization is enabled in your production environment.
   
MediaTypeAttribute Properties
-----------------------------
The following media type properties can be set using the `MediaTypeAttribute` attribute.

Id
^^
The media type identifier. Specifying a media type identifier will enable media type tracking. Tracked media types can be renamed; uJet will keep your Umbraco database synchronized.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.MediaTypes
   {
       [MediaType("6E1F2ED5-CBC2-4B46-AE70-79C5C6A9FACC", "My Media")]
       public class MyMedia
       {
       }
   }

Name
^^^^
**Required**
The name of the media type. The media type name is displayed in the Umbraco back office.

Description
^^^^^^^^^^^
A description of the media type. The media type description is displayed in the Umbraco back office.

Icon
^^^^
The icon for the media type. The media type icon is displayed in the Umbraco back office.

Thumbnail
^^^^^^^^^
The thumbnail for the media type. The media type thumbnail is displayed in the Umbraco back office.

AllowedAsRoot
^^^^^^^^^^^^^
Whether or not media of this type can be created at the root of the content tree.

AllowedChildNodeTypes
^^^^^^^^^^^^^^^^^^^^^
Which other types are allowed as child nodes to media of this type in the content tree.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.MediaTypes
   {
       [MediaType("My Media", AllowedChildNodeTypes = new[] {typeof(OurMedia), typeof(TheirMedia)})]
       public class MyMedia
       {
       }
   }

CompositionNodeTypes
^^^^^^^^^^^^^^^^^^^^
The composition media types of the media type.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.MediaTypes
   {
       [MediaType("My Media", CompositionNodeTypes = new[] {typeof(OurMedia), typeof(TheirMedia)})]
       public class MyMedia
       {
       }
   }

Member Types
============
A member type is created by decorating a public non-abstract class, with a default constructor that takes no parameters, using the `MemberTypeAttribute` attribute.

.. code-block:: csharp
   
   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.MemberTypes
   {
       [MemberType("My Member")]
       public class MyMember
       {
       }
   }

.. tip::
   Your member type classes can be concidered models. Following ASP.NET MVC conventions, models are placed in the `Models\\` folder. It's recommended to place all member type classes in `Models\\MemberTypes\\`.

When your Umbraco application is started, uJet will scan all assemblies in the app domain, looking for member type classes. Found classes will be used as blueprints to synchronize your database.

.. note::
   Assemblies to scan can be configured. Having uJet scan all app domain assemblies will have an impact on performance. Configuring assemblies is recommended if synchronization is enabled in your production environment.

MemberTypeAttribute Properties
------------------------------
The following member type properties can be set using the `MemberTypeAttribute` attribute.

Id
^^
The member type identifier. Specifying a member type identifier will enable member type tracking. Tracked member types can be renamed; uJet will keep your Umbraco database synchronized.

.. code-block:: csharp
   
   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.MemberTypes
   {
       [MemberType("DAE131E7-1159-4841-A669-3A39A4190903", "My Member")]
       public class MyMember
       {
       }
   }

Name
^^^^
**Required**
The name of the member type. The member type name is displayed in the Umbraco back office.

Description
^^^^^^^^^^^
A description of the member type. The member type description is displayed in the Umbraco back office.

Icon
^^^^
The icon for the member type. The member type icon is displayed in the Umbraco back office.

Data Types
==========
A data type is created by decorating a public non-abstract class, with a default constructor that takes no parameters, using the `DataTypeAttribute` attribute.

.. code-block:: csharp
   
   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.DataTypes
   {
       [DataType(typeof(int), "Umbraco.MediaPicker")]
       public class MyData
       {
       }
   }

.. tip::
   Your data type classes can be concidered models. Following ASP.NET MVC conventions, models are placed in the `Models\\` folder. It's recommended to place all data type classes in `Models\\DataTypes\\`.

When your Umbraco application is started, uJet will scan all assemblies in the app domain, looking for data type classes. Found classes will be used as blueprints to synchronize your database.

.. note::
   Assemblies to scan can be configured. Having uJet scan all app domain assemblies will have an impact on performance. Configuring assemblies is recommended if synchronization is enabled in your production environment.

DataTypeAttribute Properties
----------------------------
The following data type properties can be set using the `DataTypeAttribute` attribute.

Type
^^^^
**Required**
The type of the data type. The type property will determine how Umbraco stores property values of this data type in the Umbraco database (`DataTypeDatabaseType.Ntext`, `DataTypeDatabaseType.Integer`, or `DataTypeDatabaseType.Date`).

Editor
^^^^^^
**Required**
The editor of the data type. The editor property will determine which property editor will be used for editing property values of this data type in the Umbraco back office.

Type Tracking
=============
When a document, media, or member type is synchronized, uJet tries to match the type declared in code with a type definition. uJet creates an Umbraco alias for the type, based on the type name (namespace excluded), and uses that alias to look for a matching type definition in the database. If a match is found the definition is updated; if not, a new type definition is created. Renaming a type that has been synchronized, in code or using the Umbraco back office, will cause duplicate definitions to be created, with different aliases.

Type tracking refers to the use of the `id` parameter when declaring document types, media types, and member types in code. Using the `id` parameter uJet can keep track of types and their corresponding type definitions without relying on the type names. With type tracking, types can be renamed; uJet will keep your Umbraco database synchronized.