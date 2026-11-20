
function BodyRepresentation({bodyParts}) {
    const bodyPartStyle = (bodyPartName) => ({
        fill: getPartColor(bodyPartName),
        stroke: "rgb(0, 0, 0)",
        strokeWidth: 1
    });

    function getHealthColor(health, maxHealth) {
        if (maxHealth === 0) return "rgba(40, 0, 169, 0.93)";

        const damage = 1 - (health / maxHealth)
        const intensity = Math.round(255 * damage);

        return `rgb(255, ${255 - intensity}, ${255 - intensity})`
    }

    function getPartColor(bodyPartName) {
        const part = bodyParts.find(p => p.BodyPartName === bodyPartName)

        if (!part) return "rgb(0, 255, 4)";

        return getHealthColor(part.BodyPart.Health, part.BodyPart.BodyPart.MaxHealth);
    }

    return (
        <svg
            id="svg2"
            viewBox="0 0 500 670"
            xmlns="http://www.w3.org/2000/svg"
            width="250"
            height="335"
            preserveAspectRatio="xMidYMid meet"
        >
            <ellipse
                id="HEAD"
                style={bodyPartStyle("HEAD")}
                cx="246.181"
                cy="148.221"
                rx="39.411"
                ry="36.556"
            />

            <rect
                id="TORSO"
                x="210.197"
                y="205.341"
                width="73.111"
                height="156.505"
                style={bodyPartStyle("TORSO")}
            />

            <rect
                id="RIGHT_LEG"
                x="210.197"
                y="378.981"
                width="29.702"
                height="97.102"
                style={bodyPartStyle("RIGHT_LEG")}
            />

            <rect
                id="LEFT_LEG"
                x="254.749"
                y="378.41"
                width="29.702"
                height="97.102"
                style={bodyPartStyle("LEFT_LEG")}
            />

            <rect
                id="RIGHT_ARM"
                x="162.216"
                y="205.912"
                width="29.702"
                height="97.102"
                style={bodyPartStyle("RIGHT_ARM")}
            />

            <rect
                id="LEFT_ARM"
                x="305.013"
                y="207.055"
                width="29.702"
                height="97.102"
                style={bodyPartStyle("LEFT_ARM")}
            />
        </svg>
    );
}

export default BodyRepresentation;
