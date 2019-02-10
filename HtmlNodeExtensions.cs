using System.Collections.Generic;
using System.Linq;

namespace HtmlAgilityPack {
	public static class HtmlNodeExtensions {
		public static IList<HtmlNode> SelectNodesAsList(
			this HtmlNode node,
			string xpath) {
			return node.SelectNodes(xpath)?.ToList() ?? new List<HtmlNode>();
		}
	}
}