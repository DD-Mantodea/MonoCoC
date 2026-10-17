using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace MonoCoC.Graphics.Shaders
{
    /// <summary>
    /// Runtime .fx compilation using the MonoGame content pipeline.
    /// This is the same approach used by NifRenderer's FxCompiler, adapted to
    /// compile against the running game's GraphicsDevice.
    /// </summary>
    public static class FxCompiler
    {
        private static readonly EffectProcessor Processor = new();

        public static Effect Compile(string file)
        {
            var identity = new ContentIdentity(file);

            var content = new EffectContent
            {
                Identity = identity
            };

            var compiled = Processor.Process(content, new FxCompilerContext());

            return new Effect(CoC.Instance.GraphicsDevice, compiled.GetEffectCode());
        }
    }

    internal sealed class FxCompilerContext : ContentProcessorContext
    {
        public override string BuildConfiguration => "Debug";

        public override string IntermediateDirectory => Path.GetTempPath();

        public override ContentBuildLogger Logger => throw new NotSupportedException();

        public override ContentIdentity SourceIdentity => throw new NotSupportedException();

        public override string OutputDirectory => Path.GetTempPath();

        public override string OutputFilename => "runtime.mgfxo";

        public override OpaqueDataDictionary Parameters => new();

        public override string ProjectDirectory => Path.GetTempPath();

        public override TargetPlatform TargetPlatform => TargetPlatform.Windows;

        public override GraphicsProfile TargetProfile => GraphicsProfile.HiDef;

        public override void AddDependency(string filename)
        {
        }

        public override void AddOutputFile(string filename)
        {
        }

        public override TOutput BuildAndLoadAsset<TInput, TOutput>(
            ExternalReference<TInput> sourceAsset,
            string processorName,
            OpaqueDataDictionary? processorParameters,
            string? importerName) => throw new NotSupportedException();

        public override TOutput BuildAndLoadAsset<TInput, TOutput>(
            ExternalReference<TInput> sourceAsset,
            IContentImporter importer,
            IContentProcessor processor) => throw new NotSupportedException();

        public override ExternalReference<TOutput> BuildAsset<TInput, TOutput>(
            ExternalReference<TInput> sourceAsset,
            string processorName,
            OpaqueDataDictionary? processorParameters,
            string? importerName,
            string? assetName) => throw new NotSupportedException();

        public override ExternalReference<TOutput> BuildAsset<TInput, TOutput>(
            ExternalReference<TInput> sourceAsset,
            IContentImporter importer,
            IContentProcessor processor,
            string? assetName = null) => throw new NotSupportedException();

        public override TOutput Convert<TInput, TOutput>(
            TInput input,
            string processorName,
            OpaqueDataDictionary processorParameters) => throw new NotSupportedException();

        public override TOutput Convert<TInput, TOutput>(
            TInput input,
            IContentProcessor processor) => throw new NotSupportedException();
    }
}
