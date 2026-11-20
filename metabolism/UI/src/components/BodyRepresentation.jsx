
function BodyRepresentation() {
    const bodyPartStyle = {
        fill: "rgb(216, 216, 216)",
        stroke: "rgb(0, 0, 0)",
        strokeWidth: 1
    };

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
                style={bodyPartStyle}
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
                style={bodyPartStyle}
            />

            <rect
                id="RIGHT_LEG"
                x="210.197"
                y="378.981"
                width="29.702"
                height="97.102"
                style={bodyPartStyle}
            />

            <rect
                id="LEFT_LEG"
                x="254.749"
                y="378.41"
                width="29.702"
                height="97.102"
                style={bodyPartStyle}
            />

            <rect
                id="RIGHT_ARM"
                x="162.216"
                y="205.912"
                width="29.702"
                height="97.102"
                style={bodyPartStyle}
            />

            <rect
                id="LEFT_ARM"
                x="305.013"
                y="207.055"
                width="29.702"
                height="97.102"
                style={bodyPartStyle}
            />
        </svg>
    );
}

export default BodyRepresentation;
