import { useState, useEffect } from "react";
import "./App.css";
import Gauge from "./components/Gauge";

function App() {
  const [isHudVisible, setHudVisible] = useState(false);
  const [isAllHudVisible, setAllHudVisible] = useState(true);
  const [gauges, setGauges] = useState(null);

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
        gauges && isAllHudVisible && (
          <div id="main-gauge-container">
            {
              gauges.Permanents.map(gauge => (
                <Gauge
                  key={gauge.Name}
                  label={gauge.Name}
                  color="rgb(0, 85, 0)"
                  value={gauge.Value}
                  maxValue={gauge.Max}
                />
              ))
            }
          </div>
        )
      }

      {
        gauges && isHudVisible && isAllHudVisible && (
          <div id="full-gauge-container">
            {
              gauges.Hiddens.map(gauge => (
                <Gauge
                  key={gauge.Name}
                  label={gauge.Name}
                  color="rgb(128, 126, 126)"
                  value={gauge.Value}
                  maxValue={gauge.Max} />
              ))
            }
          </div>
        )
      }
    </div>
  );
}

export default App;
