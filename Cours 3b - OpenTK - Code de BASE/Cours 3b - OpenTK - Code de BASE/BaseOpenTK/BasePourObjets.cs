using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace BaseOpenTK
{
    internal abstract class BasePourObjets
    {
        // ****************************************************************
        // Attributs
        protected Vector2[] listePoints;
        protected Vector2[] coordonneesTextures;
        protected int textureID;
        protected string nomTexture;

        // ****************************************************************
        // Constructeurs et Initialisation

        // --------------------------
        // Constructeur pour triangle
        public BasePourObjets(string nomTexture, Vector2 a, Vector2 b, Vector2 c)
        {
            this.listePoints = new Vector2[3];
            this.listePoints[0] = a;
            this.listePoints[1] = b;
            this.listePoints[2] = c;
            setCoordonneesTextureTriangle();
            init(nomTexture);
        }

        // -----------------------------------
        // Constructeur pour rectangles/carrés
        public BasePourObjets(string nomTexture, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            this.listePoints = new Vector2[4];
            this.listePoints[0] = a;
            this.listePoints[1] = b;
            this.listePoints[2] = c;
            this.listePoints[3] = d;
            setCoordonneesTextureCarre();
            init(nomTexture);
        }

        private void init(string nomTexture)
        {
            this.nomTexture = nomTexture;
            chargerTexture();
        }

        // ****************************************************************
        // GestionTexture
        private void chargerTexture()
        {
            GL.GenTextures(1, out textureID);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            BitmapData textureData = chargerImage(nomTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, textureData.Width, textureData.Height, 0,
                                        OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, textureData.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        private BitmapData chargerImage(string nomImage)
        {
            Bitmap bmpImage = new Bitmap(nomImage);
            Rectangle rectangle = new Rectangle(0, 0, bmpImage.Width, bmpImage.Height);
            BitmapData bmpData = bmpImage.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bmpImage.UnlockBits(bmpData);
            return bmpData;
        }
        private void setCoordonneesTextureTriangle()
        {
            coordonneesTextures = new Vector2[3];
            coordonneesTextures[0] = new Vector2(0.05f, 1.0f);
            coordonneesTextures[1] = new Vector2(1.0f, 0.95f);
            coordonneesTextures[2] = new Vector2(0.55f, 0.0f);
        }
        private void setCoordonneesTextureCarre()
        {
            coordonneesTextures = new Vector2[4];
            coordonneesTextures[0] = new Vector2(0.0f, 1.0f);
            coordonneesTextures[1] = new Vector2(1.0f, 1.0f);
            coordonneesTextures[2] = new Vector2(1.0f, 0.0f);
            coordonneesTextures[3] = new Vector2(0.0f, 0.0f);
        }

        // ****************************************************************
        // Gestion Affichage
        abstract public void update();
        public void dessiner(PrimitiveType typeDessin)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.Begin(typeDessin);
            for (int i = 0; i < listePoints.Length; i++)
            {
                GL.TexCoord2(coordonneesTextures[i]);
                GL.Vertex2(listePoints[i].X, listePoints[i].Y);
            }
            GL.End();
        }
    }
}
