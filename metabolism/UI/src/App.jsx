import { useState, useEffect } from "react";
import "./App.css";
import Gauge from "./components/Gauge";

function App() {
  const [visible, setVisible] = useState(false);

  useEffect(() => {
    const handler = (event) => {
      if (event.data.action === "setVisible") {
        setVisible(event.data.visible);
      }
    };
    window.addEventListener("message", handler);
    return () => window.removeEventListener("message", handler);
  }, []);

  return (
    <div id="container">
      <div id="main-gauge-container">
        <Gauge label="PV" color="#F00" value="80" maxValue="100"></Gauge>
        <Gauge label="Stamina" color="#00F" value="70" maxValue="100"></Gauge>
        <Gauge label="Apnea" color="#03fcf8" value="100" maxValue="100"></Gauge>
      </div>

      {visible && (
        <div id="full-gauge-container">
          <Gauge label="PV" color="rgb(128, 126, 126)" value="100" maxValue="100"></Gauge>
          <Gauge label="Stamina" color="rgb(128, 126, 126)" value="80" maxValue="100"></Gauge>
          <Gauge label="Apnea" color="rgb(128, 126, 126)" value="100" maxValue="100"></Gauge>
          <Gauge label="Hunger" color="rgb(128, 126, 126)" value="70" maxValue="100"></Gauge>
          <Gauge label="Thirst" color="rgb(128, 126, 126)" value="50" maxValue="100"></Gauge>
          <Gauge label="Sleep" color="rgb(128, 126, 126)" value="80" maxValue="100"></Gauge>
          <Gauge label="Hygiene" color="rgb(128, 126, 126)" value="40" maxValue="100"></Gauge>
          <Gauge label="Needs" color="rgb(128, 126, 126)" value="100" maxValue="100"></Gauge>
        </div>
      )}
    </div>
  );
}

export default App;
