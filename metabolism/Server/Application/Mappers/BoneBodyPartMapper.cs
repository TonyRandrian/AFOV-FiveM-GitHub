using metabolism.Server.Domain;
using System.Collections.Generic;

namespace metabolism.Server.Application.Mappers
{
    public class BoneBodyPartMapper
    {
        private const BodyPartEnum DefaultBodyPart = BodyPartEnum.TORSO;


        private static readonly Dictionary<int, BodyPartEnum> BoneToBodyPartMap =
            new Dictionary<int, BodyPartEnum>
            {
                { 31086, BodyPartEnum.HEAD },

                { 0, BodyPartEnum.TORSO },
                { 23553, BodyPartEnum.TORSO },
                { 24816, BodyPartEnum.TORSO },
                { 24817, BodyPartEnum.TORSO },
                { 24818, BodyPartEnum.TORSO },
                { 39317, BodyPartEnum.TORSO },
                { 11816, BodyPartEnum.TORSO },

                { 64729, BodyPartEnum.LEFT_ARM },
                { 45509, BodyPartEnum.LEFT_ARM },
                { 61163, BodyPartEnum.LEFT_ARM },
                { 18905, BodyPartEnum.LEFT_ARM },

                { 10706, BodyPartEnum.RIGHT_ARM },
                { 40269, BodyPartEnum.RIGHT_ARM },
                { 28252, BodyPartEnum.RIGHT_ARM },
                { 57005, BodyPartEnum.RIGHT_ARM },

                { 58271, BodyPartEnum.LEFT_LEG },
                { 63931, BodyPartEnum.LEFT_LEG },
                { 14201, BodyPartEnum.LEFT_LEG },

                { 51826, BodyPartEnum.RIGHT_LEG },
                { 36864, BodyPartEnum.RIGHT_LEG },
                { 52301, BodyPartEnum.RIGHT_LEG }
            };

        public BodyPartEnum ToBodyPart(int boneId)
        {
            return BoneToBodyPartMap.TryGetValue(boneId, out BodyPartEnum bodyPart)
                ? bodyPart
                : DefaultBodyPart;
        }
    }
}
