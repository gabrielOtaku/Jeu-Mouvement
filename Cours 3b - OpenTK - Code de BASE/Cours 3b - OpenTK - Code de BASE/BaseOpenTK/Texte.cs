using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing; // Référence à System.Drawing nécessaire
using System;

namespace BaseOpenTK
{
    class Texte
    {
        Vector2 pointA, pointB, pointC, pointD;
        int largeurZoneTexte, hauteurZoneTexte;
        int textureID;

        string texte = "";
        Color couleurDeFond;
        SolidBrush pinceau;
        PointF position;
        Font policeSansSerif, policeSansSerifGras, policeAffichage;

        public Texte(Vector2 coinInfGauche, int largeur, int hauteur)
        {
            this.pointA = coinInfGauche;
            this.pointB = new Vector2(coinInfGauche.X + largeur, coinInfGauche.Y);
            this.pointC = new Vector2(coinInfGauche.X + largeur, coinInfGauche.Y + hauteur);
            this.pointD = new Vector2(coinInfGauche.X, coinInfGauche.Y + hauteur);

            this.largeurZoneTexte = largeur;
            this.hauteurZoneTexte = hauteur;

            couleurDeFond = Color.LightGray;
            pinceau = new SolidBrush(Color.Blue);
            position = new PointF(0.0f, 2.0f);
            policeSansSerif = new Font(FontFamily.GenericSansSerif, 11);
            policeAffichage = policeSansSerif;

            textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, largeur, hauteur, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        }

        public void setTexte(string txt)
        {
            texte = txt;
            chargerTexte();
        }

        private void chargerTexte()
        {
            Bitmap bmpTxt = new Bitmap(largeurZoneTexte, hauteurZoneTexte, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics graphique = Graphics.FromImage(bmpTxt);
            graphique.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            graphique.Clear(couleurDeFond);
            graphique.DrawString(texte, policeAffichage, pinceau, position);

            System.Drawing.Imaging.BitmapData dataTxt = bmpTxt.LockBits(
                new Rectangle(0, 0, largeurZoneTexte, hauteurZoneTexte),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, largeurZoneTexte, hauteurZoneTexte,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, dataTxt.Scan0);

            bmpTxt.UnlockBits(dataTxt);
        }

        public void dessiner()
        {
            GL.Enable(EnableCap.Texture2D); // S'assurer que les textures sont actives
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.PushMatrix();
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(pointA.X, pointA.Y);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(pointB.X, pointB.Y);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(pointC.X, pointC.Y);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(pointD.X, pointD.Y);
            GL.End();
            GL.PopMatrix();
        }
    }
}