using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BaseOpenTK
{
    internal class Triangle2D : BasePourObjets
    {
        // ****************************************************************
        // Attributs
        // Code à venir ...

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
        }

        // ****************************************************************
        // Methodes Classe Parent
        override public void update()
        {
            // Code à venir ...
        }
        public void dessiner()
        {
            base.dessiner(PrimitiveType.Triangles);
        }
    }
}
