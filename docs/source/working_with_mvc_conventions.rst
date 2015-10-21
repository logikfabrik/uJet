****************************
Working with MVC Conventions
****************************
In addition to enabling Code First development, uJet tries to close the gap between Umbraco and conventional ASP.NET MVC. uJet extends the capabilities of Umbraco; it does not strip it of any of its out-of-the-box features. uJet makes it possible for you to develop conventional MVC sites within Umbraco, while still levering the power of the CMS.

Controllers and Model Binding
-----------------------------
With uJet you're no longer bound by the `RenderModel`. Model binding allows uJet to serve your controller action methods with typed document instances. The models for Code First development are used to enable easy, typed access to the documents in Umbraco. These models can be passed on to you views for strongly typed views, without the need of view models.

To take advantage of this functionality, have your controllers inherit from `Logikfabrik.Umbraco.Jet.Web.Mvc.JetController` and redeclare your `Index` action methods.

Views
-----
In Umbraco the concept of templates and the concept of views are interchangeable; templates are Razor views saved as `.cshtml` files in the root of the `Views\\` folder.

When developing with uJet, the default ASP.NET MVC naming conventions are supported. Views can be saved in controller subfolders e.g. `Views\\Home\\`, and/or as shared views in the subfolder `Views\\Shared\\`. uJet does so by providing its own implementation of the `IViewEngine` interface.

Views following the default ASP.NET MVC naming conventions will not be treated as templates; they will not be available for manual selection in the Umbraco back office. This is by design; views following the default ASP.NET MVC naming conventions are not Umbraco templates.