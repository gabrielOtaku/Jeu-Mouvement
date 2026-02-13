using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BaseOpenTK
{
    internal class Carre2D : BasePourObjets
    {
        float deplacementVertical;
        float incrementVertical;

        public Carre2D(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD)
            : base("./images/CaisseBoisBMP.bmp", pointA, pointB, pointC, pointD)
        {
            this.deplacementVertical = 0.0f;
            this.incrementVertical = 1.5f; 
        }

        override public void update()
        {
     
            if (deplacementVertical + incrementVertical >= 150.0f - listePoints[3].Y
             || deplacementVertical + incrementVertical <= -150.0f - listePoints[0].Y)
            {
                incrementVertical *= -1.0f; 
            }
            deplacementVertical += incrementVertical; 
        }

        public void dessiner()
        {
            GL.PushMatrix();
            GL.Translate(0.0f, deplacementVertical, 0.0f); 
            base.dessiner(PrimitiveType.Quads);
            GL.PopMatrix();
        }
    }
}