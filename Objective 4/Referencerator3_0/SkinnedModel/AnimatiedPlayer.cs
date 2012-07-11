using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkinnedModel
{
    public class AnimatiedPlayer
    {
        public Matrix[] boneTransforms;
        public Matrix[] worldTransforms;
        public Matrix[] skinTransforms;

        SkinningDataStorage skinningDataValue;
        //Construct an updated model with the movement calculated 
        public AnimatiedPlayer(SkinningDataStorage skinningData)
        {
            if (skinningData == null)
                throw new ArgumentNullException("skinningData");

            skinningDataValue = skinningData;

            boneTransforms = new Matrix[skinningData.BindPose.Count];
            worldTransforms = new Matrix[skinningData.BindPose.Count];
            skinTransforms = new Matrix[skinningData.BindPose.Count];
        }

        //Refrest worldTransforms data
        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];
                worldTransforms[bone] = boneTransforms[bone] * worldTransforms[parentBone];
            }
        }

        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] * worldTransforms[bone];
            }
        }

        //get current bone transform matrices, relative to their parent bones. 
        public Matrix[] getBoneTransforms()
        {
            return boneTransforms;
        }

        public Matrix[] getWorldTransforms()
        {
            return worldTransforms;
        }
        //Get current bone transform matrices, relative to their skinning bind pose. 
        public Matrix[] getSkinTransforms()
        {
            return skinTransforms;
        }

    }
}
