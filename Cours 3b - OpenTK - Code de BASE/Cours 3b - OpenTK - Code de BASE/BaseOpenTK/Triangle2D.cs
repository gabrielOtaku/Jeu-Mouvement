using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System.Drawing.Imaging;

namespace BaseOpenTK
{
    internal class Triangle2D : BasePourObjets
    {
        float theta;             // Angle de rotation 
        float incrementRotation; // Vitesse de rotation 

        public Triangle2D(Vector2 pointA, Vector2 pointB, Vector2 pointC)
            : base("./images/DoritosBMP.bmp", pointA, pointB, pointC) // Assurez-vous d'avoir l'image ou changez le chemin
        {
            theta = 0.0f;
            incrementRotation = 0.5f; 
        }

        override public void update()
        {
            theta += incrementRotation;
            if (theta >= 360.0f) theta = 0.0f;
            else if (theta <= 0.0f) theta = 360.0f;
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

        public void inverserRotation(Key touche)
        {
            if ((touche == Key.Right && incrementRotation > 0)
             || (touche == Key.Left && incrementRotation < 0))
            {
                incrementRotation *= -1.0f;
            }
        }
    }
}