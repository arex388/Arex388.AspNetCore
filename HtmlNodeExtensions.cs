using System.Collections.Generic;
using System.Linq;

namespace HtmlAgilityPack {
	public static class HtmlNodeExtensions {
		public static IList<HtmlNode> SelectNodesAsList(
			this HtmlNode node,
			string xpath) {
			return node.SelectNodes(xpath)?.ToList() ?? new List<HtmlNode>(0);
		}

		public static void TrimWhitespace(
			this HtmlNode document) {
			var textNodes = document.SelectNodesAsList("//text()").Where(
				n => string.IsNullOrWhiteSpace(n.InnerHtml));

			foreach (var textNode in textNodes) {
				textNode.Remove();
			}

			var commentNodes = document.SelectNodesAsList("//comment()").Where(
				n => n.InnerHtml != "<!DOCTYPE html>");

			foreach (var commentNode in commentNodes) {
				commentNode.Remove();
			}
		}
	}
}