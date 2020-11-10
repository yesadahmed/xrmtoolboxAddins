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
        ExportMetadata("Description", "Converts any CRM entity or json to c# class. Helpful for Dynamic365 WebAPI models."),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAA3QAAAN0BcFOiBwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAPQSURBVFiFtZdbaFxVFIa/dc5kEpukzUNgMknR1BaTiQHFaoo3MBa1ELAlYLURKvoqCD6IPkRbENEHn0QRClbEh9KQCxTRJhGi6INCVVA701SoTXNrTGwSM7nNnLOXD2nSJLPnmvR//Nd/9v+fzVp7nyOqSqGYHYt9gPKmpdS+qybyXi5rOAW7A2p0ycaLcCzXNWQrOzA9HGt0HP5IU/5eVE6JI7+UV9ddBqxGWwoAMDMeOyKGV4GDgKSR/SbK8zt3R/7a9gCriI//GfGN0w1Sb6urEq1YKnqAffuW1/Nb6oH1KAs3xkA+SlcXoWG6NNG0md+2AACqBDPVxZfK2xZgbmzwURF9LYPEx+XXzWQg28JTU4PlwYQ5jtKEsF+VshSRUIxSlb4HAeTtinD9UAqbqQlnR6MHQT4D7soWdB0U5ALoAqAKw4h2VlQ3nLOJ0+7AzOhgsyD9ZH6tFIjIizur68/kqrf2wORktEwwp/M1BxDkWj56a4DipBwHavM1BzBqumZGoicXhmLhggOoYX8h5jcREpETyQBDsxODe7KJrT2gwqjA2S2EAMD42gj8nUmzbUdxodiwA9fbWo6CvlLoYr5SuuCZlLvAdeSlu89+83XWAIjsEeUZcV3UmJUpzgeqeMak0CLOgWjr03cKMrOsifP39wzMrNY2NKHAFEBpKEygpCQ/c9Jc+IBv9B0RPkX0TLFTFL3Yeug+awCFKwDxsRG8xcW8A/hpdsys8JeBOBB2xJxKE0CvAATLytkRqso7gLEEcEV8cKojXX11Dhy6ST949eXmkpQA4XuahoFhb3GRwB07ECe/y9IzqQHEkeFI1/lxABU5AoByqfbzgSWwjOHEsZZ2hHdLQ2GM57H472Ru5qr8l/BS+KAjXyR8dRGeBKoBVPWNhu7+D60B/mk7HFK8a+K6wbKa3SxP3yAxN5c1wLzns+xvnABXZENfCIwY5PWGrt7ONc52EE20tbwPvOWWlOC4AZLz8YzmviqzlrcPODLtGf1KVH/w4MfGnm8vscnQehSHvNL2icD8AX9pqdnPaL0yevFkqiroyk97O3ofzvJ4mk+yjg6fgPcCcDXbAvNJP2X8ihyZ2HvvI49nexay3AXjbc9WCv6Xcmt81mBUiXt+SucXO87PztTyE7UDA9a/JmuA75qbA1WVgcesAqNSXlzUJuo8tMqpatG879cA7i0OXEd6kuqfthtJvK6z74I1ACISa31qGtiVS+pCIHCuvqvv8GZ+pQdUVeCT22W+YiEf2/i1Jrx+I3lC0JPk0Hh5wAP5XZHnIt29/TbB/0YCjVwDYWpcAAAAAElFTkSuQmCC"),
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