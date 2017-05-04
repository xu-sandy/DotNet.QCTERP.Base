using System;
using System.Reflection;

namespace Qct.ERPMoblie.Api.Retailing.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}