


namespace Tenacity.Battles.Data.Field
{
    public abstract class FieldCreator
    {
        public abstract FieldCreationType[][] Generate(); 
    }

    public sealed class RhombusFieldGenerator : FieldCreator
    {
        private int _sideSize;
        
        
        public RhombusFieldGenerator(int sideSize)
        {
            _sideSize = sideSize;
        }
        
        
        public override FieldCreationType[][] Generate()
        {
            var field = new FieldCreationType[(_sideSize * 2) - 1][];
            var fieldSizeChange = -1;

            for (int i = 0; i < field.Length; i++)
            {
                fieldSizeChange = (i >= _sideSize) ? fieldSizeChange - 1 : fieldSizeChange + 1;
                field[i] = new FieldCreationType[_sideSize + fieldSizeChange];
            }
            field[0][0] = field[^1][field[^1].Length - 1] = FieldCreationType.Player;
            
            return field;
        }
    }
}