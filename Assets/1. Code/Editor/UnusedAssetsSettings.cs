using System.Collections.Generic;
using System.IO;
using Neuston.UnusedAssetFinder;

namespace CleanRoom.Editor
{
    public class UnusedAssetsSettings : IUnusedAssetConfiguration
    {
        public void FilterAssetPaths(List<string> assetPaths)
        {
            // Exclude third party assets:
            assetPaths.RemoveAll(path => path.StartsWith(Path.Join("Assets", "6. Libraries")));
        }
    }
}