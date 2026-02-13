using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BaseOpenTK
{
    internal class Triangle2D : BasePourObjets
    {
        // ****************************************************************
        // Attributs
        float theta;

        // ****************************************************************
        // Constructeur et Initialisation
        public Triangle2D(Vector2 pointA, Vector2 pointB, Vector2 pointC)
            : base("./images/DoritosBMP.bmp", pointA, pointB, pointC)
        {
            pointA.X = -100.0f;
            pointA.Y = 0.0f;

            pointB.X = 0.0f;
            pointB.Y = 0.0f;

            pointC.X = -50.0f;
            pointC.Y = 85.0f;

            this.theta = 0.0f;
        }

        // ****************************************************************
        // Methodes Classe Parent
        override public void update()
        {
            theta += 0.5f;
            if (theta > 360.0f)
            {
                theta = 0.0f;

            }
        }

        public void dessiner()
        {
            GL.PushMatrix();
            GL.Translate(listePoints[0].X, 0.0f, 0.0f);
            GL.Rotate(theta, 0.0, 0.0, 1.0);
            GL.Translate(-listePoints[0].X, 0.0f, 0.0f);
            base.dessiner(PrimitiveType.Triangles);
            GL.PopMatrix();
        }
    }
}
