using Sitecore.Diagnostics;
using Sitecore.Pipelines.RenderField;
using System;
using System.Text;

namespace Sitecore.Support.Pipelines.RenderField
{
  /// <summary>
  /// Sets anchors position in RTE links.
  /// </summary>
  public class SetAnchorsPositionInLinks
  {
    /// <summary>
    /// The pattern for finding links.
    /// </summary>
    protected static readonly string pattern = "href=\"";

    /// <summary>Sets the correct position of anchors in RTE links.</summary>
    /// <param name="args">The arguments.</param>
    /// <contract>
    ///   <requires name="args" condition="none" />
    /// </contract>
    public void Process(RenderFieldArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      if (args.FieldTypeKey == "rich text")
      {
        args.Result.FirstPart = this.CheckLinks(args.Result.FirstPart);
        args.Result.LastPart = this.CheckLinks(args.Result.LastPart);
      }
    }

    /// <summary>
    /// Checks if links exist in the RTE field and modifies them.
    /// </summary>
    /// <param name="text">The text.</param>
    protected string CheckLinks(string text)
    {
      if (!text.Contains(SetAnchorsPositionInLinks.pattern))
      {
        return text;
      }
      int num = 0;
      StringBuilder stringBuilder = new StringBuilder();
      int num2;
      while ((num2 = text.IndexOf(SetAnchorsPositionInLinks.pattern, num, StringComparison.Ordinal)) >= 0)
      {
        int length = num2 - num + SetAnchorsPositionInLinks.pattern.Length;
        stringBuilder.Append(text.Substring(num, length));
        int length2 = text.IndexOf("\"", num2 + SetAnchorsPositionInLinks.pattern.Length + 1, StringComparison.Ordinal) - num2 - SetAnchorsPositionInLinks.pattern.Length;
        string text2 = text.Substring(num2 + SetAnchorsPositionInLinks.pattern.Length, length2);
        text2 = this.MoveAnchor(text2);
        stringBuilder.Append(text2);
        num = num2 + SetAnchorsPositionInLinks.pattern.Length + text2.Length;
      }
      stringBuilder.Append(text.Substring(num));
      return stringBuilder.ToString();
    }

    /// <summary>
    /// Changes the position of the anchor in the link if needed.
    /// </summary>
    /// <param name="link">The link.</param>
    protected string MoveAnchor(string link)
    {
      int num = link.IndexOf("#");
      if (num < 0)
      {
        return link;
      }
      int num2 = link.IndexOf("?", num + 1);
      if (num2 < 0)
      {
        num2 = link.IndexOf("&", num + 1);
      }
      if (num2 < 0)
      {
        return link;
      }
      int length = num2 - num;
      return link.Substring(0, num) + link.Substring(num2) + link.Substring(num, length);
    }
  }
}

