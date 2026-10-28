import { useState, useEffect } from "react";
import "./App.css";
import Gauge from "./components/Gauge";

function App() {
  const [visible, setVisible] = useState(true);

  useEffect(() => {
    const handler = (event) => {
      if (event.data.action === "setVisible") {
        setVisible(event.data.visible);
      }
    };
    window.addEventListener("message", handler);
    return () => window.removeEventListener("message", handler);
  }, []);

  if (!visible) return null;

  return (
    <div id="container">
      <div id="gauge-container">
        <Gauge label="PV" color="#F00" value="80" maxValue="100"></Gauge>
        <Gauge label="Stamina" color="#00F" value="70" maxValue="100"></Gauge>
        <Gauge label="Apnea" color="#03fcf8" value="100" maxValue="100"></Gauge>
      </div>
    </div>
  );
}

export default App;
