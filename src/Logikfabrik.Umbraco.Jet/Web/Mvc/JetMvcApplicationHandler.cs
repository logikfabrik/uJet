//----------------------------------------------------------------------------------
// <copyright file="JetMvcApplicationHandler.cs" company="Logikfabrik">
//     The MIT License (MIT)
//
//     Copyright (c) 2015 anton(at)logikfabrik.se
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
//----------------------------------------------------------------------------------

namespace Logikfabrik.Umbraco.Jet.Web.Mvc
{
    using System.Web.Mvc;
    using Configuration;
    using global::Umbraco.Core;
    using global::Umbraco.Web.Mvc;

    public class JetMvcApplicationHandler : ApplicationHandler
    {
        private static readonly object Lock = new object();
        private static bool configured;
        
        public override void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            FilteredControllerFactoriesResolver.Current.InsertTypeBefore(typeof(RenderControllerFactory), typeof(JetControllerFactory));
        }

        public override void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            if (!this.IsInstalled)
            {
                return;
            }

            if (configured)
            {
                return;
            }

            lock (Lock)
            {
                // Synchronize.
                if (JetConfigurationManager.Synchronize.HasFlag(SynchronizationMode.DocumentTypes))
                {
                    new TemplateSynchronizationService().Synchronize();
                    new PreviewTemplateSynchronizationService().Synchronize();
                }

                ModelBinders.Binders.DefaultBinder = new JetModelBinder();

                // Adds the Jet view engine. The Jet view engine allows views to be structured using the ASP.NET MVC convention.
                ViewEngines.Engines.Insert(0, new JetViewEngine());

                configured = true;
            }
        }
    }
}
