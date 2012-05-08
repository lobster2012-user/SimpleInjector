﻿#region Copyright (c) 2012 S. van Deursen
/* The Simple Injector is an easy-to-use Inversion of Control library for .NET
 * 
 * Copyright (C) 2012 S. van Deursen
 * 
 * To contact me, please visit my blog at http://www.cuttingedge.it/blogs/steven/ or mail to steven at 
 * cuttingedge.it.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
 * associated documentation files (the "Software"), to deal in the Software without restriction, including 
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial 
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT 
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO 
 * EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE 
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

namespace SimpleInjector.Integration.Web.Mvc.LifetimeScoping
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;

    using SimpleInjector.Extensions.LifetimeScoping;

    /// <summary>
    /// Simple Injector Http Module for enabling lifetime scoping.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mvc",
        Justification = "Mvc is the word.")]
    public sealed class SimpleInjectorMvcLifetimeScopingHttpModule : IHttpModule
    {
        /// <summary>Initializes a module and prepares it to handle requests.</summary>
        /// <param name="context">An <see cref="HttpApplication"/> that provides access to the methods, 
        /// properties, and events common to all application objects within an ASP.NET application.</param>
        void IHttpModule.Init(HttpApplication context)
        {
            context.BeginRequest += this.BeginLifetimeScope;
            context.EndRequest += this.EndLifetimeScope;
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements 
        /// <see cref="IHttpModule"/>.
        /// </summary>
        void IHttpModule.Dispose()
        {
        }

        private void BeginLifetimeScope(object sender, EventArgs e)
        {
            var resolver = DependencyResolver.Current as SimpleInjectionDependencyResolver;

            if (resolver != null)
            {
                var scope = resolver.Container.BeginLifetimeScope();

                HttpContext.Current.Items.Add(typeof(LifetimeScope), scope);
            }
        }

        private void EndLifetimeScope(object sender, EventArgs e)
        {
            var scope = (LifetimeScope)HttpContext.Current.Items[typeof(LifetimeScope)];

            if (scope != null)
            {
                scope.Dispose();
            }
        }
    }
}