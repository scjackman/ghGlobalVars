using System;
using System.Linq;

using Grasshopper.Kernel;

namespace GHGlobalVars
{
  public class GHGlobalVarsSetter : GH_Component
  {
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public GHGlobalVarsSetter()
      : base("GHGlobalVarsSetter", "Setter",
        "A component for setting global variables on the Grasshopper canvas.",
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

      pManager.AddTextParameter("Keys", "K", "Keys for the global variables to be added.", GH_ParamAccess.item);
      pManager.AddGenericParameter("Values", "V", "Values associated with the provided keys", GH_ParamAccess.item);

    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.

      pManager.AddTextParameter("GlobalVars", "GV", "Globally accessible variables", GH_ParamAccess.item);
      pManager.AddTextParameter("GlobalVarTypes", "T", "Types of the globally accessible variables", GH_ParamAccess.item);

    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
      // Declare variables and assigning start values.
      string key = "";
      object value = null;

      // Retrieve data from input parameters.
      if (!DA.GetData(0, ref key)) return;
      if (!DA.GetData(1, ref value)) return;

      // Input validation.
      if (key == "")
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The key cannot be an empty string");
        return;
      }

      if (value == null)
      {
        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No value has been provided");
        return;
      }

      // Add key:value pair to dictionary and expire 'getter' components.
      SetGlobalVar(key, value);
      ExpireGetters();

      // Prepare output values.
      String globalVar = $"'{key}':{value}";
      String globalVarType = GetTypeName(value);
      
      // Finally assign values to the output parameters.
      DA.SetData(0, globalVar);
      DA.SetData(1, globalVarType);
    }

    void SetGlobalVar(string key, object value)
    {
      // Implementation for setting a variable to the global dictionary.
      GlobalState.Set(key, value);
    }

    void ExpireGetters()
    {
      // Implementation for expiring getter components.
      // Expire all GHGlobalVarsGetter and GHGlobalVarsViewer components in the document and force recompute to show updated dict.
      var objects = OnPingDocument().Objects.OfType<GH_Component>();
      foreach (var obj in objects)
      {
        if (obj.Name == "GHGlobalVarsGetter" || obj.Name == "GHGlobalVarsViewer")
        {
          obj.ExpireSolution(true);
        }
      }
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
    public override Guid ComponentGuid => new Guid("beb31e00-a775-4858-bf2d-b541bf4ac55a");
  }
}