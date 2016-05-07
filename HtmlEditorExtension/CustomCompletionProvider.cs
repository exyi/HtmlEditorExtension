using Microsoft.Html.Editor.Completion.Def;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Html.Editor.Completion;
using Microsoft.VisualStudio.Utilities;
using Microsoft.Html.Core.Artifacts;
using Microsoft.Web.Core.Text;
using System.Text.RegularExpressions;

namespace HtmlEditorExtension
{
    //[Export(typeof(IHtmlCompletionListProvider))]
    [HtmlCompletionProvider(CompletionTypes.GroupValues, "*", "*")]
    [ContentType("htmlx")]
    class CustomCompletionProvider : IHtmlCompletionListProvider
    {
        public string CompletionType => CompletionTypes.GroupValues;

        public IList<HtmlCompletion> GetEntries(HtmlCompletionContext context)
        {
            return new[] {
                new HtmlCompletion("YES", "YES", "yes", null, "", context.Session),
                new HtmlCompletion("NO", "NO", "no", null, "", context.Session),
            };
        }
    }

    [Export(typeof(IArtifactProcessorProvider))]
    [ContentType("htmlx")]
    public class MyArtifactProcessorProvider : IArtifactProcessorProvider
    {
        public IArtifactProcessor GetProcessor()
        {
            return new MyArtifactProcessor();
        }
    }

    public class MyArtifactProcessor : IArtifactProcessor
    {
        public bool IsReady => true;

        public string LeftCommentSeparator => "";

        public string LeftSeparator => "☃";

        public string RightCommentSeparator => "";

        public string RightSeparator => "☃";

        public void GetArtifacts(ITextProvider text, ArtifactCollection artifactCollection)
        {
            foreach (var a in FindArtifacts(text.GetText(new TextRange(0, text.Length))))
            {
                artifactCollection.Add(a);
            }
        }

        Regex regex = new Regex("☃.*?☃", RegexOptions.Compiled | RegexOptions.Singleline);
        IEnumerable<MyArtifact> FindArtifacts(string text)
        {
            return regex.Matches(text).OfType<Match>()
                .Select(m => new MyArtifact(m.Index, m.Length));
        }
    }

    class MyArtifact: Artifact
    {
        public MyArtifact(int start, int len)
            :base(ArtifactTreatAs.Code, new TextRange(start, len), 1, 1, "Default", true)
        {

        }
    }
}
