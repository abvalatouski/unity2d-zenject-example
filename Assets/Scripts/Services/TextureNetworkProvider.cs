using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Networking;

namespace Banner
{
    public class TextureNetworkProvider : IProvider<Texture>
    {
        private IEnumerator<string> urls;

        public TextureNetworkProvider(IEnumerable<string> urls)
        {
            this.urls = urls.GetEnumerator();
        }

        public async Task<Texture> ProvideAsync(CancellationToken cancellationToken)
        {
            if (urls.MoveNext())
            {
                return await RequestTexture(urls.Current, cancellationToken);
            }
            
            urls.Reset();
            if (!urls.MoveNext())
            {
                throw new InvalidOperationException("Expected at least one URL.");
            }

            return await RequestTexture(urls.Current, cancellationToken);
        }

        private async Task<Texture> RequestTexture(string url, CancellationToken cancellationToken)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

            UnityWebRequestAsyncOperation operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Yield();
            }

            return DownloadHandlerTexture.GetContent(request);
        }
    }
}
