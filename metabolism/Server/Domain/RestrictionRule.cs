using metabolism.Shared;

namespace metabolism.Server.Domain
{
    public class RestrictionRule
    {
        public BodyPartEnum BodyPart { get; set; }
        public InjuryState State { get; set; }
        public ActionRestriction Restrictions { get; set; } = ActionRestriction.NONE;
    }
}