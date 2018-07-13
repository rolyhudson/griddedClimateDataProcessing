using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using System.Drawing;

namespace gridData01
{
    class TriVariateLogScale
    {

        //    
        //        C
        //       /\
        //      /  \
        //     A----B 
        //unit equilatral triangle to represent the range

        public Point3d A = new Point3d(1, 1, 0);
        public Point3d B = new Point3d(2, 1, 0);
        public Point3d C = new Point3d(1.5, 1.866, 0);

        double areaTri;
        //max values at vertices
        //vertex data linear
        public double[] aScale = new double[2];
        public double[] bScale = new double[2];
        public double[] cScale = new double[2];
        // log scales 
        double[] aLogScale = new double[2];
        double[] bLogScale = new double[2];
        double[] cLogScale = new double[2];
        //vertex colors
        Vector3d aColor;
        Vector3d bColor;
        Vector3d cColor;

        double aRange;
        double bRange;
        double cRange;

        bool invertAScale;
        bool invertBScale;
        bool invertCScale;

        public TriVariateLogScale()
        {
            this.areaTri = areaTriangle(this.A, this.B, this.C);
        }
        public void setVertces(Point3d a,Point3d b,Point3d c)
        {
            this.A = a;
            this.B = b;
            this.C = c;
            this.areaTri = areaTriangle(this.A, this.B, this.C);
        }
        public double[] getABCParamsFromPoint(Point3d p)
        {
            double[] abc = new double[3];
            double u = areaTriangle(p, this.B, this.C) / areaTri;
            double v = areaTriangle(p, this.C, this.A) / areaTri;
            double w = 1 - u - v;// areaTriangle(p, this.A, this.B) / areaTri;
            if (this.invertAScale) u = 1 - u;
            if (this.invertBScale) v = 1 - v;
            if (this.invertCScale) w = 1 - w;

            abc[0] = Math.Pow(10, u * this.aRange+this.aScale[0]);
            abc[1] = Math.Pow(10, v * this.bRange + this.bScale[0]);
            abc[2] = Math.Pow(10, w * this.cRange + this.cScale[0]);

            return abc;
        }
        public Point3d getPointFromABCParams(double a, double b,double c)
        {
            double t1 = Math.Log10(a);
            double t2 = Math.Log10(b);
            double u = (Math.Log10(a) - aScale[0]) / aRange;
            double v = (Math.Log10(b) - bScale[0]) / bRange;
            if (this.invertAScale) u = 1 - u;
            if (this.invertBScale) v = 1 - v;
            double w = 1 - u - v;// (Math.Log10(c) - cScale[0]) / cRange;

            Point3d p = this.A * u + this.B * v + this.C * w;
            return p;
        }
        public void setColors(Color colorA, Color colorB, Color colorC)
        {
            this.aColor = colorToVector(colorA);
            this.bColor = colorToVector(colorB);
            this.cColor = colorToVector(colorC);
        }
        private Vector3d colorToVector(Color color)
        {
            Vector3d colorV = new Vector3d();
            colorV.X = color.R / 255.0;
            colorV.Y = color.G / 255.0;
            colorV.Z = color.B / 255.0;
            return colorV;
        }
        public Color getColor(double a,double b,double c)
        {
            Color newCol = new Color();

            double u = (Math.Log10(a) - aScale[0]) / aRange;
            double v = (Math.Log10(b) - bScale[0]) / bRange;
            if (this.invertAScale) u = 1 - u;
            if (this.invertBScale) v = 1 - v;
            double w = 1 - u - v; //(Math.Log10(c) - cScale[0]) / cRange;

            Vector3d newVecColor = combineColors(u, v, w);
            newCol = Color.FromArgb((int)(newVecColor.X * 255), (int)(newVecColor.Y * 255), (int)(newVecColor.Z * 255));
            return newCol;
        }
        public Vector3d getVectorColor(double a, double b, double c)
        {
            double u = (Math.Log10(a) - aScale[0]) / aRange;
            double v = (Math.Log10(b) - bScale[0]) / bRange;
            if (this.invertAScale) u = 1 - u;
            if (this.invertBScale) v = 1 - v;
            double w = 1 - u - v; //(Math.Log10(c) - cScale[0]) / cRange;

            return combineColors(u, v, w);
        }

        private Vector3d combineColors(double u,double v,double w)
        {
            Vector3d newAColor = this.aColor;
            Vector3d newBColor = this.bColor;
            Vector3d newCColor = this.cColor;
            newAColor = newAColor * u;
            newBColor = newBColor * v;
            newCColor = newCColor * w;
            return newAColor + newBColor + newCColor;
        }
        private double areaTriangle(Point3d a, Point3d b, Point3d c)
        {
            Vector3d v1 = new Vector3d(b - a);
            Vector3d v2 = new Vector3d(c - a);
            Vector3d cp = Vector3d.CrossProduct(v1, v2);
            double area = cp.Length / 2;
            return area;
        }
        public void setScales(double[] aRange,bool aInvert, double[] bRange, bool bInvert, double[] cRange,bool cInvert)
        {
            this.aLogScale = aRange;
            this.bLogScale = bRange;
            this.cLogScale = cRange;
            //set vertex data linear
            this.aScale = invLog10(aRange);
            this.bScale = invLog10(bRange);
            this.cScale = invLog10(cRange);

            this.aRange = (this.aScale[1] - this.aScale[0]);
            this.bRange = (this.bScale[1] - this.bScale[0]);
            this.cRange = (this.cScale[1] - this.cScale[0]);

            this.invertAScale = aInvert;
            this.invertBScale = bInvert;
            this.invertCScale = cInvert;

        }
        public double[] invLog10(double[] logValues)
        {
            double[] linearVals = {Math.Log10(logValues[0]), Math.Log10(logValues[1])};
            return linearVals;
        }
        //colouring with barycentric http://www.scratchapixel.com/lessons/3d-basic-rendering/ray-tracing-rendering-a-triangle/barycentric-coordinates
        //    // vertex position
        //    Vec3f triVertex[3] = { { -3, -3, 5 }, { 0, 3, 5 }, { 3, -3, 5 } };
        //    // vertex data
        //    Vec3f triColor[3] = { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } }; 
        //if (rayTriangleIntersect(...)) { 
        //    // compute pixel color
        //    // col = w*col0 + u*col1 + v*col2 where w = 1-u-v
        //    Vec3f PhitColor = u * triColor[0] + v * triColor[1] + (1 - u - v) * triColor[2];
    }

}
