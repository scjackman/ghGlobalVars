using System;
using System.Linq;

using Grasshopper.Kernel;

namespace GHGlobalVars
{
  public class GHGlobalVarsGetter : GH_Component
  {
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public GHGlobalVarsGetter()
      : base("GHGlobalVarsGetter", "Getter",
        "A component for getting global variables on the Grasshopper canvas.",
        "GHGlobalVars", "Getters & Setters")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      // Use the pManager object to register your input parameters.
      // You can often supply default values when creating parameters.
      // All parameters must have the correct access type. If you want 
      // to import lists or trees of values, modify the ParamAccess flag.
      pManager.AddTextParameter("Key", "K", "The key of the global variable to get.", GH_ParamAccess.item);

      // If you want to change properties of certain parameters, 
      // you can use the pManager instance to access them by index:
      //pManager[0].Optional = true;
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter("Value", "V", "Value associated with the input key", GH_ParamAccess.item);
      pManager.AddTextParameter("ValueType", "T", "Type of the value associated with the input key", GH_ParamAccess.item);

      // Sometimes you want to hide a specific parameter from the Rhino preview.
      // You can use the HideParameter() method as a quick way:
      //pManager.HideParameter(0);
    }

    /// <summary>
    /// This is the method that actually does the work.
    /// </summary>
    /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
    /// to store data in output parameters.</param>
    protected override void SolveInstance(IGH_DataAccess DA)
    {
      // Implementation for getting a global variable from the 'sticky' dictionary.
      string key = "";
      object value = null;

      if (!DA.GetData(0, ref key)) return;

      if (key == "")
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The key cannot be an empty string");
        return;
      }

      value = GetGlobalVar(key);
      DA.SetData(0, value != null ? value : "No value found for the provided key");
      DA.SetData(1, value != null ? GetTypeName(value) : "null");
    }

    object GetGlobalVar(string key)
    {
      // Implementation for setting a global variable to the global dictionary.
      GlobalState.TryGet<object>(key, out var value);
      return value;
    }

    String GetTypeName(object value)
    {
      if (value == null)
      {
        return "null";
      }
      String typeName = value.GetType().Name;
      return typeName.Split('.').Last();
    }

    /// <summary>
    /// The Exposure property controls where in the panel a component icon 
    /// will appear. There are seven possible locations (primary to septenary), 
    /// each of which can be combined with the GH_Exposure.obscure flag, which 
    /// ensures the component will only be visible on panel dropdowns.
    /// </summary>
    public override GH_Exposure Exposure => GH_Exposure.primary;

    /// <summary>
    /// Provides an Icon for every component that will be visible in the User Interface.
    /// Icons need to be 24x24 pixels.
    /// You can add image files to your project resources and access them like this:
    /// return Resources.IconForThisComponent;
    /// </summary>
    protected override System.Drawing.Bitmap Icon => null;

    /// <summary>
    /// Each component must have a unique Guid to identify it. 
    /// It is vital this Guid doesn't change otherwise old ghx files 
    /// that use the old ID will partially fail during loading.
    /// </summary>
    public override Guid ComponentGuid => new Guid("32c8d5ee-2f6d-4c1c-a68f-8967cb20fb90");
  }
}