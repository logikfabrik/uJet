***********************
Working with Properties
***********************
Document and media type properties are created by adding properties with public getters and setters to classes decorated using the `DocumentTypeAttribute` or `MediaTypeAttribute` attributes.

When your Umbraco application is started, uJet will scan your document and media type classes, looking for properties. Found properties will be used as blueprints to synchronize your database.

Data Annotations
================
Data annotations can be used to customize the user experience in the Umbraco back office, e.g. to set property name and description.

Supported Data Annotations
--------------------------
uJet supports the following data annotations.

* `RequiredAttribute`
* `DefaultValueAttribute`
* `RegularExpressionAttribute`
* `UIHintAttribute`
* `DisplayAttribute`
* `ScaffoldColumnAttribute`

RequiredAttribute
^^^^^^^^^^^^^^^^^
Properties decorated using the `RequiredAttribute` attribute will be mandatory in the Umbraco back office.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;
   using System.ComponentModel.DataAnnotations;

   namespace Example.Mvc.Models.DocumentTypes
   {
       [DocumentType("My Page")]
       public class MyPage
       {
           [Required]
           public string MyProperty { get; set; }
       }
   }

DefaultValueAttribute
^^^^^^^^^^^^^^^^^^^^^
Properties decorated using the `DefaultValueAttribute` attribute will have a default value of whatever value was set when setting the attribute.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;
   using System.ComponentModel;

   namespace Example.Mvc.Models.DocumentTypes
   {
       [DocumentType("My Page")]
       public class MyPage
       {
           [DefaultValue("My Default Value")]
           public string MyProperty { get; set; }
       }
   }

RegularExpressionAttribute
^^^^^^^^^^^^^^^^^^^^^^^^^^
Properties decorated using the `RegularExpressionAttribute` attribute will be validated in the Umbraco back office using the regular expression specified.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;
   using System.ComponentModel.DataAnnotations;

   namespace Example.Mvc.Models.DocumentTypes
   {
       [DocumentType("My Page")]
       public class MyPage
       {
           [RegularExpression("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*")]
           public string MyEmailProperty { get; set; }
       }
   }

DisplayAttribute
^^^^^^^^^^^^^^^^
Use the `DisplayAttribute` attribute to customize the property name and description in the Umbraco back office. The `DisplayAttribute` attribute makes it possible to set sort order (`Order`), and property group (`GroupName`) also.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;
   using System.ComponentModel.DataAnnotations;

   namespace Example.Mvc.Models.DocumentTypes
   {
       [DocumentType("My Page")]
       public class MyPage
       {
           [Display(Name = "My Property", Description = "Description of My Property", GroupName = "My Tab", Order = 1)]
           public string MyProperty { get; set; }
       }
   }

UIHintAttribute
^^^^^^^^^^^^^^^
Use the `UIHintAttribute` attribute to specify the property editor used in the Umbraco back office. The property editor is inferred by the property type by default, but can be overridden using this attribute.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;
   using System.ComponentModel.DataAnnotations;

   namespace Example.Mvc.Models.DocumentTypes
   {
       [DocumentType("My Page")]
       public class MyPage
       {
           [UIHint("ContentPicker")]
           public int MyContentProperty { get; set; }
       }
   }

ScaffoldColumnAttribute
^^^^^^^^^^^^^^^^^^^^^^^
Properties decorated using the `ScaffoldColumnAttribute` attribute (set to false) will not be available for editors through the Umbraco back office.

.. code-block:: csharp

   using Logikfabrik.Umbraco.Jet;
   using System.ComponentModel.DataAnnotations;

   namespace Example.Mvc.Models.DocumentTypes
   {
       [DocumentType("My Page")]
       public class MyPage
       {
           [ScaffoldColumn(false)]
           public string MyHiddenProperty { get; set; }
       }
   }

Data Types
==========
.NET data types are mapped to Umbraco data type definitions using data type definition mappings.

The data type property definition mapped will determine how Umbraco stores the property value in the database, and what editor to use for editing the property value in the Umbraco back office.

Supported .NET Data Types
-------------------------

uJet supports the following .NET data types out-of-the-box.

* `Int16` and `Int16?`
* `Int32` and `Int32?`
* `UInt16` and `UInt16?`
* `UInt32` and `UInt32?`
* `string`
* `decimal` and `decimal?`
* `float` and `float?`
* `double` and `double?`
* `DateTime` and `DateTime?`

.. seealso::
   The uJet .NET data type support can be extended by writing custom data type definition mappings.