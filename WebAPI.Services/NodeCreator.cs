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

        public string CreatePrepareAndVerifyRowNodeOuterHtml(string html, [CallerMemberName] string callerName = "")//todo: CallerMemberName
        {
            HtmlNode row = CreateDocumentAndRowNode(html, callerName);

            if (row != null)
            {
                return row.OuterHtml;
            }

            return null;
        }

        public string CreatePrepareAndVerifyRowNodeString(string html, [CallerMemberName] string callerName = "")//todo: CallerMemberName
        {
            HtmlNode row = CreateDocumentAndRowNode(html, callerName);

            if (row != null)
            {
                if (_stringParser.IsTableRowCorrect(row.OuterHtml))
                {
                    return _stringParser.SplitTableRow(row.OuterHtml);
                }
            }

            return null;
        }

        public HtmlNodeCollection CreateNodeCollection(string html_document_1, [CallerMemberName] string callerName = "")//todo: CallerMemberName
        {
            HtmlDocument doc = CreateNodeDocument(html_document_1);

            return doc.DocumentNode.SelectNodes(_stringParser.GetXpath(callerName));
        }
        private HtmlDocument CreateNodeDocument(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }

        private HtmlNode CreateDocumentAndRowNode(string html, string callerName)
        {
            HtmlDocument doc = CreateNodeDocument(html);

            return doc.DocumentNode.SelectSingleNode(_stringParser.GetXpath(callerName));
        }
    }
}
