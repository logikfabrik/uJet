**********************
Working with Templates
**********************

Template Synchronization
------------------------
When creating a template through the Umbraco back office, the template markup is saved to a `.cshtml` file in the `Views\\` folder, and the database.

Copying `.cshtml` files from one Umbraco setup to another is not enough to make the copied templates available in the back office. The database must reflect the contents of the `Views\\` folder.

uJet supports template synchronization. When your Umbraco application is started, uJet will scan the `Views\\` folder, looking for `.cshtml` files. Found files will be used as blueprints to synchronize your database. When your database has been updated, the copied templates will be available in the back office.

.. note::
   uJet will not synchronize files with file names that starts with an underscore. E.g. files such as `_layout.cshtml` and `_viewstart.cshtml` will be excluded when synchronizing templates. uJet does not scan subfolders in the `Views\\` folder.

Preview Template
----------------
The preview button in the Umbraco back office does not support documents of types without templates. This is by design in Umbraco.

When developing applications in uJet, using ASP.NET MVC conventions, Umbraco templates are not used. And, as a result, the preview button will be unavailable.

.. seealso:: For more information on the topic of ASP.NET MVC conventions see :doc:`working_with_mvc_conventions`.