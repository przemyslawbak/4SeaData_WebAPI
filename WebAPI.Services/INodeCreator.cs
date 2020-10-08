using HtmlAgilityPack;
using System.Runtime.CompilerServices;

namespace WebAPI.Services
{
    public interface INodeCreator
    {
        string CreatePrepareAndVerifyRowNodeString(string html_document_2, [CallerMemberName] string name = null);
        HtmlNodeCollection CreateNodeCollection(string html_document_1, [CallerMemberName] string name = null);
        string CreatePrepareAndVerifyRowNodeOuterHtml(string html_document_1, [CallerMemberName] string name = null);
    }
}