#region

using System;
using System.Reflection;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);
        string GetDocumentation(Type type);
    }
}