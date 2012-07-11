using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SkinnedModel
{
    public class SkinnedSphere
    {
        public string BoneName;
        public float Radius;
        [ContentSerializer(Optional = true)]
        public float Offset;
    }
}
