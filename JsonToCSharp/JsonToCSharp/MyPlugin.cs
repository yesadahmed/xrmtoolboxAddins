using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace JsonToCSharp
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "JSON to CSharp"),
        ExportMetadata("Description", "Converts any CRM entity or valid json to c# class. Works with OAuth or Certifcate connections only."),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "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"),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class MyPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new MyPluginControl();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public MyPlugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
            

        }

        

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}