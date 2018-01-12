using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidMvcSharePoint.Services
{
    public class ServiceInjectionContainer : IDisposable
    {
        public ServiceInjectionContainer()
        {
            _kernel = new Ninject.StandardKernel(new ServiceDependencyResolver());
        }

        #region IDisposable Implementation
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _kernel.Dispose();
            }
        }
        #endregion

        private readonly Ninject.IKernel _kernel;
    }
}