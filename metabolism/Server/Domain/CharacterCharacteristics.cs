namespace metabolism.Server.Domain
{
    public class CharacterCharacteristics
    {
        public Character Character { get; set; }
        public Characteristic Characteristic { get; set; }
        public float ActualValue { get; set; }
        public float MaxValue { get; set; }


        public CharacterCharacteristics() { }
    }
}
