using OpenTK;
using OpenTK.Graphics;

namespace BaseOpenTK
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ****************************************************************
            // Attributs
            int largeurFenetre = 600;
            int hauteurFenetre = 300;
            string titreFenetre = "Jeu-Mouvements";

            // ****************************************************************
            // Code
            DisplayDevice moniteur = DisplayDevice.Default;
            if (DisplayDevice.Default == DisplayDevice.GetDisplay(DisplayIndex.Second))
            {
                moniteur = DisplayDevice.GetDisplay(DisplayIndex.First);
            }
            GameWindow window = new GameWindow(largeurFenetre, hauteurFenetre, GraphicsMode.Default, titreFenetre, GameWindowFlags.Default, moniteur);
            GestionJeu fenetrePrincipale = new GestionJeu(window);
        }
    }
}
