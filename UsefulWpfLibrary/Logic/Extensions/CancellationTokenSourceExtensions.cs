using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UsefulWpfLibrary.Logic.Extensions
{
    public static class CancellationTokenSourceExtensions
    {
        public static (CancellationTokenSource tokenSource, CancellationToken token)
            Link(this CancellationTokenSource tokenSource,
                IEnumerable<CancellationToken>? tokens = default,
                IEnumerable<CancellationTokenSource>? tokenSources = default)
        {
            CancellationToken token = tokenSource.Token;
            return token.Link(tokens, tokenSources);
        }

        public static (CancellationTokenSource tokenSource, CancellationToken token)
            Link(this CancellationToken token,
                IEnumerable<CancellationToken>? tokens = default,
                IEnumerable<CancellationTokenSource>? tokenSources = default)
        {
            CancellationToken[]? buffTokens = tokens?.ToArray();
            CancellationTokenSource[]? buffTokenSources = tokenSources?.ToArray();
            int length = (buffTokens?.Length ?? 0) + (buffTokenSources?.Length) ?? 0;
            var list = new List<CancellationToken>(length + 1);
            if (buffTokens != null) list.AddRange(buffTokens);
            if (buffTokenSources != null)
                list.AddRange(buffTokenSources.Select(source => source.Token));
            list.Add(token);
            var tokenSource =
#pragma warning disable DF0010 // Marks undisposed local variables.
                CancellationTokenSource.CreateLinkedTokenSource(list.ToArray());
#pragma warning restore DF0010 // Marks undisposed local variables.
            return (tokenSource, tokenSource.Token);
        }
    }
}
