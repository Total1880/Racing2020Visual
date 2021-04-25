using Microsoft.Xna.Framework.Graphics;

namespace Racing2020Visual
{
    public class Cyclist
    {
        private float _cyclistPositionX;
        private float _cyclistPositionY = 600f;
        private float _cyclistSpeedHorizontal;
        private float _cyclistSpeedUp;
        private float _cyclistSpeedDown;
        private Texture2D _cyclistTexture;
        private string _name;

        public float CyclistPositionX { get => _cyclistPositionX; set { _cyclistPositionX = value; } }
        public float CyclistPositionY { get => _cyclistPositionY; set { _cyclistPositionY = value; } }
        public Texture2D CyclistTexture { get => _cyclistTexture; set { _cyclistTexture = value; } }
        public float CyclistSpeedHorizontal { get => _cyclistSpeedHorizontal; set { _cyclistSpeedHorizontal = value; } }
        public float CyclistSpeedUp { get => _cyclistSpeedUp; set { _cyclistSpeedUp = value; } }
        public float CyclistSpeedDown { get => _cyclistSpeedDown; set { _cyclistSpeedDown = value; } }
        public string Name { get => _name; set { _name = value; } }

        public Cyclist(float speedHorizontal, float speedUp, float speedDown, string name)
        {
            CyclistSpeedHorizontal = speedHorizontal;
            CyclistSpeedUp = speedUp;
            CyclistSpeedDown = speedDown;
            Name = name;
        }
    }
}
