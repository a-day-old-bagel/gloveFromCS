using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace DllTester {
    class Program {      
        static void Main(string[] args) {
            Console.WriteLine(test(10));
            Learner learner = new DllTester.Learner();
            learner.Run();
        }

        [DllImport("learning64.dll")]
        static extern int test(int foo);
    }

    class Learner {
        private CooccurArgs cooccurArgs;
        private GloveArgs gloveArgs;
        private ShuffleArgs shuffleArgs;
        private VocabCountArgs vocabCountArgs;

        public Learner() {
            cooccurArgs = new CooccurArgs();
            FillCooccurArgs(ref cooccurArgs);
            gloveArgs = new GloveArgs();
            FillGloveArgs(ref gloveArgs);
            shuffleArgs = new ShuffleArgs();
            FillShuffleArgs(ref shuffleArgs);
            vocabCountArgs = new VocabCountArgs();
            FillVocabCountArgs(ref vocabCountArgs);
        }

        public void Run() {
            string corpus = "corpus.txt";
            string vocab = "vocab.txt";
            string cooccurence = "cooccurence.bin";
            string cooccurenceShuf = "cooccurence.shuf.bin";
            string gradsq = "gradsq";
            string save = "vectors";

            vocabCount(ref vocabCountArgs, corpus, vocab);
            cooccur(ref cooccurArgs, corpus, vocab, cooccurence);
            shuffle(ref shuffleArgs, cooccurence, cooccurenceShuf);
            glove(ref gloveArgs, cooccurenceShuf, vocab, save, gradsq);
        }

        public void FillCooccurArgs(ref CooccurArgs args) {
            args.verbose = 1;
            args.symmetric = 1;
            args.windowSize = 15;
            args.memory = 4;
            args.maxProduct = -1;
            args.overflowLength = -1;
            args.overflowFile = "overflow";
            args.mode = 0;
        }
        [DllImport("learning64.dll")]
        static extern int cooccur(ref CooccurArgs args, string corpusInFile, string vocabInFile, string cooccurOutFile);

        public void FillGloveArgs(ref GloveArgs args) {
            args.verbose = 1;
            args.vectorSize = 50;
            args.threads = 8;
            args.iter = 15;
            args.eta = 0.05f;
            args.alpha = 0.75f;
            args.xMax = 100.0f;
            args.binary = 0;
            args.model = 2;
            args.saveGradsq = 0;
            args.checkpointEvery = 0;
            args.mode = 0;
        }
        [DllImport("learning64.dll")]
        static extern int glove(ref GloveArgs args, string shufCooccurInFile, string vocabInFile, string gloveOutFile, string gradsqOutFile);

        public void FillShuffleArgs(ref ShuffleArgs args) {
            args.verbose = 1;
            args.memory = 4.0f;
            args.arraySize = -1;
            args.tempFile = "temp_shuffle";
            args.mode = 0;
        }
        [DllImport("learning64.dll")]
        static extern int shuffle(ref ShuffleArgs args, string cooccurInFile, string shufCooccurOutFile);

        public void FillVocabCountArgs(ref VocabCountArgs args) {
            args.verbose = 1;
            args.maxVocab = -1;
            args.minCount = 1;
            args.mode = 0;
        }
        [DllImport("learning64.dll")]
        static extern int vocabCount(ref VocabCountArgs args, string corpusInFile, string vocabOutFile);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CooccurArgs {
        public int verbose, symmetric, windowSize;
        public float memory;
        public int maxProduct, overflowLength;
        public string overflowFile;
        public int mode;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct GloveArgs {
        public int verbose, vectorSize, threads, iter;
        public float eta, alpha, xMax;
        public int binary, model;
        public int saveGradsq, checkpointEvery, mode;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ShuffleArgs {
        public int verbose;
        public float memory;
        public int arraySize;
        public string tempFile;
        public int mode;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct VocabCountArgs {
        public int verbose, maxVocab, minCount, mode;
    }
}
