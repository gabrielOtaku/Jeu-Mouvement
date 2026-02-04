using System.Runtime.CompilerServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BaseOpenTK
{
    internal class Carre2D : BasePourObjets
    {
        // ****************************************************************
        // Attributs
        float deplacementVertical;
        float incrementVertical;

        // ****************************************************************
        // Constructeur et Initialisation
        public Carre2D(float, float)
        {
            this.deplacementVertical = 0.0f;
            this.incrementVertical = -1.5f;
        }

        public Carre2D(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD)
            : base("./images/CaisseBoisBMP.bmp", pointA, pointB, pointC, pointD)
        {
            pointA.X = 40.0f;
            pointA.Y = -40.0f;

            pointB.X = 100.0f;
            pointB.Y = -40.0f;

            pointC.X = 100.0f;
            pointC.Y = 20.0f;

            pointD.X = 40.0f;
            pointD.Y = 20.0f;
        }

        // ****************************************************************
        // Methodes Classe Parent
        override public void update()
        {
            // Code à venir ...
        }
        public void dessiner()
        {
            base.dessiner(PrimitiveType.Quads);
        }
    }
}
