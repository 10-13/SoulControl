using SoulControl.Map;
using SoulControl.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SoulControl.Utils
{
    public class ModelGenerator
    {
        private Model _model;
        public Tile DrawTile { get; set; } = new Tile();
        private TextWriter log = null;
        private void Log(string str)
        {
            log?.WriteLine(str);
        }

        public Model Model { get => _model; }

        public ModelGenerator(TextWriter log = null)
        {
            _model = new Model();
            _model.ActualField = new Map.Map(_model);
        }

        public void DrawMap(int x,int y, int w, int h)
        {
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    _model.ActualField[x + i, j + y] = DrawTile.Clone() as Tile;
                }
        }
        public void LoadDrawTile(string FilePath)
        {
            XmlSerializer f = new XmlSerializer(typeof(Tile));
            try
            {
                DrawTile = f.Deserialize(new FileStream(FilePath, FileMode.Open)) as Tile;
                DrawTile.LinkModel(_model);
            }
            catch(Exception ex)
            {
                Log(ex.Message);
            }
        }
        public void LoadPlayer(string FilePath)
        {
            XmlSerializer f = new XmlSerializer(typeof(Player));
            try
            {
                _model.Player = f.Deserialize(new FileStream(FilePath, FileMode.Open)) as Player;
                _model.Player.LinkModel(_model);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }
        public void SetClearQueue(bool a,bool b)
        {
            if (_model.Options == null)
                _model.Options = new ModelOptions.Options();
            if (_model.Options.Request == null)
                _model.Options.Request = new ModelOptions.Detailed.RequestOptions();
            _model.Options.Request.ClearQueueOnTick = a;
            _model.Options.Request.ClearQueueOnRequest = b;
        }
    }
}
