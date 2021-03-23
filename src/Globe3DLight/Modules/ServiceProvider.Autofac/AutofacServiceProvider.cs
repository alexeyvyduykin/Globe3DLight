﻿using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Globe3DLight.ServiceProvider.Autofac
{
    /// <summary>
    /// Service provider based on lifetime scope.
    /// </summary>
    public class AutofacServiceProvider : IServiceProvider
    {
        private readonly ILifetimeScope _scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProvider"/> class.
        /// </summary>
        /// <param name="scope">The lifetime scope.</param>
        public AutofacServiceProvider(ILifetimeScope scope)
        {
            _scope = scope;
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">The type of resolved service object.</param>
        /// <returns>The instance of type <paramref name="serviceType"/>.</returns>
        object IServiceProvider.GetService(Type serviceType)
        {
            return _scope.Resolve(serviceType);
        }
    }
}
