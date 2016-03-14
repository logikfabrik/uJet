*********
Archetype
*********
Archetype is a popular property editor for Umbraco 7. With Archetype it possible to create complex properties using existing editors.

The following is a guide to how Archetype can be used with uJet.

1. First, create a new solution in VS.

.. code-block:: bat

   PM> Install-Package uJet
   PM> Install-Package UmbracoCms
   PM> Install-Package Archetype

1. Create your Archetype data type. Pre-values define the Archetype configuration.

.. code-block:: csharp
   
   namespace Example.Models.DataTypes
   {
       using System.Collections.Generic;
       using Logikfabrik.Umbraco.Jet;
	   
       [DataType(typeof(string), "Imulus.Archetype")]
       public class MyDataType : IDataType
       {
           public Dictionary<string, string> PreValues => new Dictionary<string, string> { { "archetypeConfig", @"{
               'showAdvancedOptions':false,
               'startWithAddButton':false,
               'hideFieldsetToolbar':false,
               'enableMultipleFieldsets':false,
               'hideFieldsetControls':false,
               'hidePropertyLabel':false,
               'maxFieldsets':null,
               'enableCollapsing':true,
               'enableCloning':false,
               'enableDisabling':true,
               'enableDeepDatatypeRequests':false,
               'fieldsets':[
               {
                   'alias':'myProperty',
                   'remove':false,
                   'collapse':false,
                   'labelTemplate':'',
                   'icon':'',
                   'label':'My Property',
                   'properties':[
                   {
                       'alias':'property1',
                       'remove':false,
                       'collapse':true,
                       'label':'Property 1',
                       'helpText':'',
                       'dataTypeGuid':'0cc0eba1-9960-42c9-bf9b-60e150b429ae',
                       'value':'',
                       'aliasIsDirty':true,
                       '$$hashKey':'0RR'
                   },
                   {
                       'alias':'property2',
                       'remove':false,
                       'collapse':true,
                       'label':'Property 2',
                       'helpText':'',
                       'dataTypeGuid':'0cc0eba1-9960-42c9-bf9b-60e150b429ae',
                       'value':'',
                       'aliasIsDirty':true,
                       '$$hashKey':'0RS'
                   }],
                   'group':null,
                   'aliasIsDirty':true,
                   '$$hashKey':'0RI'
                   }],
               'fieldsetGroups':[],
               'selection':[]
           }" } };

           public string Property1 { get; set; }

           public string Property2 { get; set; }
       }
   }
   
2. Add an Archetype property to your document type; a public property of your data type.

.. code-block:: csharp
   
   namespace Example.Models.DocumentTypes
   {
       using DataTypes;
       using Logikfabrik.Umbraco.Jet;
	   
       [DocumentType("My Page")]
       public class MyPage
       {
           public MyDataType MyProperty { get; set; }
       }
   }

3. Create and register a data type definition mapping for your data type. The data type definition mapping will be used by uJet to map the property MyProperty to your data type.

.. code-block:: csharp
   
   namespace Example
   {
       using System;
       using System.Linq;
       using Logikfabrik.Umbraco.Jet.Mappings;
       using Models.DataTypes;
       using Umbraco.Core;
       using Umbraco.Core.Models;
	   
       public class MyDataTypeDataTypeDefinitionMapping : DataTypeDefinitionMapping
       {
           protected override Type[] SupportedTypes => new[] { typeof(MyDataType) };

           public override IDataTypeDefinition GetMappedDefinition(Type fromType)
           {
               return !CanMapToDefinition(fromType) ? null : GetDefinition();
           }

           private static IDataTypeDefinition GetDefinition()
           {
               var definitions = ApplicationContext.Current.Services.DataTypeService.GetDataTypeDefinitionByPropertyEditorAlias("Imulus.Archetype");

               return definitions.First(definition => definition.Name.Equals(typeof(MyDataType).Name));
           }
       }
   }

.. code-block:: csharp
   
   namespace Example
   {
       using Logikfabrik.Umbraco.Jet;
       using Logikfabrik.Umbraco.Jet.Mappings;
       using Models.DataTypes;
       using Umbraco.Core;
	   
       public class MyApplicationHandler : ApplicationHandler
       {
           public override void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
           {
               DataTypeDefinitionMappingRegistrar.Register<MyDataType>(new MyDataTypeDataTypeDefinitionMapping());
           }
       }
   }
   
4. Make sure uJet is configured to synchronize data types and document types. Fire up your Umbraco application, create a new document of type MyPage, and edit the value for MyProperty. That's it!

5. This step is optional. If you're using the uJet model binder, create and register a property value converter. The property value converter will be used by uJet to convert the property value into and instance of the data type created in step 1.

.. code-block:: csharp
   
   namespace Example
   {
       using System;
       using System.Linq;
       using Archetype.Models;
       using Logikfabrik.Umbraco.Jet.Web.Data.Converters;
       using Models.DataTypes;
	   
       public class MyDataTypePropertyValueConverter : IPropertyValueConverter
       {
           public bool CanConvertValue(string uiHint, Type from, Type to)
           {
               return to == typeof(MyDataType);
           }

           public object Convert(object value, Type to)
           {
               var model = value as ArchetypeModel;

               if (model == null)
               {
                   return null;
               }

               var fieldset = model.Fieldsets.First();

               return new MyDataType
               {
                   Property1 = fieldset.Properties.First(p => p.Alias.Equals("property1")).Value as string,
                   Property2 = fieldset.Properties.First(p => p.Alias.Equals("property2")).Value as string
               };
           }
       }
   }

.. code-block:: csharp
   
   namespace Example
   {
       using Logikfabrik.Umbraco.Jet;
       using Logikfabrik.Umbraco.Jet.Mappings;
       using Logikfabrik.Umbraco.Jet.Web.Data.Converters;
       using Models.DataTypes;
       using Umbraco.Core;
	   
       public class MyApplicationHandler : ApplicationHandler
       {
           public override void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
           {
               DataTypeDefinitionMappingRegistrar.Register<MyDataType>(new MyDataTypeDataTypeDefinitionMapping());
               PropertyValueConverterRegistrar.Register<MyDataType>(new MyDataTypePropertyValueConverter());
           }
       }
   }

.. code-block:: csharp
   
   namespace Example.Controllers
   {
       using System.Web.Mvc;
       using Logikfabrik.Umbraco.Jet.Web.Mvc;
       using Models.DocumentTypes;

       public class MyPageController : JetController
       {
           public ActionResult Index(MyPage model)
           {
               return View(model);
           }
       }
   }