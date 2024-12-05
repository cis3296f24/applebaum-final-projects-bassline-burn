namespace GameNamespace
{
    public class PlayerResult
    {
        public string playerName;  
        public float result;       
        public int position;       

        public PlayerResult(string name, float result, int position)
        {
            this.playerName = name;
            this.result = result;
            this.position = position;
        }

        public override string ToString()
        {
            return $"Name: {playerName}, Result: {result:F2}, Position: {position}";
        }
    }
}
