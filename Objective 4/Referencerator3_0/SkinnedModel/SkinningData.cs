using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SkinnedModel
{
    public class SkinningDataStorage
    {
        public SkinningDataStorage(List<Matrix> bindPose, List<Matrix> inverseBindPose, List<int> skeletonHierarchy, Dictionary<string, int> Indices)
        {
            BindPose = bindPose;
            InverseBindPose = inverseBindPose;
            SkeletonHierarchy = skeletonHierarchy;
            BoneIndices = Indices;
        }

        private SkinningDataStorage()
        {
        }

        [ContentSerializer]
        public List<Matrix> BindPose { get; private set; }

        [ContentSerializer]
        public List<Matrix> InverseBindPose { get; private set; }

        [ContentSerializer]
        public List<int> SkeletonHierarchy { get; private set; }

        [ContentSerializer]
        public Dictionary<string, int> BoneIndices { get; private set; }
    }
}
