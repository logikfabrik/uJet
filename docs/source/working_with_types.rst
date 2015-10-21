******************
Working with Types
******************
uJet supports document types, media types, member types and data types.

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
   Your document type classes can be concidered models. Following MVC convention, models are placed in the `Models\\` folder. It's recommended to place all document type classes in `Models\\DocumentTypes\\`.

When your Umbraco application is started, uJet will scan all assemblies in the app domain, looking for document type classes. Found classes will be used as blueprints to synchronize your database.

.. note::
   Assemblies to scan can be configured. Having uJet scan all app domain assemblies will have an impact on performance. Configuring assemblies is recommended if synchronization is enabled in your production environment.

DocumentTypeAttribute Properties
----------------------------------
The following document type properties can be set using the `DocumentTypeAttribute` attribute.

Name
^^^^
**Required**
Inherited from the `ContentTypeAttribute` attribute. The name of the document type. The document type name is displayed in the Umbraco back office.

Description
^^^^^^^^^^^
Inherited from the `ContentTypeAttribute` attribute. A description of the document type. The document type description is displayed in the Umbraco back office.

Icon
^^^^
Inherited from the `ContentTypeAttribute` attribute. The icon for the document type. The document type icon is displayed in the Umbraco back office.

Thumbnail
^^^^^^^^^
Inherited from the `ContentTypeAttribute` attribute. The thumbnail for the document type. The document type thumbnail is displayed in the Umbraco back office.

AllowedAsRoot
^^^^^^^^^^^^^
Inherited from the `ContentTypeAttribute` attribute. Whether or not documents of this type can be created at the root of the content tree.

AllowedChildNodeTypes
^^^^^^^^^^^^^^^^^^^^^
Inherited from the `ContentTypeAttribute` attribute. The allowed child document types of the document type.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.DocumentTypes
   {
       [DocumentType("My Page", AllowedChildNodeTypes = new[] {typeof(OurPage), typeof(TheirPage)})]
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
   Your media type classes can be concidered models. Following MVC convention, models are placed in the `Models\\` folder. It's recommended to place all media type classes in `Models\\MediaTypes\\`.

When your Umbraco application is started, uJet will scan all assemblies in the app domain, looking for media type classes. Found classes will be used as blueprints to synchronize your database.

.. note::
   Assemblies to scan can be configured. Having uJet scan all app domain assemblies will have an impact on performance. Configuring assemblies is recommended if synchronization is enabled in your production environment.
   
MediaTypeAttribute Properties
-------------------------------
The following media type properties can be set using the `MediaTypeAttribute` attribute.

Name
^^^^
**Required**
Inherited from the `ContentTypeAttribute` attribute. The name of the media type. The media type name is displayed in the Umbraco back office.

Description
^^^^^^^^^^^
Inherited from the `ContentTypeAttribute` attribute. A description of the media type. The media type description is displayed in the Umbraco back office.

Icon
^^^^
Inherited from the `ContentTypeAttribute` attribute. The icon for the media type. The media type icon is displayed in the Umbraco back office.

Thumbnail
^^^^^^^^^
Inherited from the `ContentTypeAttribute` attribute. The thumbnail for the media type. The media type thumbnail is displayed in the Umbraco back office.

AllowedAsRoot
^^^^^^^^^^^^^
Inherited from the `ContentTypeAttribute` attribute. Whether or not media of this type can be created at the root of the content tree.

AllowedChildNodeTypes
^^^^^^^^^^^^^^^^^^^^^
Inherited from the `ContentTypeAttribute` attribute. The allowed child media types of the media type.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;

   namespace Example.Models.MediaTypes
   {
       [DocumentType("My Media", AllowedChildNodeTypes = new[] {typeof(OurMedia), typeof(TheirMedia)})]
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
   Your member type classes can be concidered models. Following MVC convention, models are placed in the `Models\\` folder. It's recommended to place all member type classes in `Models\\MemberTypes\\`.

When your Umbraco application is started, uJet will scan all assemblies in the app domain, looking for member type classes. Found classes will be used as blueprints to synchronize your database.

.. note::
   Assemblies to scan can be configured. Having uJet scan all app domain assemblies will have an impact on performance. Configuring assemblies is recommended if synchronization is enabled in your production environment.

MemberTypeAttribute Properties
------------------------------
The following member type properties can be set using the `MemberTypeAttribute` attribute.

Name
^^^^
**Required**
Inherited from the `ContentTypeAttribute` attribute. The name of the member type. The member type name is displayed in the Umbraco back office.

Description
^^^^^^^^^^^
Inherited from the `ContentTypeAttribute` attribute. A description of the member type. The member type description is displayed in the Umbraco back office.

Icon
^^^^
Inherited from the `ContentTypeAttribute` attribute. The icon for the member type. The member type icon is displayed in the Umbraco back office.

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
   Your data type classes can be concidered models. Following MVC convention, models are placed in the `Models\\` folder. It's recommended to place all data type classes in `Models\\DataTypes\\`.

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