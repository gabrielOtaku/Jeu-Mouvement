using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace BaseOpenTK
{
    internal class GestionJeu
    {
        // ****************************************************************
        // Attributs
        GameWindow window;
        Triangle2D triangle;
        Carre2D caisseDeBois;

        // ****************************************************************
        // Constructeur et Initialisation
        public GestionJeu(GameWindow window)
        {
            this.window = window;
            start();
        }

        private void start()
        {
            double nbrImagesParSecondes = 60.0f;
            double dureeAffichageChaqueImage = 1.0f / nbrImagesParSecondes;

            window.Load += chargement;
            window.Resize += redimensionner;
            window.UpdateFrame += update;
            window.RenderFrame += rendu;

            window.Run(dureeAffichageChaqueImage);
        }
        private void chargement(object sender, EventArgs arg)
        {
            GL.ClearColor(0.75f, 0.75f, 0.75f, 1.0f);
            GL.Enable(EnableCap.Texture2D);

            Vector2 pointA = new Vector2(-0.4f, 0.0f);
            Vector2 pointB = new Vector2(0.0f, 0.0f);
            Vector2 pointC = new Vector2(-0.2f, 0.6f);
            triangle = new Triangle2D(pointA, pointB, pointC);

            pointA = new Vector2(0.2f, -0.7f);
            pointB = new Vector2(0.46f, -0.7f);
            pointC = new Vector2(0.46f, -0.2f);
            Vector2 pointD = new Vector2(0.2f, -0.2f);
            caisseDeBois = new Carre2D(pointA, pointB, pointC, pointD);
        }

        // ****************************************************************
        // Gestion Affichage

        #region GestionAffichage

        private void redimensionner(object sender, EventArgs arg)
        {
            GL.Viewport(0, 0, window.Width, window.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-300.0, 300.0, -150.0, 150.0, -1.0, 1.0);
            GL.MatrixMode(MatrixMode.Modelview);
        }
        private void update(object sender, FrameEventArgs arg)
        {
            caisseDeBois.update();
        }
        private void rendu(object sender, FrameEventArgs arg)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            triangle.dessiner();
            caisseDeBois.dessiner();
            window.SwapBuffers();
        }
        #endregion
    }
}
