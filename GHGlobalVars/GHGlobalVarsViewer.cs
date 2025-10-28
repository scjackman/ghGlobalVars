using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;

namespace GHGlobalVars
{
  public class GHGlobalVarsViewer : GH_Component
  {
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public GHGlobalVarsViewer()
      : base("GHGlobalVarsViewer", "Viewer",
        "A component for viewing all available global variables.",
        "GHGlobalVars", "Viewer")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      // This component has no inputs.
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      // Use the pManager object to register your output parameters.
      // Output parameters do not have default values, but they too must have the correct access type.
      pManager.AddGenericParameter("Keys", "K", "Keys of all available global variables", GH_ParamAccess.list);
      pManager.AddGenericParameter("Values", "V", "All available global variables", GH_ParamAccess.list);
      pManager.AddTextParameter("ValueTypes", "T", "Types of the available global variables", GH_ParamAccess.list);

    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
      // Implementation for getting all global variables and returning to user.
      // Call method to get vars from global dictionary.
      Dictionary<string, object> dict = GetGlobalVars();

      // Prepare list of types of the global variables.
      List<string> typeList = new List<string>();
      foreach (var val in dict.Values)
      {
        typeList.Add(val != null ? GetTypeName(val) : "null");
      }

      // Finally assign values to the output parameters.
      DA.SetDataList(0, dict.Keys);
      DA.SetDataList(1, dict.Values);
      DA.SetDataList(2, typeList);
    }

    Dictionary<string, object> GetGlobalVars()
    {
      // Implementation for getting all variables from the global dictionary.
      return GlobalState.GetAll();
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
    public override Guid ComponentGuid => new Guid("30d0ee2e-2182-4213-92e3-0a996ad45bb7");
  }
}