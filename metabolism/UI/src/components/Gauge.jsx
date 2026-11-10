import '../assets/css/gauge.css'

function Gauge({ label, color, value, maxValue }) {
  const maxValueWidth = 200;

  const maxValueStyle = {
    backgroundColor: "#dadada",
    width: maxValueWidth + "px",
    height: "10px",
  };

  const valueStyle = {
    backgroundColor: color,
    width: (value / maxValue) * maxValueWidth,
    height: "100%",
  };

  return (
    <div>
      {Math.floor(value)}/{Math.floor(maxValue)}
      <div className="gauge-group">
        {label}
        <div className="max-value" style={maxValueStyle}>
          <div className="value" style={valueStyle}></div>
        </div>
      </div>
    </div>
  );
}

export default Gauge;
