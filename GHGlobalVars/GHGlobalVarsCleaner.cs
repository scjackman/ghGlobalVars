using System;
using System.Linq;

using Grasshopper.Kernel;

namespace GHGlobalVars
{
  public class GHGlobalVarsCleaner : GH_Component
  {
    /// <summary>
    /// Each implementation of GH_Component must provide a public 
    /// constructor without any arguments.
    /// Category represents the Tab in which the component will appear, 
    /// Subcategory the panel. If you use non-existing tab or panel names, 
    /// new tabs/panels will automatically be created.
    /// </summary>
    public GHGlobalVarsCleaner()
      : base("GHGlobalVarsCleaner", "Cleaner",
        "A component for clearing the global dictionary.",
        "GHGlobalVars", "Cleaner")
    {
    }

    /// <summary>
    /// Registers all the input parameters for this component.
    /// </summary>
    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddGenericParameter("Clean", "C", "A boolean value to trigger cleaning the global dictionary.", GH_ParamAccess.item);
    }

    /// <summary>
    /// Registers all the output parameters for this component.
    /// </summary>
    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      // This component has no outputs.
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
      // Implementation for clearing the global dictionary.
      // Declare variables and assigning start values.
      bool cleanDict = false;

      // Try and retrieve data from input boolean.
      if (!DA.GetData(0, ref cleanDict)) return;

      // If input bool is true, clear the global dictionary'
      if (cleanDict)
      {
        ClearGlobalVars();
        ExpireGetters();
        return;
      }
    }

    void ClearGlobalVars()
    {
      // Implementation for clearing the global dictionary.
      GlobalState.Clear();
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
    public override Guid ComponentGuid => new Guid("31a19c1b-ec55-4972-9403-f5a1d138c030");
  }
}