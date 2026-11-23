using metabolism.Shared;

namespace metabolism.Server.Domain
{
    public class AnimationRule
    {
        public BodyPartEnum BodyPart { get; set; }
        public InjuryState State { get; set; }
        public string Dictionary { get; set; }
        public string Animation { get; set; }
        public string EnterAnimation { get; set; }
        public string ExitAnimation { get; set; }
        public AnimationPlaybackTypeEnum PlaybackType { get; set; }
        public string MovementClipset { get; set; }
        public int Interval { get; set; }
        public int Priority { get; set; }
    }
}
