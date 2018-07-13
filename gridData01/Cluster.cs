using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Accord;

using Accord.IO;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math;
using Accord.Statistics.Kernels;
using Accord.MachineLearning;


namespace gridData01
{
    class Cluster
    {
        public void test2(double[][] observations,int nClusters)
        {
            // Create a new K-Means algorithm with 3 clusters 
            KMeans kmeans = new KMeans(nClusters);

            // Compute the algorithm, retrieving an integer array
            //  containing the labels for each of the observations
            KMeansClusterCollection clusters = kmeans.Learn(observations);

            // As a result, the first two observations should belong to the
            //  same cluster (thus having the same label). The same should
            //  happen to the next four observations and to the last three.
            int[] labels = clusters.Decide(observations);
        }
        public void test1()
        {
            Accord.Math.Random.Generator.Seed = 0;

            // Declare some observations
            double[][] observations =
            {
    new double[] { -5, -2, -1 },
    new double[] { -5, -5, -6 },
    new double[] {  2,  1,  1 },
    new double[] {  1,  1,  2 },
    new double[] {  1,  2,  2 },
    new double[] {  3,  1,  2 },
    new double[] { 11,  5,  4 },
    new double[] { 15,  5,  6 },
    new double[] { 10,  5,  6 },
};

            // Create a new K-Means algorithm with 3 clusters 
            KMeans kmeans = new KMeans(3);

            // Compute the algorithm, retrieving an integer array
            //  containing the labels for each of the observations
            KMeansClusterCollection clusters = kmeans.Learn(observations);

            // As a result, the first two observations should belong to the
            //  same cluster (thus having the same label). The same should
            //  happen to the next four observations and to the last three.
            int[] labels = clusters.Decide(observations);
        }
    }
}
