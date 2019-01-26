using Autofac;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Modules;
using BeforeOurTime.Models.Modules.Core.Managers;
using BeforeOurTime.Models.Modules.Script.ItemProperties.Javascripts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages.Admin.ScriptEditor
{
    public class VMScriptEditorPage : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Script code
        /// </summary>
        public string Script
        {
            get { return _script; }
            set { _script = value; NotifyPropertyChanged("Script"); }
        }
        private string _script { set; get; }
        /// <summary>
        /// Javascript function definitions
        /// </summary>
        public List<JavascriptFunctionDefinition> Functions
        {
            get { return _functions; }
            set { _functions = value; NotifyPropertyChanged("Functions"); }
        }
        private List<JavascriptFunctionDefinition> _functions { set; get; }
        /// <summary>
        /// Selected javascript function from picker
        /// </summary>
        public JavascriptFunctionDefinition SelectedFunction
        {
            get { return _selectedFunction; }
            set { _selectedFunction = value; NotifyPropertyChanged("SelectedFunction"); }
        }
        private JavascriptFunctionDefinition _selectedFunction { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public VMScriptEditorPage(IContainer container) : base(container)
        {
            Container.Resolve<IMessageService>().SendRequestAsync<ScriptReadJSDefinitionsResponse>(
                new ScriptReadJSDefinitionsRequest()).ContinueWith((responseTask) =>
                {
                    var response = responseTask.Result;
                    Functions = response.Definitions;
                });
        }
        /// <summary>
        /// Remove escape characters and set script string
        /// </summary>
        /// <param name="script"></param>
        public void SetScript(string script)
        {
            var beautifier = new Jsbeautifier.Beautifier(new Jsbeautifier.BeautifierOptions() {
                IndentSize = 4
            });
            Script = beautifier.Beautify(script);
            NotifyPropertyChanged("Script");
        }
        /// <summary>
        /// Get the script string
        /// </summary>
        /// <returns></returns>
        public string GetScript()
        {
            return Script;
        }
        /// <summary>
        /// Insert code snippet
        /// </summary>
        public void InsertFunction(JavascriptFunctionDefinition function)
        {
            Script = $"// {function.Description}\n{function.Prototype}\n{Script}";

//            if (snippetName == "OnUse(Item item, ItemCommand itemCommand, Item origin)")
//            {
//                Script = @"// Respond to a javascript callback command
//var onUse = function(item, itemCommand, origin) {
//    botEmote(300, ""I like "" + itemCommand.data.like);
//};" + $"\n{Script}";
//            }
//            if (snippetName == "OnItemRead")
//            {
//                Script = @"// Add javascript (onUse) command to item
//var onItemRead = function(item) {
//    botAddProperty(""BeforeOurTime.Models.Modules.Core.Models.Properties.CommandItemProperty"", {
//        ""commands"": [{
//            ""itemId"": item.Id,
//            ""id"": ""22a73822-6655-4b7b-aa2d-100b5c4a00a7"",
//            ""data"": {
//                ""like"": 5
//            },
//            ""name"": ""Run Javascript Callback""
//        }]
//    });
//};" + $"\n{Script}";
//            }
//            if (snippetName == "OnTick()")
//            {
//                Script = @"// Opportunity to execute code at regular interval
//var onTick = function() {
//};" + $"\n{Script}";
//            }
        }
    }
}
