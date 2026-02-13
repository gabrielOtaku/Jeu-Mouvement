using OpenTK.Audio.OpenAL;
using System;
using System.IO;

namespace BaseOpenTK
{
    class FichierWAV
    {
        // ****************************************************************
        // Attributs
        string nomFichier;
        int nbrCanaux;
        int frequence;
        int nbrBits;
        int qteDonneesSonores;
        byte[] donneesSonores;

        // ****************************************************************
        // Constructeur
        public FichierWAV(string nomFichier)
        {
            this.nomFichier = nomFichier;
            // Utilisation de 'using' pour garantir la fermeture du fichier
            using (Stream fichierAudio = File.Open(nomFichier, FileMode.Open))
            {
                chargerFichier(fichierAudio);
            }
        }

        // ****************************************************************
        // Méthode de chargement (AMÉLIORÉE)
        private void chargerFichier(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);

            // 1. Vérification du Header RIFF
            string signature = new string(reader.ReadChars(4));
            if (signature != "RIFF")
                throw new NotSupportedException("Ce n'est pas un fichier RIFF.");

            reader.ReadInt32(); // On ignore la taille totale du fichier

            string format = new string(reader.ReadChars(4));
            if (format != "WAVE")
                throw new NotSupportedException("Ce n'est pas un format WAVE.");

            // 2. Parcours des blocs (Chunks) jusqu'à trouver "data"
            // Cette boucle rend le code compatible avec tous les fichiers WAV
            bool fmtTrouve = false;
            bool dataTrouve = false;

            while (stream.Position < stream.Length && !dataTrouve)
            {
                // Lecture de l'identifiant du bloc (ex: "fmt ", "data", "LIST")
                string chunkID = new string(reader.ReadChars(4));
                int chunkSize = reader.ReadInt32();

                // Traitement selon le type de bloc
                // Note : "fmt " a souvent un espace à la fin, on utilise Trim() pour sécuriser
                if (chunkID.Trim() == "fmt")
                {
                    // --- Lecture du format audio ---
                    int formatAudio = reader.ReadInt16(); // Devrait être 1 (PCM)
                    nbrCanaux = reader.ReadInt16();
                    frequence = reader.ReadInt32();
                    reader.ReadInt32(); // ByteRate (ignoré)
                    reader.ReadInt16(); // BlockAlign (ignoré)
                    nbrBits = reader.ReadInt16();

                    fmtTrouve = true;

                    // Si le chunk fmt est plus grand que 16 (ex: 18 ou 40 octets), on saute le reste
                    int bytesLus = 16;
                    if (chunkSize > bytesLus)
                    {
                        reader.ReadBytes(chunkSize - bytesLus);
                    }
                }
                else if (chunkID == "data")
                {
                    // --- Lecture des données sonores ---
                    qteDonneesSonores = chunkSize;
                    donneesSonores = reader.ReadBytes(qteDonneesSonores);
                    dataTrouve = true;
                }
                else
                {
                    // --- Bloc inconnu (ex: métadonnées, info artiste) ---
                    // On le saute simplement pour ne pas faire planter le programme
                    if (chunkSize > 0)
                    {
                        // Attention aux fichiers corrompus ou padding impair, 
                        // mais pour ce labo, un simple ReadBytes suffit.
                        // On s'assure de ne pas dépasser la fin du flux.
                        if (stream.Position + chunkSize <= stream.Length)
                            reader.ReadBytes(chunkSize);
                        else
                            break; // Fin de fichier inattendue
                    }
                }
            }

            if (!fmtTrouve) throw new Exception("Erreur : Bloc 'fmt' introuvable.");
            if (!dataTrouve) throw new Exception("Erreur : Bloc 'data' introuvable.");
        }

        // ****************************************************************
        // Méthodes publiques (Identiques au PDF pour la compatibilité)
        public ALFormat getFormatSonAL()
        {
            ALFormat format;
            switch (nbrCanaux)
            {
                case 1:
                    format = (nbrBits == 8 ? ALFormat.Mono8 : ALFormat.Mono16);
                    break;
                case 2:
                    format = (nbrBits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16);
                    break;
                default:
                    throw new NotSupportedException("Nombre de canaux non supporté (" + nbrCanaux + ")");
            }
            return format;
        }

        public byte[] getDonneesSonores()
        {
            return donneesSonores;
        }

        public int getQteDonneesSonores()
        {
            return qteDonneesSonores;
        }

        public int getFrequence()
        {
            return frequence;
        }
    }
}