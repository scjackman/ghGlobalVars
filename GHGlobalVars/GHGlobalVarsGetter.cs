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

    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
      // Declare variables and assigning start values.
      string key = "";
      object value = null;

      // Retrieve data from input parameters.
      if (!DA.GetData(0, ref key)) return;

      // Input validation.
      if (key == "")
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The key cannot be an empty string");
        return;
      }

      // Get the global variable.
      value = GetGlobalVar(key);

      // Finally assign values to the output parameters.
      DA.SetData(0, value != null ? value : "No value found for the provided key");
      DA.SetData(1, value != null ? GetTypeName(value) : "null");
    }

    object GetGlobalVar(string key)
    {
      // Implementation for getting a variable from the global dictionary.
      if (GlobalState.TryGet<object>(key, out var value))
      {
        return value;
      } 
      return null;
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