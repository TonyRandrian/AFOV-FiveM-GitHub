import { useState, useEffect } from "react";
import "./App.css";
import Gauge from "./components/Gauge";

function App() {
  const [isHudVisible, setHudVisible] = useState(false);
  const [isAllHudVisible, setAllHudVisible] = useState(true);
  const [gauges, setGauges] = useState([]);

  useEffect(() => {
    const handler = (event) => {
      if (event.data.action === "setHudVisible") {
        setHudVisible(event.data.value);
      }
      else if (event.data.action === "updateState") {
        setGauges(event.data.value);
      }
      else if (event.data.action === "setAllHudVisible") {
        setAllHudVisible(event.data.value);
      }
    };

      window.addEventListener("message", handler);
      return () => window.removeEventListener("message", handler);
    }, []);

  return (
    <div id="container">
      {
        isAllHudVisible && (
          <div id="main-gauge-container">
            <Gauge label="PV" color="#F00" value="100" maxValue="100"></Gauge>
            <Gauge label="Stamina" color="#00F" value="70" maxValue="100"></Gauge>
            <Gauge label="Apnea" color="#03fcf8" value="100" maxValue="100"></Gauge>
          </div>
        )
      }

      {isHudVisible && isAllHudVisible && (
        <div id="full-gauge-container">
          {gauges.map(gauge => <Gauge label={gauge.Gauge.Name} color="rgb(128, 126, 126)" value={gauge.ActualValue} maxValue={gauge.MaxValue}></Gauge>)}
        </div>
      )}
    </div>
  );
}

export default App;
