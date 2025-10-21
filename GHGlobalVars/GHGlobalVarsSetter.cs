using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

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
      pManager.AddTextParameter("GlobalVars", "GV", "Globally accessible variables", GH_ParamAccess.item);
      pManager.AddTextParameter("GlobalVarTypes", "T", "Types of the globally accessible variables", GH_ParamAccess.item);


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
      // First, we need to retrieve all data from the input parameters.
      // We'll start by declaring variables and assigning them starting values.
      string key = "";
      object value = null;

      if (!DA.GetData(0, ref key)) return;
      if (!DA.GetData(1, ref value)) return;

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
 
      // The actual functionality for setting values and expiring getter components will be in a different methods, called below:
      SetGlobalVar(key, value);
      String globalVar = $"'{key}':{value.ToString()}";
      String globalVarType = value.GetType().ToString();
      ExpireGetters();

      // Finally assign the spiral to the output parameter.
      DA.SetData(0, globalVar);
      DA.SetData(1, globalVarType);
    }

    void SetGlobalVar(string key, object value)
    {
      // Implementation for setting a global variable to the global dictionary.
      GlobalState.Set(key, value);
    }

    void ExpireGetters()
    {
      // Implementation for expiring getter components goes here.
      var objects = this.OnPingDocument().Objects.OfType<GH_Component>();
      foreach (var obj in objects)
      {
          if (obj.Name == "GHGlobalVarsGetter")
          {
              obj.ExpireSolution(true);
          }
      }
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