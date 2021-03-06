using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

// TODO: replace this with the type you want to write out.
using TWrite = AWGP.InputManager;

namespace AWGP
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class InputManagerWriters : ContentTypeWriter<InputManager>
    {
        protected override void Write(ContentWriter output, InputManager value)
        {
            // TODO: write the specified value to the output ContentWriter.
            List<AWGP.Player> players = new List<Player>();
            players = value.players;
            AWGP.PlayerMoves[] moves;          
            foreach (Player p in players)
            {
                output.Write(p.PlayerName.ToString());
                output.Write(p.Controller.ToString());
                moves = p.GetMoves();
                for (int i = 0; i < moves.Length; i++)
                {
                    output.Write(moves[i].Control.ToString());
                    output.Write(moves[i].Move.ToString());
                }

            }
            output.WriteObject<List<Player>>(value.players);

        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // TODO: change this to the name of your ContentTypeReader
            // class which will be used to load this data.
            return "AWGP.InputManagerReader, AWGP";
        }
    }
}
