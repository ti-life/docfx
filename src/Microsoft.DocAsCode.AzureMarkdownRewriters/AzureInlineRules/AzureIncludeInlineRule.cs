﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.AzureMarkdownRewriters
{
    using System.IO;
    using System.Text.RegularExpressions;

    using Microsoft.DocAsCode.Common;
    using Microsoft.DocAsCode.MarkdownLite;
    using Microsoft.DocAsCode.Utility;

    public class AzureIncludeInlineRule : IMarkdownRule
    {
        public virtual string Name => "AZURE.INCLUDE.INLINE";

        private static readonly Regex _azureIncludeRegex = new Regex(@"^\[AZURE.INCLUDE\s*\[((?:\[[^\]]*\]|[^\[\]]|\](?=[^\[]*\]))*)\]\(\s*<?([^)]*?)>?(?:\s+(['""])([\s\S]*?)\3)?\s*\)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public virtual Regex AzureIncludeRegex => _azureIncludeRegex;

        public IMarkdownToken TryMatch(IMarkdownParser engine, ref string source)
        {
            var match = AzureIncludeRegex.Match(source);
            if (match.Length == 0)
            {
                return null;
            }
            source = source.Substring(match.Length);

            // [!azure.include[title](path "optionalTitle")]
            // 1. Get include file path 
            var path = match.Groups[2].Value;

            // 2. Get title
            var value = match.Groups[1].Value;
            var title = match.Groups[4].Value;

            if (!PathUtility.IsRelativePath(path))
            {
                Logger.LogWarning($"Azure inline include path {path} is not a relative path, can't expand it");
                return new MarkdownTextToken(this, engine.Context, match.Value, match.Value);
            }

            // 3. Apply inline rules to the included content
            object currentFilePath;
            if (!engine.Context.Variables.TryGetValue("path", out currentFilePath))
            {
                Logger.LogWarning($"Can't get path for the file that ref azure inline include file, return MarkdownTextToken. Raw: {match.Value}");
                return new MarkdownTextToken(this, engine.Context, match.Value, match.Value);
            }

            var includeFilePath = PathUtility.NormalizePath(Path.Combine(Path.GetDirectoryName(currentFilePath.ToString()), path));
            if (!File.Exists(includeFilePath))
            {
                Logger.LogWarning($"Can't get include file path {includeFilePath} in the file {currentFilePath}, return MarkdownTextToken. Raw: {match.Value}");
                return new MarkdownTextToken(this, engine.Context, match.Value, match.Value);
            }

            return new TwoPhaseBlockToken(this, engine.Context, match.Value, (p, t) =>
            {
                var inlineTokens = p.Tokenize(MarkdownEngine.Normalize(File.ReadAllText(includeFilePath)));
                return new AzureIncludeInlineToken(t.Rule, t.Context, path, value, title, inlineTokens, match.Groups[0].Value, match.Value);
            });
        }
    }
}
