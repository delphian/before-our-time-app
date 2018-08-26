using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.Graph
{
    public class GraphEditorPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        public IEnumerable<testing> Testing
        {
            set { _testing = value; NotifyPropertyChanged("Testing"); }
            get { return _testing; }
        }
        private IEnumerable<testing> _testing { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public GraphEditorPageViewModel(IContainer container) : base(container)
        {
            Testing = new List<testing>() {
                new testing()
                {
                    Name = "test 1"
                },
                new testing()
                {
                    Name = "test 2"
                },
                new testing()
                {
                    Name = "test 3",
                    Children = new List<testing>()
                    {
                        new testing()
                        {
                            Name = "test 4"
                        },
                        new testing()
                        {
                            Name = "test 5"
                        }
                    }
                }
            };
        }
    }
    public class testing
    {
        public string Name { set; get; }
        public List<testing> Children { set; get; }
    }
}
