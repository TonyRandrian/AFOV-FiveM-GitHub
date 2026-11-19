using metabolism.Shared;

namespace metabolism.Server.Domain
{
    public class AnimationRule
    {
        public BodyPartEnum BodyPart { get; set; }
        public InjuryState State { get; set; }
        public string MovementClipset { get; set; }
        public int Priority { get; set; }
    }
}
