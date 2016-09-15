using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Square_Ball_Mono.GameObjects
{
    class Table
    {
        List<Card> cards = new List<Card>();
        Game game;
        SpriteBatch batch;

        public Table(Game _game, SpriteBatch _batch)
        {
            game = _game;
            batch = _batch;
        }

        public void Update(GameTime time)
        {
            foreach (Card c in cards)
                c.Draw(time);
        }

        public void addCard()
        {
            cards.Add(new Card(game, batch));
        }
        
    }
}
