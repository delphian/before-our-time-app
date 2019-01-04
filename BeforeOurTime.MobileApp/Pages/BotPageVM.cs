using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages
{
    /// <summary>
    /// Base view model for a single page
    /// </summary>
    public class BotPageVM : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Depedency injection container
        /// </summary>
        public IContainer Container {
            set { _container = value; NotifyPropertyChanged("Container"); }
            get { return _container; }
        }
        protected IContainer _container { set; get; }
        /// <summary>
        /// Indicate if network request is still pending with service
        /// </summary>
        public bool Working
        {
            get { return _working; }
            set { _working = value; NotifyPropertyChanged("Working"); }
        }
        protected bool _working { set; get; } = false;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public BotPageVM(IContainer container)
        {
            Container = container;
        }
    }
}
