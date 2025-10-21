using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace GHGlobalVars
{
  public class GHGlobalVarsInfo : GH_AssemblyInfo
  {
    public override string Name => "GHGlobalVars Info";

    //Return a 24x24 pixel bitmap to represent this GHA library.
    public override Bitmap Icon => null;

    //Return a short string describing the purpose of this GHA library.
    public override string Description => "A set of components for getting/setting global variables within a Grasshopper definition.";

    public override Guid Id => new Guid("ad48bf2e-6c14-452c-898b-8b501c0385ae");

    //Return a string identifying you or your company.
    public override string AuthorName => "SamJackman";

    //Return a string representing your preferred contact details.
    public override string AuthorContact => "samjackman10@hotmail.co.uk";

    //Return a string representing the version.  This returns the same version as the assembly.
    public override string AssemblyVersion => GetType().Assembly.GetName().Version.ToString();
  }
}