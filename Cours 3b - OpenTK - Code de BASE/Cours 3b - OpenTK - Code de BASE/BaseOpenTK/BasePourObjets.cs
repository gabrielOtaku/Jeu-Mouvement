using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO; // Ajouté pour File.Exists
using System;

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
        // GestionTexture (CORRIGÉ)
        private void chargerTexture()
        {
            // 1. Vérification de sécurité
            if (!File.Exists(nomTexture))
            {
                throw new FileNotFoundException("Texture introuvable: " + nomTexture);
            }

            // 2. Génération de l'ID Texture
            textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            // 3. Chargement et Verrouillage (Tout se passe ici)
            using (Bitmap bmpImage = new Bitmap(nomTexture))
            {
                // On verrouille les bits en mémoire
                Rectangle rectangle = new Rectangle(0, 0, bmpImage.Width, bmpImage.Height);

                BitmapData bmpData = bmpImage.LockBits(
                    rectangle,
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb // On garde ton format 24 bits
                );

                // 4. Envoi à OpenGL (PENDANT que c'est verrouillé)
                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgb,
                    bmpData.Width,
                    bmpData.Height,
                    0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgr, // Bgr correspond au Format24bppRgb de Windows
                    PixelType.UnsignedByte,
                    bmpData.Scan0
                );

                // 5. Déverrouillage (Maintenant on peut libérer)
                bmpImage.UnlockBits(bmpData);
            }

            // 6. Paramètres de filtre (Indispensables pour l'affichage)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // 7. Génération des Mipmaps
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        // NOTE : La méthode chargerImage a été supprimée car elle est intégrée ci-dessus
        // pour éviter l'erreur de pointeur invalide.

        private void setCoordonneesTextureTriangle()
        {
            coordonneesTextures = new Vector2[3];
            coordonneesTextures[0] = new Vector2(0.5f, 1.0f); // Ajusté pour centrer un peu mieux (optionnel)
            coordonneesTextures[1] = new Vector2(1.0f, 0.0f);
            coordonneesTextures[2] = new Vector2(0.0f, 0.0f);
        }

        private void setCoordonneesTextureCarre()
        {
            coordonneesTextures = new Vector2[4];
            // Correspondance typique (Haut-Gauche, Haut-Droite, Bas-Droite, Bas-Gauche)
            // Attention: OpenGL a (0,0) en bas à gauche, Bitmap en haut à gauche.
            // Il faut parfois inverser le Y selon votre configuration de projection.
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
            // On s'assure que les textures sont activées
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            // Important: remettre la couleur à Blanc sinon la texture sera teintée
            GL.Color3(Color.White);

            GL.Begin(typeDessin);
            for (int i = 0; i < listePoints.Length; i++)
            {
                // Vérification de sécurité pour éviter le crash si les tableaux ont des tailles différentes
                if (i < coordonneesTextures.Length)
                    GL.TexCoord2(coordonneesTextures[i]);

                GL.Vertex2(listePoints[i].X, listePoints[i].Y);
            }
            GL.End();
        }
    }
}