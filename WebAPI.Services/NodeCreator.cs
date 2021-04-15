using HtmlAgilityPack;
using System.Runtime.CompilerServices;

namespace WebAPI.Services
{
    public class NodeCreator : INodeCreator
    {
        private readonly IStringParser _stringParser;

        public NodeCreator(IStringParser stringParser)
        {
            _stringParser = stringParser;
        }

        public string CreatePrepareAndVerifyRowNodeOuterHtml(string html, [CallerMemberName] string callerName = "")
        {
            HtmlNode row = GetDocumentAndSelectSingleNode(html, callerName);

            if (row != null)
            {
                return row.OuterHtml;
            }

            return null;
        }

        public string CreatePrepareAndVerifyRowNodeString(string html, [CallerMemberName] string callerName = "")
        {
            HtmlNode row = GetDocumentAndSelectSingleNode(html, callerName);

            if (row != null)
            {
                if (_stringParser.IsTableRowCorrect(row.OuterHtml))
                {
                    return row.InnerText;
                }
            }

            return null;
        }

        public HtmlNodeCollection CreateNodeCollection(string html, [CallerMemberName] string callerName = "")
        {
            HtmlDocument doc = CreateNodeDocument(html);

            return doc.DocumentNode.SelectNodes(_stringParser.GetXpath(callerName));
        }

        private HtmlNode GetDocumentAndSelectSingleNode(string html, string callerName)
        {
            HtmlDocument doc = CreateNodeDocument(html);

            return doc.DocumentNode.SelectSingleNode(_stringParser.GetXpath(callerName));
        }

        private HtmlDocument CreateNodeDocument(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }
    }
}
