using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using SkinnedModel;
// TODO: replace these with the processor input and output types.


namespace SkinningModelExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor]
    public class SkinnedModelProcessor : ModelProcessor
    {
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {

            ValidateMesh(input, context, null);
            BoneContent skeleton = MeshHelper.FindSkeleton(input);
            if (skeleton == null)
                throw new InvalidContentException("Input skeleton not found.");
            //Bakes everything
            FlattenTransforms(input, skeleton);
            //Read bind pse and skeleton hierarchy data.
            IList<BoneContent> bones = MeshHelper.FlattenSkeleton(skeleton);

            if (bones.Count > SkinnedEffect.MaxBones)
            {
                throw new InvalidContentException(string.Format("Skeleton has {0} bones, but the max is {1}.", bones.Count, SkinnedEffect.MaxBones));
            }
            List<Matrix> bindPose = new List<Matrix>();
            List<Matrix> inverseBindPose = new List<Matrix>();
            List<int> skeletonHierarchy = new List<int>();
            Dictionary<string, int> boneIndices = new Dictionary<string, int>();

            foreach (BoneContent bone in bones)
            {
                bindPose.Add(bone.Transform);
                inverseBindPose.Add(Matrix.Invert(bone.AbsoluteTransform));
                skeletonHierarchy.Add(bones.IndexOf(bone.Parent as BoneContent));
                boneIndices.Add(bone.Name, boneIndices.Count);
            }
            ModelContent model = base.Process(input, context);
            model.Tag = new SkinningDataStorage(bindPose, inverseBindPose, skeletonHierarchy, boneIndices);
            return model;

        }

        //Make Sure mesh contains the kind of data we know how to animate
        static void ValidateMesh(NodeContent node, ContentProcessorContext context, string parentBoneName)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                if (parentBoneName != null)
                {
                    context.Logger.LogWarning(null, null, "Mesh {0} is a child of bone {1}", mesh.Name, parentBoneName);
                }
                if (!MeshHasSkinning(mesh))
                {
                    context.Logger.LogWarning(null, null, "Mesh {0} has no skinning information, so it has been deleted.", mesh.Name);
                    mesh.Parent.Children.Remove(mesh);
                    return;
                }
            }
            else if (node is BoneContent)
            {
                //if this is a bone, remember that we are not looking inside it. 
                parentBoneName = node.Name;
            }
            //Recurse (iterating over a copy of the child collection
            //because validating children may delete some of them)
            foreach (NodeContent child in new List<NodeContent>(node.Children))
                ValidateMesh(child, context, parentBoneName);
        }

        static bool MeshHasSkinning(MeshContent mesh)
        {
            foreach (GeometryContent geometry in mesh.Geometry)
            {
                if (!geometry.Vertices.Channels.Contains(VertexChannelNames.Weights()))
                    return false;
            }
            return true;
        }

        static void FlattenTransforms(NodeContent node, BoneContent skeleton)
        {
            foreach (NodeContent child in node.Children)
            {
                if (child == skeleton)
                    continue;

                //Bake Local Transform into actual geometry.
                MeshHelper.TransformScene(child, child.Transform);
                //now we can set the local coordinate system back to identity. 
                child.Transform = Matrix.Identity;

                //Recures
                FlattenTransforms(child, skeleton);


            }
        }
        [DefaultValue(MaterialProcessorDefaultEffect.SkinnedEffect)]
        public override MaterialProcessorDefaultEffect DefaultEffect
        {
            get
            {
                return MaterialProcessorDefaultEffect.SkinnedEffect;
            }
            set
            {

            }
        }
    }
}