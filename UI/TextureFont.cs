using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SquareBall
{
    struct TextureLetter
    {
        public ushort Code;
        public Rectangle Source;
        public short Top;

        public TextureLetter(ushort code, Rectangle area)
        {
            Code = code;
            Source = area;
            Top = 0;
        }
    }

    public class TextureFont
    {
        private Texture2D texture;
        private List<TextureLetter> letters;
        private int width;
        private int height;        
        private readonly ContentManager content;

        public int Kerning = 0;
        public float BaseScale = 1.0f;

        public TextureFont(ContentManager content, string textureName)
        {
            this.content = content;
            SetTexture(textureName);
        }

        public void SetTexture(string textureName)
        {            
            texture = content.Load<Texture2D>(textureName);

            string fileName = content.RootDirectory + "\\" + textureName + ".xml";

            if (!File.Exists(fileName))
                throw new Exception("Settings file does not exist - " + fileName);

            StreamReader r = new StreamReader(fileName);
            string xmlText = r.ReadToEnd();
            r.Close();
            
            while (true)
            {
                int i = xmlText.IndexOf("raw");
                if (i < 0)
                    break;

                xmlText = xmlText.Remove(i, 8);// delete raw="*", because xml can't read &,<,> etc symbols
            }
            StreamWriter w = new StreamWriter(fileName);
            w.Write(xmlText);
            w.Close();

            letters = new List<TextureLetter>();
            XmlReader reader = XmlReader.Create(fileName);

            reader.Read();
            width = int.Parse(reader.GetAttribute("width"));
            height = int.Parse(reader.GetAttribute("height"));

            letters.Add(new TextureLetter('\r', new Rectangle()));// \r
            letters.Add(new TextureLetter('\n', new Rectangle(0, 0, 0, height)));// \n
            letters.Add(new TextureLetter(32, new Rectangle(0, 0, width / 3, 0)));// пробел

            while (reader.Read())
            {
                if (reader.Name != "item")
                    continue;

                Rectangle rect = new Rectangle();
                rect.X = int.Parse(reader.GetAttribute("x"));
                rect.Y = int.Parse(reader.GetAttribute("y"));
                rect.Width = int.Parse(reader.GetAttribute("width"));
                rect.Height = int.Parse(reader.GetAttribute("height"));

                string code = reader.GetAttribute("ascii") ?? reader.GetAttribute("ucode");

                if (code == null)
                    throw new Exception("Wrong settings file format!");

                ushort c = ushort.Parse(code);

                TextureLetter letter = new TextureLetter(c, rect);
                letter.Top = short.Parse(reader.GetAttribute("top"));

                letters.Add(letter);
            }

            reader.Close();
        }

        public int TextHeight(string text)
        {
            int h = height;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                    h += height;
            }
            //h = (int)((float)h * 2.5f);
            h = (int)((float)h * BaseScale);
            return h;
        }

        public int TextWidth(string text)
        {
            if (text.Length == 0)
                return 0;

            int maxWidth = 0;

            int w = 0;
            for (int i = 0; i < text.Length; i++)
            {
                w += FindLetterByCode(text[i]).Source.Width - Kerning;
                if (text[i] == '\r' || i == text.Length - 1)
                {
                    if (w > maxWidth)
                        maxWidth = w;

                    w = 0;
                    continue;
                }
            }
            maxWidth = (int)((float)maxWidth * BaseScale);
            //maxWidth = (int)((float)maxWidth * 2.5f);
            return maxWidth;
        }

        public int MeasureString(string text, float scale)
        {
            Vector2 offset = Vector2.Zero;
            float maxOffsX = 0.0f;
            //scale *= 2.5f;
            foreach (char c in text)
            {
                if (c == '\r')
                {
                    offset.X = 0;
                    continue;
                }

                if (c == '\n')
                {
                    offset.X = 0;
                    offset.Y += (float)height * scale;
                    continue;
                }

                ushort code = c;

                TextureLetter letter = FindLetterByCode(code);
                
                Rectangle source = letter.Source;
                //offset.X += (float)letter.Source.Width-2.0f;
                //HACK: Magic numbers!
                offset.X += (float)letter.Source.Width - 1.0f - Kerning;
                if (offset.X > maxOffsX)
                    maxOffsX = offset.X;
            }
            return (int)(maxOffsX * scale * BaseScale);

        }

        public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color, Rectangle? scissor, float scale)
        {
            DrawString(spriteBatch, text, position, color, scissor, scale, false);
        }

        public void DrawString(SpriteBatch spriteBatch, string text, Vector2 position, Color color, Rectangle? scissor, float scale, bool quantize)
        {
            Vector2 offset = Vector2.Zero;
            //scale *= 2.5f;
            scale *= BaseScale;
            if (text != null)
            foreach (char c in text)
            {
                if (c == '\r')
                {
                    offset.X = 0;
                    continue;
                }

                if (c == '\n')
                {
                    offset.Y += height;
                    continue;
                }

                ushort code = c;
                TextureLetter letter = FindLetterByCode(code);
                Vector2 pos = position + offset;
                pos.Y += (float)letter.Top * scale;
                Rectangle source = letter.Source;
                float addX = (float)(letter.Source.Width - Kerning) * scale;
                if ((addX < 2.0f) && (c == ' '))
                    offset.X += Kerning;
                offset.X += addX;

                Rectangle destination = new Rectangle((int)pos.X, (int)pos.Y, (int)((float)source.Width * scale), (int)((float)source.Height * scale));

                if (scissor.HasValue)
                {
                    Rectangle bounds = scissor.Value;

                    if (!bounds.Intersects(destination))
                        continue;

                    if (destination.X < bounds.X)
                    {
                        source.Width -= bounds.X - destination.X;
                        source.X += bounds.X - destination.X;
                        pos.X = bounds.X;
                    }
                    if (destination.Right > bounds.Right)
                        source.Width -= destination.Right - bounds.Right;

                    if (destination.Y < bounds.Y)
                    {
                        source.Height -= bounds.Y - destination.Y;
                        source.Y += bounds.Y - destination.Y;
                        pos.Y = bounds.Y;
                    }
                    if (destination.Bottom > bounds.Bottom)
                        source.Height -= destination.Bottom - bounds.Bottom;
                }

                if (quantize)
                    pos = new Vector2((int)pos.X, (int)pos.Y);

                //spriteBatch.Draw(texture, pos, source, color);
                //spriteBatch.Draw(texture, destination, source, color, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
                spriteBatch.Draw(texture, pos, source, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
        }

        private TextureLetter FindLetterByCode(ushort code)
        {
            foreach (TextureLetter letter in letters)
            {
                if (letter.Code == code)
                    return letter;
            }

            //throw new Exception("Symbol which code is " + code + " does not exist in this font");
            return letters[0];
        }
    }
}