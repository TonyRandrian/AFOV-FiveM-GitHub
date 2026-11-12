using metabolism.Server.Domain;

namespace metabolism.Server.Application.Services
{
    public class BoneService
    {

        public BodyPartEnum ToBodyPart(int boneId)
        {
            switch (boneId)
            {
                case 31086:
                    return BodyPartEnum.HEAD;

                // Torso / Colonne / Neck / Pelvis
                case 0:
                case 23553:
                case 24816:
                case 24817:
                case 24818:
                case 39317:
                case 11816:
                    return BodyPartEnum.TORSO;

                case 64729:
                case 45509:
                case 61163:
                case 18905:
                    return BodyPartEnum.LEFT_ARM;

                case 10706:
                case 40269:
                case 28252:
                case 57005:
                    return BodyPartEnum.RIGHT_ARM;

                case 58271:
                case 63931:
                case 14201:
                    return BodyPartEnum.LEFT_LEG;

                case 51826:
                case 36864:
                case 52301:
                    return BodyPartEnum.RIGHT_LEG;

                // Fallback
                default:
                    return BodyPartEnum.TORSO;
            }
        }
    }
}
